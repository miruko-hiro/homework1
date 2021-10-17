using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    [RequireComponent(typeof(InputMechanics))]
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

        private int _asteroidIndex = 0;
        private int _damageTextIndex = 0;
        private int _numberOfLivingAsteroids = 0;
        private int _numberSpaceships = 1;
        private int _lvlAsteroids = 1;
        private int _healthAsteroid = 10;

        public const string Asteroid01Tag = "Asteroid0";
        public const string Asteroid02Tag = "Asteroid1";
        public const string Asteroid03Tag = "Asteroid2";

        public event Action GameOver;

        private IEnumerator Start()
        {
            _inputMechanics = GetComponent<InputMechanics>();
            _inputMechanics.OnTouch += CheckTouchPosition;
            _inputMechanics.OnClick += CheckClickPosition;

            _spaceshipList.Add(Instantiate(spaceshipPrefab).GetComponent<Spaceship>());
            _spaceshipList[0].UpLvl();
            _spaceshipList[0].SetPosition(new Vector2(-2f, -1.1f));

            for (int i = 0; i < _damageTextArray.Length; i++)
            {
                _damageTextArray[i] =
                    Instantiate(damageTextPrefab, damageTextParent.transform).GetComponent<DamageText>();
            }

            for (int i = 0; i < _asteroidArray.Length; i++)
            {
                _asteroidArray[i] = Instantiate(asteroidPrefab, GetComponent<Transform>()).GetComponent<Asteroid>();
                _asteroidArray[i].SetPosition(new Vector2(-10f, 0f));
                _asteroidArray[i].tag = "Asteroid" + i;
                _asteroidArray[i].Count = i;
                _asteroidArray[i].Died += DeadAsteroid;

                while (!_asteroidArray[i].Enable)
                    yield return null;

                _asteroidArray[i].gameObject.SetActive(false);
            }

            _planet = Instantiate(planetPrefab);
            _planet.GetComponent<Transform>().position = new Vector2(-1.5f, -3.7f);
            _player = _planet.GetComponent<Player>();
            Player = _player;
            _player.Died += StopGame;

            for (int i = 0; i < _explosionArray.Length; i++)
            {
                _explosionArray[i] = Instantiate(explosionPrefab).GetComponent<Explosion>();
                _explosionArray[i].gameObject.SetActive(false);
            }

            _asteroidArray[_asteroidIndex].gameObject.SetActive(true);

            InvokeRepeating(nameof(IncreaseAsteroidIndex), 1.5f, 3f);
        }

        private void SetInitialStateOfAsteroid(Asteroid asteroid)
        {
            asteroid.SetPosition(new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(3.5f, 4.5f)));
            asteroid.SetLocalScale(new Vector2(0.1f, 0.1f));

            if (_lvlAsteroids % 7 == 0) _healthAsteroid += 10;
            asteroid.Health.SetAmount(_healthAsteroid);
            asteroid.Attack.Amount = 1;
            asteroid.Movement.Move(new Vector2(-0.5f, -3f), 0.3f);

            asteroid.Scale.SetScale(0.005f, 0.005f);
            asteroid.Scale.SetMaxScale(1.7f, 1.7f);
            asteroid.Scale.ActiveScale(true);
        }

        private void DeadAsteroid()
        {
            int i = 1;
            if (_lvlAsteroids > 70) i = 7;
            if (_lvlAsteroids > 60) i = 6;
            if (_lvlAsteroids > 50) i = 5;
            if (_lvlAsteroids > 30) i = 4;
            if (_lvlAsteroids > 20) i = 3;
            if (_lvlAsteroids > 10) i = 2;
            _player.Money.Amount += i;

            foreach (Asteroid asteroid in _asteroidArray)
            {
                if (asteroid.Health.Amount == 0 || asteroid.IsCollision)
                {
                    asteroid.Health.SetAmount(_healthAsteroid);
                    asteroid.gameObject.SetActive(false);

                    _explosionArray[asteroid.Count].gameObject.SetActive(true);
                    _explosionArray[asteroid.Count].SetPosition(asteroid.transform.position);
                    _numberOfLivingAsteroids -= 1;

                    StartCoroutine(WaitForEndOfExplosion(asteroid.Count));
                }
            }
        }

        private void IncreaseAsteroidIndex()
        {
            if (_numberOfLivingAsteroids == 3) return;
            _lvlAsteroids += 1;

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
                switch (hit.collider.tag)
                {
                    case Asteroid01Tag:
                        _asteroidArray[0].TakeDamage(_player.Attack.Amount);
                        break;
                    case Asteroid02Tag:
                        _asteroidArray[1].TakeDamage(_player.Attack.Amount);
                        break;
                    case Asteroid03Tag:
                        _asteroidArray[2].TakeDamage(_player.Attack.Amount);
                        break;
                }

                ShowDamageText(_player.Attack.Amount, touchWorldPos);

                foreach (var spaceship in _spaceshipList)
                {
                    spaceship.ShotLaser(touchWorldPos);
                }
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
            switch (_numberSpaceships)
            {
                case 1:
                    _spaceshipList.Add(Instantiate(spaceshipPrefab).GetComponent<Spaceship>());
                    _spaceshipList[_numberSpaceships].UpLvl();
                    _spaceshipList[_numberSpaceships].SetPosition(new Vector2(1f, -2.5f));
                    _numberSpaceships += 1;
                    break;
                case 2:
                    _spaceshipList.Add(Instantiate(spaceshipPrefab).GetComponent<Spaceship>());
                    _spaceshipList[_numberSpaceships].UpLvl();
                    _spaceshipList[_numberSpaceships].SetPosition(new Vector2(-0.3f, -1.3f));
                    _numberSpaceships += 1;
                    break;
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
