using System;
using System.Collections;
using System.Collections.Generic;
using GameMechanics.AsteroidMechanics;
using GameMechanics.AsteroidMechanics.CommonAsteroid;
using GameMechanics.AsteroidMechanics.DamageDisplay;
using GameMechanics.AsteroidMechanics.ExplosionOfAsteroid;
using GameMechanics.PlayerMechanics;
using GameMechanics.PlayerMechanics.Planet;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameMechanics
{
    [RequireComponent(typeof(InputMechanics))]
    [RequireComponent(typeof(CommonAsteroidMechanics),
        typeof(GoldenAsteroidMechanics),
        typeof(ExplosionMechanics))]
    public class MainMechanics : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        private Animator _animatorMainCamera;
        
        [SerializeField] private GameObject damageTextPrefab;
        [SerializeField] private GameObject damageTextParent;
        private DamageText[] _damageTextArray = new DamageText[15];

        [SerializeField] private GameObject playerManagerPrefab;
        [SerializeField] private GameObject explosionPlanetPrefab;
        public PlayerManager PlayerManager { get; private set; }
        
        [SerializeField] private GameObject spaceshipPrefab;
        private List<Spaceship> _spaceshipList = new List<Spaceship>();
        
        private int _damageTextIndex = 0;
        private int _asteroidLayerIndex;
        private int _numberSpaceships = 1;
        private int _goldenModeIndex = 0;
        
        private InputMechanics _inputMechanics;
        private CommonAsteroidMechanics _commonAsteroidMechanics;
        private GoldenAsteroidMechanics _goldenAsteroidMechanics;
        private ExplosionMechanics _explosionMechanics;
        
        private Coroutine _coroutineCommonAsteroid;
        private Coroutine _coroutineGoldenAsteroid;
        private static readonly int GoldenMode = Animator.StringToHash("GoldenMode");
        private static readonly int IsGoldenMode = Animator.StringToHash("isGoldenMode");

        
        public event Action GameOver;
        public event Action<string> ChangeScoreGoldenMode;
        public event Action<string, bool> ChangeTimeGoldenMode;
        private IEnumerator Start()
        {
            _asteroidLayerIndex = 1 << LayerMask.NameToLayer("Asteroid");
            _explosionMechanics = GetComponent<ExplosionMechanics>();
            
            _commonAsteroidMechanics = GetComponent<CommonAsteroidMechanics>();
            _commonAsteroidMechanics.AsteroidDroppedMoney += IncreasePlayerMoney;
            _commonAsteroidMechanics.AsteroidExploded += _explosionMechanics.EnableExplosion;
            
            _goldenAsteroidMechanics = GetComponent<GoldenAsteroidMechanics>();
            _goldenAsteroidMechanics.AsteroidDroppedMoney += IncreasePlayerMoneyGoldenMode;
            _goldenAsteroidMechanics.AsteroidExploded += _explosionMechanics.EnableExplosion;
            _goldenAsteroidMechanics.AsteroidDied += ChangeScore;
            
            _animatorMainCamera = mainCamera.GetComponent<Animator>();
            
            InitInputMechanics();
            InitSpaceships();
            InitDamageText();
            InitPlayer();

            while (!_commonAsteroidMechanics.Enable)
                yield return null;
            
            _coroutineCommonAsteroid = StartCoroutine(SpawnAsteroids());
        }
        
        private void InitSpaceships()
        {
            _spaceshipList.Add(Instantiate(spaceshipPrefab).GetComponent<Spaceship>());
            _spaceshipList[0].UpLvl();
            _spaceshipList[0].SetPosition(new Vector2(-2f, -1.1f));
        }

        private void IncreasePlayerMoney(int money)
        {
            PlayerManager.IncreaseMoney(money);
        }
        
        private void IncreasePlayerMoneyGoldenMode(int money)
        {
            money *= _goldenAsteroidMechanics.GetNumberOfDeadAsteroids();
            _goldenAsteroidMechanics.ResetNumberOfDeadAsteroids();
            PlayerManager.IncreaseMoney(money);
        }

        private void StartGoldenGameMode()
        {
            _animatorMainCamera.SetTrigger(GoldenMode);
            _goldenAsteroidMechanics.ResetNumberOfDeadAsteroids();
            _commonAsteroidMechanics.DisableAsteroids();
            StopCoroutine(_coroutineCommonAsteroid);
            _coroutineGoldenAsteroid = StartCoroutine(SpawnGoldenAsteroids());
            StartCoroutine(LifeOfGoldenMode());
        }
        
        private void SpaceshipsShoot(Vector2 posEnemy)
        {
            foreach (var spaceship in _spaceshipList)
            {
                spaceship.ShotLaser(posEnemy);
            }
        }
        
        private IEnumerator SpawnAsteroids()
        {
            while (true)
            {
                yield return new WaitForSeconds(3f);
                
                if (_goldenModeIndex == 6)
                {
                    StartGoldenGameMode();
                    _goldenModeIndex += 1;
                    break;
                }

                _goldenModeIndex += 1;
                
                _commonAsteroidMechanics.LaunchAsteroid();
            }
        }
        
        private IEnumerator SpawnGoldenAsteroids()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.7f);
                
                _goldenAsteroidMechanics.LaunchAsteroid();
            }
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
            _goldenAsteroidMechanics.DisableAsteroids();
            StopCoroutine(_coroutineGoldenAsteroid);
            _animatorMainCamera.SetBool(IsGoldenMode, false);
            _coroutineCommonAsteroid = StartCoroutine(SpawnAsteroids());
        }
        
        private void InitInputMechanics()
        {
            _inputMechanics = GetComponent<InputMechanics>();
            _inputMechanics.OnTouch += CheckTouchPosition;
            _inputMechanics.OnClick += CheckClickPosition;
        }
        
        private void InitDamageText()
        {
            for (int i = 0; i < _damageTextArray.Length; i++)
            {
                _damageTextArray[i] =
                    Instantiate(damageTextPrefab, damageTextParent.transform).GetComponent<DamageText>();
            }
        }
        
        private void InitPlayer()
        {
            PlayerManager = Instantiate(playerManagerPrefab).GetComponent<PlayerManager>();
            PlayerManager.Init();
            PlayerManager.transform.position = new Vector2(-1.5f, -3.7f);
            PlayerManager.Died += StopGame;
            PlayerManager.SetHealth(3);
            PlayerManager.SetMoney(0);
            PlayerManager.SetAttack(1);
        }
        
        private void StopGame(PlayerManager playerManager)
        {
            StartCoroutine(ExplosionPlanet());
        }

        public void ReStart()
        {
            GameStateHelper.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        private IEnumerator ExplosionPlanet()
        {
            Instantiate(explosionPlanetPrefab).transform.position = new Vector2(-1.5f, -3.7f);
            PlayerManager.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            GameOver?.Invoke();
            GameStateHelper.Pause();
        }
        
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
                hit.collider.GetComponent<AsteroidView>().TakeDamage(PlayerManager.GetAttack());
                ShowDamageText(PlayerManager.GetAttack(), touchWorldPos);
                SpaceshipsShoot(touchWorldPos);
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
        
        private void ChangeScore()
        {
            ChangeScoreGoldenMode?.Invoke(_goldenAsteroidMechanics.GetNumberOfDeadAsteroids().ToString());
        }

        private void OnDestroy()
        {
            _commonAsteroidMechanics.AsteroidDroppedMoney -= IncreasePlayerMoney;
            _commonAsteroidMechanics.AsteroidExploded -= _explosionMechanics.EnableExplosion;
            _goldenAsteroidMechanics.AsteroidDroppedMoney -= IncreasePlayerMoneyGoldenMode;
            _goldenAsteroidMechanics.AsteroidExploded -= _explosionMechanics.EnableExplosion;
            _goldenAsteroidMechanics.AsteroidDied -= ChangeScore;
            PlayerManager.Died -= StopGame;
        }
    }
}