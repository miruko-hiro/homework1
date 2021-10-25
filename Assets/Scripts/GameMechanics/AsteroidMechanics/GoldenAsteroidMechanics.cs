using System;
using System.Collections;
using GameMechanics.AsteroidMechanics.CommonAsteroid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics.AsteroidMechanics
{
    [RequireComponent(typeof(LvlUpAsteroidMechanics))]
    public class GoldenAsteroidMechanics : MonoBehaviour, IAsteroidMechanics
    {
        [SerializeField] private GameObject goldenAsteroidManagerPrefab;
        private AsteroidManager[] _asteroidManagers = new AsteroidManager[5];

        private LvlUpAsteroidMechanics _lvlUpAsteroidMechanics;
        
        private int _asteroidIndex = 0;
        private int _numberOfLivingAsteroids = 0;
        private int _totalKilledAsteroids = 0;
        
        public event Action<int> AsteroidDroppedMoney;
        public event Action<Vector2> AsteroidExploded;
        public event Action AsteroidDied;

        private void Start()
        {
            _lvlUpAsteroidMechanics = GetComponent<LvlUpAsteroidMechanics>();
            StartCoroutine(InitGoldenAsteroids());
        }

        private IEnumerator InitGoldenAsteroids()
        {
            for (int i = 0; i < _asteroidManagers.Length; i++)
            {
                _asteroidManagers[i] = Instantiate(goldenAsteroidManagerPrefab, transform).GetComponent<AsteroidManager>();
                _asteroidManagers[i].transform.position = new Vector2(-10f, 0f);
                while (!_asteroidManagers[i].Enable)
                    yield return null;
                _asteroidManagers[i].Died += DeadAsteroid;
                _asteroidManagers[i].ReachedLineOfDestroy += ReachedLineOfDestroy;
                _asteroidManagers[i].transform.position = new Vector2(0f, 0f);
                _asteroidManagers[i].gameObject.SetActive(false);
            }
        }

        public void LaunchAsteroid()
        {
            if (_numberOfLivingAsteroids == 5) return;

            _asteroidManagers[_asteroidIndex].gameObject.SetActive(true);
            SetInitialStateOfAsteroid(_asteroidManagers[_asteroidIndex]);
            _numberOfLivingAsteroids += 1;

            _asteroidIndex = _asteroidIndex < _asteroidManagers.Length - 1 ? _asteroidIndex += 1 : 0;
        }
        
        private void SetInitialStateOfAsteroid(AsteroidManager asteroidManager)
        {
            asteroidManager.SetPosition(new Vector2(Random.Range(-1f, 1.5f), Random.Range(3.5f, 4f)));
            asteroidManager.SetLocalScale(new Vector2(0.1f, 0.1f));

            asteroidManager.SetHealth(1);
            asteroidManager.SetAttack(0);
            asteroidManager.SetMotionParameters(new Vector2(-0.5f, -3f), 0.4f);
            
            asteroidManager.SetScale(new Vector2(0.01f, 0.01f));
            asteroidManager.SetMaxScale(new Vector2(1.4f, 1.4f));
            asteroidManager.ActiveScale();
        }
        
        public int GetNumberOfDeadAsteroids()
        {
            return _totalKilledAsteroids;
        }
        
        public void ResetNumberOfDeadAsteroids()
        {
            _totalKilledAsteroids = 0;
        }
        
        private void DeadAsteroid(AsteroidManager asteroidManager)
        {
            _totalKilledAsteroids += 1;
            AsteroidDied?.Invoke();
            ShowDeadAsteroid(asteroidManager);
        }
        
        private void ShowDeadAsteroid(AsteroidManager asteroidManager)
        {
            AsteroidExploded?.Invoke(asteroidManager.GetPosition());
            asteroidManager.gameObject.SetActive(false);
            
            _numberOfLivingAsteroids -= 1;
        }
        
        public void DisableAsteroids()
        {
            AsteroidDroppedMoney?.Invoke(_lvlUpAsteroidMechanics.MoneyAsteroid);
            
            foreach (AsteroidManager asteroidManager in _asteroidManagers)
            {
                AsteroidExploded?.Invoke(asteroidManager.GetPosition());
                asteroidManager.gameObject.SetActive(false);
            }

            _numberOfLivingAsteroids = 0;
        }
        
        private void ReachedLineOfDestroy(AsteroidManager asteroidManager)
        {
            asteroidManager.gameObject.SetActive(false);
            _numberOfLivingAsteroids -= 1;
        }

        private void OnDestroy()
        {
            foreach (AsteroidManager asteroidManager in _asteroidManagers)
            {
                asteroidManager.Died -= DeadAsteroid;
                asteroidManager.ReachedLineOfDestroy -= ReachedLineOfDestroy;
            }
        }
    }
}