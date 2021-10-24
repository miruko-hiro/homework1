using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics.AsteroidMechanics
{
    [RequireComponent(typeof(LvlUpAsteroidMechanics))]
    public class CommonAsteroidMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject asteroidPrefab;
        private Asteroid[] _asteroidArray = new Asteroid[3];
        
        private LvlUpAsteroidMechanics _lvlUpAsteroidMechanics;
        
        private int _asteroidIndex = 0;
        private int _numberOfLivingAsteroids = 0;
        private bool wasAlreadyGoldenMode = false;

        public event Action<int> AsteroidDroppedMoney;
        public event Action<Vector2> AsteroidExploded;
        public event Action GoldenAsteroidFell;

        private void Start()
        {
            _lvlUpAsteroidMechanics = GetComponent<LvlUpAsteroidMechanics>();
            StartCoroutine(InitAsteroids());
        }
        
        private IEnumerator InitAsteroids()
        {
            for (int i = 0; i < _asteroidArray.Length; i++)
            {
                InitAsteroid(i, out _asteroidArray[i]);
                while (!_asteroidArray[i].Enable)
                    yield return null;
                _asteroidArray[i].gameObject.SetActive(false);
            }
        }

        private void InitAsteroid(int i, out Asteroid asteroid)
        {
            asteroid = Instantiate(asteroidPrefab, transform).GetComponent<Asteroid>();
            asteroid.SetPosition(new Vector2(-10f, 0f));
            asteroid.Count = i;
            asteroid.Died += DeadAsteroid;
        }
        
        public IEnumerator IncreaseAsteroidIndex()
        {
            while (true)
            {
                yield return new WaitForSeconds(3f);
                
                if (_numberOfLivingAsteroids == 3) continue;

                if (_lvlUpAsteroidMechanics.LvlAsteroids == 6 && !wasAlreadyGoldenMode)
                {
                    GoldenAsteroidFell!.Invoke();
                    wasAlreadyGoldenMode = true;
                    continue;
                }
                
                _lvlUpAsteroidMechanics.LvlAsteroids += 1;

                _asteroidArray[_asteroidIndex].gameObject.SetActive(true);
                SetInitialStateOfAsteroid(_asteroidArray[_asteroidIndex]);
                _numberOfLivingAsteroids += 1;

                _asteroidIndex = _asteroidIndex < 2 ? _asteroidIndex += 1 : 0;
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

        public void DisableAsteroids()
        {
            foreach (Asteroid asteroid in _asteroidArray)
            {
                asteroid.gameObject.SetActive(false);
            }
        }
        
        private void DeadAsteroid(Asteroid asteroid)
        {
            _lvlUpAsteroidMechanics.MoneyUp();
            AsteroidDroppedMoney?.Invoke(_lvlUpAsteroidMechanics.MoneyAsteroid);
            ShowDeadAsteroid(asteroid);
        }

        private void ShowDeadAsteroid(Asteroid asteroid)
        {
            asteroid.Health.SetAmount(_lvlUpAsteroidMechanics.HealthAsteroid);
            AsteroidExploded?.Invoke(asteroid.transform.position);
            asteroid.gameObject.SetActive(false);
            
            _numberOfLivingAsteroids -= 1;
        }

        private void OnDestroy()
        {
            foreach (Asteroid asteroid in _asteroidArray)
            {
                asteroid.Died -= DeadAsteroid;
            }
        }
    }
}