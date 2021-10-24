using System;
using System.Collections;
using System.Collections.Generic;
using GameMechanics.AsteroidMechanics;
using GameMechanics.PlayerMechanics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    [RequireComponent(typeof(InputMechanics))]
    [RequireComponent(
        typeof(CommonAsteroidMechanics),
        typeof(GoldenAsteroidMechanics),
        typeof(ExplosionMechanics)
    )]
    public class MainMechanics : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        private Animator _animatorMainCamera;

        [SerializeField] private GameObject planetPrefab;
        private Player _player;
        public Player Player { get; private set; }

        [SerializeField] private GameObject explosionPlanetPrefab;
        private GameObject _planet;

        [SerializeField] private GameObject damageTextPrefab;
        [SerializeField] private GameObject damageTextParent;
        private DamageText[] _damageTextArray = new DamageText[15];

        [SerializeField] private GameObject spaceshipPrefab;
        private List<Spaceship> _spaceshipList = new List<Spaceship>();

        private InputMechanics _inputMechanics;
        private CommonAsteroidMechanics _commonAsteroidMechanics;
        private GoldenAsteroidMechanics _goldenAsteroidMechanics;
        private ExplosionMechanics _explosionMechanics;

        private int _damageTextIndex = 0;
        private int _numberSpaceships = 1;
        private int _totalKilledAsteroids = 0;

        private int _asteroidLayerIndex;

        public event Action GameOver;
        public event Action<string> ChangeScoreGoldenMode;
        public event Action<string, bool> ChangeTimeGoldenMode;

        public event Action<int> AddedMoney;

        private Coroutine _coroutineCommonAsteroid;
        private Coroutine _coroutineGoldenAsteroid;
        private static readonly int GoldenMode = Animator.StringToHash("GoldenMode");
        private static readonly int IsGoldenMode = Animator.StringToHash("isGoldenMode");

        //Start Of Initialization
        private void Start()
        {
            _asteroidLayerIndex = 1 << LayerMask.NameToLayer("Asteroid");
            _explosionMechanics = GetComponent<ExplosionMechanics>();
            
            _commonAsteroidMechanics = GetComponent<CommonAsteroidMechanics>();
            _commonAsteroidMechanics.AsteroidDroppedMoney += IncreasePlayerMoney;
            _commonAsteroidMechanics.AsteroidExploded += _explosionMechanics.EnableExplosion;
            _commonAsteroidMechanics.GoldenAsteroidFell += StartGoldenGameMode;

            _goldenAsteroidMechanics = GetComponent<GoldenAsteroidMechanics>();
            _goldenAsteroidMechanics.AsteroidDroppedMoney += IncreasePlayerMoneyGoldenMode;
            _goldenAsteroidMechanics.AsteroidExploded += _explosionMechanics.EnableExplosion;
            _goldenAsteroidMechanics.GoldenAsteroidDied += ChangeScore;

            _animatorMainCamera = mainCamera.GetComponent<Animator>();
            
            InitInputMechanics();
            InitSpaceships();
            InitDamageText();

            InitPlanet();

            _coroutineCommonAsteroid = StartCoroutine(_commonAsteroidMechanics.IncreaseAsteroidIndex());
        }

        private void StartGoldenGameMode()
        {
            _animatorMainCamera.SetTrigger(GoldenMode);
            _totalKilledAsteroids = 0;
            _commonAsteroidMechanics.DisableAsteroids();
            StopCoroutine(_coroutineCommonAsteroid);
            _coroutineGoldenAsteroid = StartCoroutine(_goldenAsteroidMechanics.IncreaseGoldAsteroidIndex());
            StartCoroutine(LifeOfGoldenMode());
        }

        private void ChangeScore()
        {
            _totalKilledAsteroids += 1;
            ChangeScoreGoldenMode?.Invoke(_totalKilledAsteroids.ToString());
        }

        private IEnumerator LifeOfGoldenMode()
        {
            _animatorMainCamera.SetBool(IsGoldenMode, true);
            int time = 10;
            while (time > 0)
            {
                ChangeTimeGoldenMode?.Invoke(time.ToString(), true);
                time -= 1;
                yield return new WaitForSeconds(1f);
            }
            ChangeTimeGoldenMode?.Invoke(time.ToString(), false);
            _goldenAsteroidMechanics.DisableGoldenAsteroids();
            StopCoroutine(_coroutineGoldenAsteroid);
            _animatorMainCamera.SetBool(IsGoldenMode, false);
            _coroutineCommonAsteroid = StartCoroutine(_commonAsteroidMechanics.IncreaseAsteroidIndex());
        }

        private void IncreasePlayerMoney(int money)
        {
            _player.Money.Amount += money;
            AddedMoney?.Invoke(money);
        }
        
        private void IncreasePlayerMoneyGoldenMode(int money)
        {
            money *= _totalKilledAsteroids;
            _totalKilledAsteroids = 0;
            _player.Money.Amount += money;
            AddedMoney?.Invoke(money);
        }

        private void InitInputMechanics()
        {
            _inputMechanics = GetComponent<InputMechanics>();
            _inputMechanics.OnTouch += CheckTouchPosition;
            _inputMechanics.OnClick += CheckClickPosition;
        }

        private void InitSpaceships()
        {
            _spaceshipList.Add(Instantiate(spaceshipPrefab).GetComponent<Spaceship>());
            _spaceshipList[0].UpLvl();
            _spaceshipList[0].SetPosition(new Vector2(-2f, -1.1f));
        }

        private void InitDamageText()
        {
            for (int i = 0; i < _damageTextArray.Length; i++)
            {
                _damageTextArray[i] =
                    Instantiate(damageTextPrefab, damageTextParent.transform).GetComponent<DamageText>();
            }
        }

        private void InitPlanet()
        {
            _planet = Instantiate(planetPrefab);
            _planet.GetComponent<Transform>().position = new Vector2(-1.5f, -3.7f);
            _player = _planet.GetComponent<Player>();
            Player = _player;
            _player.Died += StopGame;
        }
        //End Of Initialization

        
        //Start Of Player Click Control
        private void CheckTouchPosition()
        {
            CheckPosition(Input.GetTouch(0).position);
        }

        private void CheckClickPosition()
        {
            CheckPosition(Input.mousePosition);
        }

        private void CheckPosition(Vector3 pos)
        {
            Vector3 touchWorldPos = mainCamera.ScreenToWorldPoint(pos);
            RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector2.zero, Mathf.Infinity, _asteroidLayerIndex);

            if (hit && hit.collider)
            {
                hit.collider.GetComponent<Asteroid>().TakeDamage(_player.Attack.Amount);
                ShowDamageText(_player.Attack.Amount, touchWorldPos);
                SpaceshipsShoot(touchWorldPos);
            }
        }

        private void SpaceshipsShoot(Vector2 posEnemy)
        {
            foreach (var spaceship in _spaceshipList)
            {
                spaceship.ShotLaser(posEnemy);
            }
        }

        private void ShowDamageText(int damage, Vector2 pos)
        {
            _damageTextArray[_damageTextIndex].EnableAnimation(damage.ToString(), pos);

            if (_damageTextIndex < _damageTextArray.Length - 1)
            {
                _damageTextIndex += 1;
            }
            else
            {
                _damageTextIndex = 0;
            }
        }
        //End Of Player Click Control

        //Start Of Player Level Up Control
        public void SpaceshipLvlUp()
        {
            foreach (Spaceship spaceship in _spaceshipList)
            {
                spaceship.UpLvl();
            }
        }

        public void AddSpaceship()
        {
            if (_numberSpaceships == 1 || _numberSpaceships == 2)
            {
                _spaceshipList.Add(Instantiate(spaceshipPrefab).GetComponent<Spaceship>());
                _spaceshipList[_numberSpaceships].UpLvl();
                if (_numberSpaceships == 1)
                    _spaceshipList[_numberSpaceships].SetPosition(new Vector2(1f, -2.5f));
                if (_numberSpaceships == 2)
                    _spaceshipList[_numberSpaceships].SetPosition(new Vector2(-0.3f, -1.3f));
                _numberSpaceships += 1;
            }
        }
        //End Of Player Level Up Control

        
        //Start Of Game State Control
        private void StopGame()
        {
            StartCoroutine(ExplosionPlanet());
        }

        public void ReStart()
        {
            GameStateHelper.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //End Of Game State Control

        private IEnumerator ExplosionPlanet()
        {
            Instantiate(explosionPlanetPrefab).transform.position = new Vector2(-1.5f, -3.7f);
            _planet.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            GameOver?.Invoke();
            GameStateHelper.Pause();
        }

        private void OnDestroy()
        {
            _commonAsteroidMechanics.AsteroidDroppedMoney -= IncreasePlayerMoney;
            _commonAsteroidMechanics.AsteroidExploded -= _explosionMechanics.EnableExplosion;
            _commonAsteroidMechanics.GoldenAsteroidFell -= StartGoldenGameMode;
            
            _goldenAsteroidMechanics.AsteroidDroppedMoney -= IncreasePlayerMoneyGoldenMode;
            _goldenAsteroidMechanics.AsteroidExploded -= _explosionMechanics.EnableExplosion;
            _goldenAsteroidMechanics.GoldenAsteroidDied -= ChangeScore;
            
            _player.Died -= StopGame;
            _inputMechanics.OnTouch -= CheckTouchPosition;
            _inputMechanics.OnClick -= CheckClickPosition;
        }
    }
}
