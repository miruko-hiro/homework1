using System;
using System.Collections;
using GameMechanics.CameraScripts;
using GameMechanics.Enemy;
using GameMechanics.Enemy.Asteroid;
using GameMechanics.Enemy.DamageDisplay;
using GameMechanics.Enemy.ExplosionOfAsteroid;
using GameMechanics.Helpers;
using GameMechanics.Player.Planet;
using GameMechanics.Player.Weapon;
using GameMechanics.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
    
namespace GameMechanics
{
    [RequireComponent(typeof(InputMechanics))]
    public class MainManager : MonoBehaviour
    {
        private InputMechanics _inputMechanics;
        private PlayerManager _playerManager;
        private SpaceshipManager _spaceshipManager;
        private GoldenAsteroidManager _goldenAsteroidManager;
        private GameStateHelper _gameStateHelper;
        private MusicClaspRepository _musicClaspRepository;
        [SerializeField] private CommonAsteroidManager commonAsteroidManager;
        [SerializeField] private DamageTextManager damageTextManager;
        [SerializeField] private ExplosionManager explosionManager;
        [SerializeField] private CameraManager cameraManager;
        [Space(10)] 
        [SerializeField] private AudioClip gameMusic;
        
        private int _asteroidLayerIndex;
        private int _goldenModeIndex = 0;
        private bool _isInitAsteroids;
        
        private Coroutine _coroutineCommonAsteroid;
        private Coroutine _coroutineGoldenAsteroid;
        
        public event Action<string, bool> ChangeTimeGoldenMode;
        public event Action MainMechanicsCreate;

        private void Start()
        {
            _asteroidLayerIndex = 1 << LayerMask.NameToLayer("Asteroid");
            cameraManager.Init();
            
            InitCommonAsteroidMechanics();
            InitGoldenAsteroidMechanics();
            InitInputMechanics();
            
            _spaceshipManager.Init();
            damageTextManager.Init();
            _playerManager.Init();
            explosionManager.Init();
            
            MainMechanicsCreate?.Invoke();
        }

        [Inject]
        private void Construct(PlayerManager playerManager, 
            SpaceshipManager spaceshipManager, 
            GoldenAsteroidManager goldenAsteroidManager,
            GameStateHelper gameStateHelper,
            MusicClaspRepository musicClaspRepository)
        {
            _playerManager = playerManager;
            _spaceshipManager = spaceshipManager;
            _goldenAsteroidManager = goldenAsteroidManager;
            _gameStateHelper = gameStateHelper;
            _musicClaspRepository = musicClaspRepository;
        }

        private void InitSpawnAsteroids()
        {
            _coroutineCommonAsteroid = StartCoroutine(SpawnAsteroids());
        }
        
        private void InitMusic()
        {
            _musicClaspRepository.AddMusic(gameMusic);
        }
        
        private void InitCommonAsteroidMechanics()
        {
            commonAsteroidManager.Init();
            commonAsteroidManager.AsteroidDroppedMoney += IncreasePlayerMoney;
            commonAsteroidManager.AsteroidExploded += explosionManager.EnableExplosion;
            commonAsteroidManager.AsteroidExploded += cameraManager.CameraShakeDueToAsteroidExplosion;
        }

        private void InitGoldenAsteroidMechanics()
        {
            _goldenAsteroidManager.Init();
            _goldenAsteroidManager.AsteroidDroppedMoney += IncreasePlayerMoneyGoldenMode;
            _goldenAsteroidManager.AsteroidExploded += explosionManager.EnableExplosion;
            _goldenAsteroidManager.AsteroidExploded += cameraManager.CameraShakeDueToGoldenAsteroidExplosion;
        }

        private void IncreasePlayerMoney(int money)
        {
            _playerManager.Model.Money.Increase(money);
        }
        
        private void IncreasePlayerMoneyGoldenMode(int money)
        {
            money *= _goldenAsteroidManager.GetNumberOfDeadAsteroids();
            _goldenAsteroidManager.ResetNumberOfDeadAsteroids();
            _playerManager.Model.Money.Increase(money);
        }

        private void StartGoldenGameMode()
        {
            cameraManager.CameraToGoldenModePosition();
            _goldenAsteroidManager.ResetNumberOfDeadAsteroids();
            commonAsteroidManager.DisableAsteroids();
            StopCoroutine(_coroutineCommonAsteroid);
            _coroutineGoldenAsteroid = StartCoroutine(SpawnGoldenAsteroids());
            StartCoroutine(LifeOfGoldenMode());
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
                
                commonAsteroidManager.LaunchAsteroid();
            }
        }
        
        private IEnumerator SpawnGoldenAsteroids()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.7f);
                
                _goldenAsteroidManager.LaunchAsteroid();
            }
        }
        
        private IEnumerator LifeOfGoldenMode()
        {
            int time = 10;
            while (time > 0)
            {
                ChangeTimeGoldenMode?.Invoke(time.ToString(), true);
                time -= 1;
                yield return new WaitForSeconds(1f);
            }
            ChangeTimeGoldenMode?.Invoke(time.ToString(), false);
            _goldenAsteroidManager.DisableAsteroids();
            StopCoroutine(_coroutineGoldenAsteroid);
            cameraManager.CameraToDefaultPosition();
            _coroutineCommonAsteroid = StartCoroutine(SpawnAsteroids());
        }
        
        private void InitInputMechanics()
        {
            _inputMechanics = GetComponent<InputMechanics>();
            _inputMechanics.OnTouch += CheckTouchPosition;
            _inputMechanics.OnClick += CheckClickPosition;
        }

        public void GameReStart()
        {
            _gameStateHelper.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void GameStart()
        {
            if (!_isInitAsteroids)
            {
                _isInitAsteroids = true;
                cameraManager.CameraToDefaultPosition();
                InitSpawnAsteroids();
            }
            _gameStateHelper.Play();
            InitMusic();
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
            Vector3 touchWorldPos = cameraManager.GetValueToWorldPoint(pos);
            RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector2.zero, Mathf.Infinity, _asteroidLayerIndex);

            if (hit && hit.collider)
            {
                hit.collider.GetComponent<AsteroidView>().TakeDamage(_playerManager.Model.LaserAttack.Amount);
                damageTextManager.ShowDamageText(_playerManager.Model.LaserAttack.Amount, touchWorldPos);
                _spaceshipManager.SpaceshipsShoot(touchWorldPos);
            }
        }

        private void OnDestroy()
        {
            commonAsteroidManager.AsteroidDroppedMoney -= IncreasePlayerMoney;
            commonAsteroidManager.AsteroidExploded -= explosionManager.EnableExplosion;
            commonAsteroidManager.AsteroidExploded -= cameraManager.CameraShakeDueToAsteroidExplosion;
            _goldenAsteroidManager.AsteroidDroppedMoney -= IncreasePlayerMoneyGoldenMode;
            _goldenAsteroidManager.AsteroidExploded -= explosionManager.EnableExplosion;
            _goldenAsteroidManager.AsteroidExploded -= cameraManager.CameraShakeDueToGoldenAsteroidExplosion;
        }
    }
}