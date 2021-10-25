using System;
using System.Collections;
using GameMechanics.AsteroidMechanics.CommonAsteroid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics.AsteroidMechanics
{
    [RequireComponent(typeof(LvlUpAsteroidMechanics))]
    public class CommonAsteroidMechanics : MonoBehaviour, IAsteroidMechanics
    {
        [SerializeField] private GameObject asteroidManagerPrefab;
        private AsteroidManager[] _asteroidManagers = new AsteroidManager[3];
        
        private LvlUpAsteroidMechanics _lvlUpAsteroidMechanics;
        
        private int _asteroidIndex = 0;
        private int _numberOfLivingAsteroids = 0;
        
        public event Action<int> AsteroidDroppedMoney;
        public event Action<Vector2> AsteroidExploded;
        
        public bool Enable { get; set; } = false;

        private void Start()
        {
            _lvlUpAsteroidMechanics = GetComponent<LvlUpAsteroidMechanics>();
            StartCoroutine(InitAsteroids());
        }
        
        private IEnumerator InitAsteroids()
        {
            for (int i = 0; i < _asteroidManagers.Length; i++)
            {
                _asteroidManagers[i] = Instantiate(asteroidManagerPrefab, transform).GetComponent<AsteroidManager>();
                _asteroidManagers[i].transform.position = new Vector2(-10f, 0f);
                while (!_asteroidManagers[i].Enable)
                    yield return null;
                _asteroidManagers[i].Died += DeadAsteroid;
                _asteroidManagers[i].transform.position = new Vector2(0f, 0f);
                _asteroidManagers[i].gameObject.SetActive(false);
            }

            Enable = true;
        }
        public void LaunchAsteroid()
        {
            if (_numberOfLivingAsteroids == 3) return;
                
            _lvlUpAsteroidMechanics.LvlAsteroids += 1;

            _asteroidManagers[_asteroidIndex].gameObject.SetActive(true);
            SetInitialStateOfAsteroid(_asteroidManagers[_asteroidIndex]);
            _numberOfLivingAsteroids += 1;

            _asteroidIndex = _asteroidIndex < _asteroidManagers.Length - 1 ? _asteroidIndex += 1 : 0;
        }
        
        private void SetInitialStateOfAsteroid(AsteroidManager asteroidManager)
        {
            asteroidManager.SetPosition(new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(3.5f, 4.5f)));
            asteroidManager.SetLocalScale(new Vector2(0.1f, 0.1f));
            
            _lvlUpAsteroidMechanics.HealthUp();
            asteroidManager.SetHealth(_lvlUpAsteroidMechanics.HealthAsteroid);
            asteroidManager.SetAttack(1);
            asteroidManager.SetMotionParameters(new Vector2(-0.5f, -3f), 0.3f);

            asteroidManager.SetScale(new Vector2(0.005f, 0.005f));
            asteroidManager.SetMaxScale(new Vector2(1.7f, 1.7f));
            asteroidManager.ActiveScale();
        }

        public void DisableAsteroids()
        {
            foreach (AsteroidManager asteroidManager in _asteroidManagers)
            {
                asteroidManager.gameObject.SetActive(false);
            }
        }
            
        private void DeadAsteroid(AsteroidManager asteroidManager)
        {
            _lvlUpAsteroidMechanics.MoneyUp();
            AsteroidDroppedMoney?.Invoke(_lvlUpAsteroidMechanics.MoneyAsteroid);
            ShowDeadAsteroid(asteroidManager);
        }

        private void ShowDeadAsteroid(AsteroidManager asteroidManager)
        {
            asteroidManager.SetHealth(_lvlUpAsteroidMechanics.HealthAsteroid);
            AsteroidExploded?.Invoke(asteroidManager.GetPosition());
            asteroidManager.gameObject.SetActive(false);
            
            _numberOfLivingAsteroids -= 1;
        }

        private void OnDestroy()
        {
            foreach (AsteroidManager asteroidManager in _asteroidManagers)
            {
                asteroidManager.Died -= DeadAsteroid;
            }
        }
    }
}