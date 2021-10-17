using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        [SerializeField] private GameObject explosionPrefab;
        private Explosion[] _explosionArray = new Explosion[3];

        [SerializeField] private GameObject damageTextPrefab;
        [SerializeField] private GameObject damageTextParent;
        private DamageText[] _damageTextArray = new DamageText[15];

        [SerializeField] private GameObject spaceshipPrefab;
        private List<Spaceship> _spaceshipList = new List<Spaceship>();

        private InputMechanics _inputMechanics;
        
        private int _asteroidIndex = 0;
        private int _explosionIndex = 0;
        private int _damageTextIndex = 0;

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
                _damageTextArray[i] = Instantiate(damageTextPrefab, damageTextParent.transform).GetComponent<DamageText>();
            }
            
            for (int i = 0; i < _asteroidArray.Length; i++)
            {
                _asteroidArray[i] = Instantiate(asteroidPrefab, GetComponent<Transform>()).GetComponent<Asteroid>();
                _asteroidArray[i].SetPosition(new Vector2(-10f, 0f));
                _asteroidArray[i].DeathAsteroid += IncreaseAsteroidIndex;

                while (!_asteroidArray[i].Enable)
                    yield return null;
                
                _asteroidArray[i].gameObject.SetActive(false);
            }

            GameObject planet = Instantiate(planetPrefab);
            planet.GetComponent<Transform>().position = new Vector2(-1.5f, -3.7f);
            _player = planet.GetComponent<Player>();

            for (int i = 0; i < _explosionArray.Length; i++)
            {
                _explosionArray[i] = Instantiate(explosionPrefab).GetComponent<Explosion>();
                _explosionArray[i].gameObject.SetActive(false);
            }

            _asteroidArray[_asteroidIndex].gameObject.SetActive(true);
            SetInitialStateOfAsteroid(_asteroidArray[_asteroidIndex]);
        }

        private void SetInitialStateOfAsteroid(Asteroid asteroid)
        {
            asteroid.SetPosition(new Vector2(Random.Range(-1.5f, 1.6f), Random.Range(1.5f, 4f)));
            asteroid.SetLocalScale(new Vector2(0.1f, 0.1f));
            asteroid.SetHealth(10);
            asteroid.SetDamage(1);
            asteroid.Move(new Vector2(-0.5f, -3f), 0.2f);
            asteroid.SetScale(0.005f, 0.005f, 1.7f, 1.7f);
        }

        private void IncreaseAsteroidIndex()
        {
            _explosionArray[_explosionIndex].gameObject.SetActive(true);
            _explosionArray[_explosionIndex].SetPosition(_asteroidArray[_asteroidIndex].transform.position);
            StartCoroutine(WaitForEndOfExplosion(_explosionIndex));
            _asteroidArray[_asteroidIndex].gameObject.SetActive(false);
            
            if (_asteroidIndex < 2)
            {
                _asteroidIndex += 1;
                _explosionIndex += 1;
            }
            else
            {
                _asteroidIndex = 0;
                _explosionIndex = 0;
            }
            
            _asteroidArray[_asteroidIndex].gameObject.SetActive(true);
            SetInitialStateOfAsteroid(_asteroidArray[_asteroidIndex]);
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
            Vector3 touchWorldPos =  mainCamera.ScreenToWorldPoint(pos);
            RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector2.zero);
            
            if (hit && hit.collider.CompareTag(Asteroid.Tag))
            {
                _asteroidArray[_asteroidIndex].TakeDamage(_player.GetAmountOfDamage());
                ShowDamageText(_player.GetAmountOfDamage(), touchWorldPos);
                
                foreach (var spaceship in _spaceshipList)
                {
                    spaceship.ShotLaser(touchWorldPos);
                }
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

        private void OnDestroy()
        {
            foreach (Asteroid asteroid in _asteroidArray)
            {
                asteroid.DeathAsteroid -= IncreaseAsteroidIndex;
            }
            
            _inputMechanics.OnTouch -= CheckTouchPosition;
            _inputMechanics.OnClick -= CheckClickPosition;
        }
    }
}
