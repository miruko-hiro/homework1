using System;
using System.Collections;
using GameMechanics.Enemy;
using GameMechanics.Enemy.Asteroid;
using GameMechanics.Enemy.DamageDisplay;
using GameMechanics.Enemy.ExplosionOfAsteroid;
using GameMechanics.Helpers;
using GameMechanics.Player;
using GameMechanics.Player.Planet;
using GameMechanics.Player.Weapon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameMechanics
{
    [RequireComponent(typeof(InputMechanics),
        typeof(PlayerMechanics),
        typeof(DamageTextMechanics))]
    [RequireComponent(typeof(CommonAsteroidMechanics),
        typeof(GoldenAsteroidMechanics),
        typeof(ExplosionMechanics))]
    [RequireComponent(typeof(SpaceshipMechanics))]
    public class MainMechanics : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        private Animator _animatorMainCamera;
        public PlayerManager PlayerManager { get; private set; }
        
        private int _asteroidLayerIndex;
        private int _goldenModeIndex = 0;
        
        private InputMechanics _inputMechanics;
        public PlayerMechanics PlayerMechanics { get; private set; }
        private DamageTextMechanics _damageTextMechanics;
        public SpaceshipMechanics SpaceshipMechanics { get; private set; }
        private CommonAsteroidMechanics _commonAsteroidMechanics;
        private GoldenAsteroidMechanics _goldenAsteroidMechanics;
        private ExplosionMechanics _explosionMechanics;
        
        private Coroutine _coroutineCommonAsteroid;
        private Coroutine _coroutineGoldenAsteroid;
        private static readonly int GoldenMode = Animator.StringToHash("GoldenMode");
        private static readonly int IsGoldenMode = Animator.StringToHash("isGoldenMode");
        
        public event Action<string> ChangeScoreGoldenMode;
        public event Action<string, bool> ChangeTimeGoldenMode;

        private void Start()
        {
            _asteroidLayerIndex = 1 << LayerMask.NameToLayer("Asteroid");
            _explosionMechanics = GetComponent<ExplosionMechanics>();

            InitCommonAsteroidMechanics();
            InitGoldenAsteroidMechanics();

            _animatorMainCamera = mainCamera.GetComponent<Animator>();
            
            InitInputMechanics();

            SpaceshipMechanics = GetComponent<SpaceshipMechanics>();
            SpaceshipMechanics.Init();

            _damageTextMechanics = GetComponent<DamageTextMechanics>();
            _damageTextMechanics.Init();
            
            PlayerMechanics = GetComponent<PlayerMechanics>();
            PlayerManager = PlayerMechanics.InitPlayer();
            
            _coroutineCommonAsteroid = StartCoroutine(SpawnAsteroids());
        }

        private void InitCommonAsteroidMechanics()
        {
            _commonAsteroidMechanics = GetComponent<CommonAsteroidMechanics>();
            _commonAsteroidMechanics.Init();
            _commonAsteroidMechanics.AsteroidDroppedMoney += IncreasePlayerMoney;
            _commonAsteroidMechanics.AsteroidExploded += _explosionMechanics.EnableExplosion;
        }

        private void InitGoldenAsteroidMechanics()
        {
            _goldenAsteroidMechanics = GetComponent<GoldenAsteroidMechanics>();
            _goldenAsteroidMechanics.Init();
            _goldenAsteroidMechanics.AsteroidDroppedMoney += IncreasePlayerMoneyGoldenMode;
            _goldenAsteroidMechanics.AsteroidExploded += _explosionMechanics.EnableExplosion;
            _goldenAsteroidMechanics.AsteroidDied += ChangeScore;
        }

        private void IncreasePlayerMoney(int money)
        {
            PlayerManager.Model.Money.Increase(money);
        }
        
        private void IncreasePlayerMoneyGoldenMode(int money)
        {
            money *= _goldenAsteroidMechanics.GetNumberOfDeadAsteroids();
            _goldenAsteroidMechanics.ResetNumberOfDeadAsteroids();
            PlayerManager.Model.Money.Increase(money);
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

        public void ReStart()
        {
            GameStateHelper.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
                hit.collider.GetComponent<AsteroidView>().TakeDamage(PlayerManager.Model.Attack.Amount);
                _damageTextMechanics.ShowDamageText(PlayerManager.Model.Attack.Amount, touchWorldPos);
                SpaceshipMechanics.SpaceshipsShoot(touchWorldPos);
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
        }
    }
}