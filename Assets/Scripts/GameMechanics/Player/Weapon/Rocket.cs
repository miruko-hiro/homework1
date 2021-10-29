using System;
using System.Collections;
using UnityEngine;

namespace GameMechanics.Player.Weapon
{
    [RequireComponent(typeof(Rigidbody2D),
        typeof(CircleCollider2D))]
    public class Rocket : MonoBehaviour
    {
        [SerializeField] private GameObject flightEffectPrefab;
        private ParticleSystem _flightEffect;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _basePosition;
        private Vector2 _enemyPosition;
        private bool _isShot;

        public event Action<int, Vector2> Exploded;
        public int Damage { get; private set; } = 10; 

        public void Init()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _flightEffect = Instantiate(flightEffectPrefab).GetComponent<ParticleSystem>();
            _flightEffect.gameObject.SetActive(false);
        }

        public void SetBasicPosition(Vector2 pos)
        {
            _basePosition = pos;
            transform.position = _basePosition;
        }

        public void Shot(Vector2 pos, Quaternion rotation)
        {
            transform.position = _basePosition;
            _flightEffect.transform.position = _basePosition;
            transform.rotation = rotation;
            _enemyPosition = pos;
            _flightEffect.gameObject.SetActive(true);
            _isShot = true;
        }    

        private void CallExplosion()
        {
            _rigidbody2D.velocity = Vector2.zero;
            _flightEffect.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (_isShot)
            {
                var position = transform.position;
                _rigidbody2D.AddForce(_enemyPosition - (Vector2) position, ForceMode2D.Force);
                _flightEffect.transform.position = position;
                if (Vector2.Distance(transform.position,_enemyPosition) < 0.01f)
                {
                    _isShot = false;
                    Exploded?.Invoke(0, transform.position);
                    CallExplosion();
                }
            }
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Asteroid"))
            {
                _isShot = false;
                CallExplosion();
                Exploded?.Invoke(Damage, transform.position);
            }
        }
    }
}