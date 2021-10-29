using System;
using System.Collections;
using GameMechanics.Enemy.Asteroid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics.Enemy
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

        public void Init()
        {
            _lvlUpAsteroidMechanics = GetComponent<LvlUpAsteroidMechanics>();
            InitGoldenAsteroids();
        }

        private void InitGoldenAsteroids()
        {
            for (int i = 0; i < _asteroidManagers.Length; i++)
            {
                _asteroidManagers[i] = Instantiate(goldenAsteroidManagerPrefab, transform).GetComponent<AsteroidManager>();
                _asteroidManagers[i].Init();
                _asteroidManagers[i].transform.position = new Vector2(-10f, 0f);
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
            asteroidManager.Model.Position = new Vector2(Random.Range(-0.5f, 1.5f), Random.Range(2.5f, 4f));
            asteroidManager.Model.LocalScale = new Vector2(0.1f, 0.1f);

            asteroidManager.Model.Health.SetAmount(1);
            asteroidManager.Model.Attack.SetAmount(0);
            asteroidManager.Model.Movement.Run(new Vector2(-0.5f, -3f), 0.4f, asteroidManager.View.transform);

            asteroidManager.Model.Scale.Scale = new Vector2(0.01f, 0.01f);
            asteroidManager.Model.Scale.MaxScale = new Vector2(1.4f, 1.4f);
            asteroidManager.Model.Scale.Run(asteroidManager.View.transform);
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
            AsteroidExploded?.Invoke(asteroidManager.View.transform.position);
            asteroidManager.gameObject.SetActive(false);
            
            _numberOfLivingAsteroids -= 1;
        }
        
        public void DisableAsteroids()
        {
            AsteroidDroppedMoney?.Invoke(_lvlUpAsteroidMechanics.MoneyAsteroid);
            
            foreach (AsteroidManager asteroidManager in _asteroidManagers)
            {
                AsteroidExploded?.Invoke(asteroidManager.View.transform.position);
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