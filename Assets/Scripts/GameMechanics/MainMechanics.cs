using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameMechanics.AsteroidMechanics;
using GameMechanics.PlayerMechanics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    [RequireComponent(
        typeof(InputMechanics),
        typeof(LvlUpAsteroidMechanics)
        )]
    public class MainMechanics : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        [SerializeField] private GameObject asteroidPrefab;
        private Asteroid[] _asteroidArray = new Asteroid[3];

        [SerializeField] private GameObject planetPrefab;
        private Player _player;
        public Player Player { get; private set; }

        [SerializeField] private GameObject explosionPlanetPrefab;
        private GameObject _planet;

        [SerializeField] private GameObject explosionPrefab;
        private Explosion[] _explosionArray = new Explosion[3];

        [SerializeField] private GameObject damageTextPrefab;
        [SerializeField] private GameObject damageTextParent;
        private DamageText[] _damageTextArray = new DamageText[15];

        [SerializeField] private GameObject spaceshipPrefab;
        private List<Spaceship> _spaceshipList = new List<Spaceship>();

        private InputMechanics _inputMechanics;
        private LvlUpAsteroidMechanics _lvlUpAsteroidMechanics;

        private int _asteroidIndex = 0;
        private int _damageTextIndex = 0;
        private int _numberOfLivingAsteroids = 0;
        private int _numberSpaceships = 1;

        // public const string Asteroid01Tag = "Asteroid0";
        // public const string Asteroid02Tag = "Asteroid1";
        // public const string Asteroid03Tag = "Asteroid2";

        public event Action GameOver;

        //Start Of Initialization
        private IEnumerator Start()
        {
            InitInputMechanics();
            _lvlUpAsteroidMechanics = GetComponent<LvlUpAsteroidMechanics>();
            InitSpaceships();
            InitDamageText();
            
            for (int i = 0; i < _asteroidArray.Length; i++)
            {
                InitAsteroid(i);
                while (!_asteroidArray[i].Enable)
                    yield return null;
                _asteroidArray[i].gameObject.SetActive(false);
            }
            
            InitPlanet();
            InitExplosions();
            _asteroidArray[_asteroidIndex].gameObject.SetActive(true);
            InvokeRepeating(nameof(IncreaseAsteroidIndex), 1.5f, 3f);
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

        private void InitAsteroid(int i)
        {
            _asteroidArray[i] = Instantiate(asteroidPrefab, GetComponent<Transform>()).GetComponent<Asteroid>();
            _asteroidArray[i].SetPosition(new Vector2(-10f, 0f));
            _asteroidArray[i].Count = i;
            _asteroidArray[i].Died += DeadAsteroid;
        }

        private void InitPlanet()
        {
            _planet = Instantiate(planetPrefab);
            _planet.GetComponent<Transform>().position = new Vector2(-1.5f, -3.7f);
            _player = _planet.GetComponent<Player>();
            Player = _player;
            _player.Died += StopGame;
        }

        private void InitExplosions()
        {
            for (int i = 0; i < _explosionArray.Length; i++)
            {
                _explosionArray[i] = Instantiate(explosionPrefab).GetComponent<Explosion>();
                _explosionArray[i].gameObject.SetActive(false);
            }
        }

        private void SetInitialStateOfAsteroid(Asteroid asteroid)
        {
            asteroid.SetPosition(new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(3.5f, 4.5f)));
            asteroid.SetLocalScale(new Vector2(0.1f, 0.1f));

            _lvlUpAsteroidMechanics.HealthUp();
            asteroid.Health.SetAmount(_lvlUpAsteroidMechanics.HealthAsteroid);
            asteroid.Attack.Amount = 1;
            asteroid.Movement.Move(new Vector2(-0.5f, -3f), 0.3f);

            asteroid.Scale.SetScale(0.005f, 0.005f);
            asteroid.Scale.SetMaxScale(1.7f, 1.7f);
            asteroid.Scale.ActiveScale(true);
        }
        //End Of Initialization
        

        //Start Of Asteroid Control
        private void DeadAsteroid()
        {
            _lvlUpAsteroidMechanics.MoneyUp();
            _player.Money.Amount += _lvlUpAsteroidMechanics.MoneyAsteroid;

            foreach (Asteroid asteroid in _asteroidArray)
            {
                if (asteroid.Health.Amount == 0 || asteroid.IsCollision)
                {
                    ShowDeadAsteroid(asteroid);
                }
            }
        }

        private void ShowDeadAsteroid(Asteroid asteroid)
        {
            asteroid.Health.SetAmount(_lvlUpAsteroidMechanics.HealthAsteroid);
            asteroid.gameObject.SetActive(false);

            _explosionArray[asteroid.Count].gameObject.SetActive(true);
            _explosionArray[asteroid.Count].SetPosition(asteroid.transform.position);
            _numberOfLivingAsteroids -= 1;

            StartCoroutine(WaitForEndOfExplosion(asteroid.Count));
        }

        private void IncreaseAsteroidIndex()
        {
            if (_numberOfLivingAsteroids == 3) return;
            _lvlUpAsteroidMechanics.LvlAsteroids += 1;

            _asteroidArray[_asteroidIndex].gameObject.SetActive(true);
            SetInitialStateOfAsteroid(_asteroidArray[_asteroidIndex]);
            _numberOfLivingAsteroids += 1;

            _asteroidIndex = _asteroidIndex < 2 ? _asteroidIndex += 1 : 0;
        }

        private IEnumerator WaitForEndOfExplosion(int index)
        {
            yield return new WaitForSeconds(2f);
            _explosionArray[index].gameObject.SetActive(false);
        }
        //End Of Asteroid Control

        
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
            RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector2.zero);

            if (hit && hit.collider)
            {
                foreach (Asteroid asteroid in _asteroidArray)
                {
                    if (!Equals(hit.collider.gameObject, asteroid.gameObject)) continue;
                    asteroid.TakeDamage(_player.Attack.Amount);
                    ShowDamageText(_player.Attack.Amount, touchWorldPos);
                    SpaceshipsShoot(touchWorldPos);
                    break;
                }
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
            Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Pause()
        {
            Time.timeScale = 0;
        }

        public void Play()
        {
            Time.timeScale = 1;
        }
        //End Of Game State Control

        private IEnumerator ExplosionPlanet()
        {
            Instantiate(explosionPlanetPrefab).transform.position = new Vector2(-1.5f, -3.7f);
            _planet.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            GameOver?.Invoke();
            Pause();
        }

        private void OnDestroy()
        {
            foreach (Asteroid asteroid in _asteroidArray)
            {
                asteroid.Died -= DeadAsteroid;
            }
            _player.Died -= StopGame;
            _inputMechanics.OnTouch -= CheckTouchPosition;
            _inputMechanics.OnClick -= CheckClickPosition;
        }
    }
}
