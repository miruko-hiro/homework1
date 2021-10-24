using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics.AsteroidMechanics
{
    public class GoldenAsteroidMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject goldenAsteroidPrefab;
        private Asteroid[] _goldenAsteroidArray = new Asteroid[5];
        
        private LvlUpAsteroidMechanics _lvlUpAsteroidMechanics;
        
        private int _goldenAsteroidIndex = 0;
        private int _numberOfLivingGoldenAsteroids = 0;
        
        public event Action<int> AsteroidDroppedMoney;
        public event Action<Vector2> AsteroidExploded;
        public event Action GoldenAsteroidDied;
        private void Start()
        {
            _lvlUpAsteroidMechanics = GetComponent<LvlUpAsteroidMechanics>();
            StartCoroutine(InitGoldenAsteroids());
        }

        private IEnumerator InitGoldenAsteroids()
        {
            for (int i = 0; i < _goldenAsteroidArray.Length; i++)
            {
                InitAsteroid(i, out _goldenAsteroidArray[i]);
                while (!_goldenAsteroidArray[i].Enable)
                    yield return null;
                _goldenAsteroidArray[i].gameObject.SetActive(false);
            }
        }

        private void InitAsteroid(int i, out Asteroid asteroid)
        {
            asteroid = Instantiate(goldenAsteroidPrefab, transform).GetComponent<Asteroid>();
            asteroid.SetPosition(new Vector2(-10f, 0f));
            asteroid.Count = i;
            asteroid.Died += DeadAsteroid;
            asteroid.ReachedLineOfDestroy += ReachedLineOfDestroy;
        }

        private void ReachedLineOfDestroy(Asteroid asteroid)
        {
            _numberOfLivingGoldenAsteroids -= 1;
        }

        public void DisableGoldenAsteroids()
        {
            AsteroidDroppedMoney?.Invoke(_lvlUpAsteroidMechanics.MoneyAsteroid);
            
            foreach (Asteroid asteroid in _goldenAsteroidArray)
            {
                AsteroidExploded?.Invoke(asteroid.transform.position);
                asteroid.gameObject.SetActive(false);
            }

            _numberOfLivingGoldenAsteroids = 0;
        }

        public IEnumerator IncreaseGoldAsteroidIndex()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.7f);
                
                if (_numberOfLivingGoldenAsteroids == 5) continue;

                _goldenAsteroidArray[_goldenAsteroidIndex].gameObject.SetActive(true);
                SetInitialStateOfGoldAsteroid(_goldenAsteroidArray[_goldenAsteroidIndex]);
                _numberOfLivingGoldenAsteroids += 1;

                _goldenAsteroidIndex = _goldenAsteroidIndex < 4 ? _goldenAsteroidIndex += 1 : 0;
            }
        }

        private void SetInitialStateOfGoldAsteroid(Asteroid asteroid)
        {
            asteroid.SetPosition(new Vector2(Random.Range(-1f, 1.5f), Random.Range(3.5f, 4f)));
            asteroid.SetLocalScale(new Vector2(0.1f, 0.1f));

            asteroid.Health.SetAmount(1);
            asteroid.Attack.Amount = 0;
            asteroid.Movement.Move(new Vector2(-0.5f, -3f), 0.4f);

            asteroid.Scale.SetScale(0.01f, 0.01f);
            asteroid.Scale.SetMaxScale(1.4f, 1.4f);
            asteroid.Scale.ActiveScale(true);
        }
        
        
        private void DeadAsteroid(Asteroid asteroid)
        {
            GoldenAsteroidDied?.Invoke();
            
            ShowDeadAsteroid(asteroid);
        }

        private void ShowDeadAsteroid(Asteroid asteroid)
        {
            AsteroidExploded?.Invoke(asteroid.transform.position);
            asteroid.gameObject.SetActive(false);
            
            _numberOfLivingGoldenAsteroids -= 1;
        }

        private void OnDestroy()
        {
            foreach (Asteroid asteroid in _goldenAsteroidArray)
            {
                asteroid.Died -= DeadAsteroid;
                asteroid.ReachedLineOfDestroy -= ReachedLineOfDestroy;
            }
        }
    }
}