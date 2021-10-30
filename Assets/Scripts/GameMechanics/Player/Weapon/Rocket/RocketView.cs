using System;
using UnityEngine;

namespace GameMechanics.Player.Weapon.Rocket
{
    [RequireComponent(typeof(Rigidbody2D),
        typeof(CircleCollider2D))]
    public class RocketView : MonoBehaviour
    {
        [SerializeField] private GameObject flightEffectPrefab;
        private ParticleSystem _flightEffect;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _basePosition;
        private Vector2 _enemyPosition;

        public delegate int Damage();

        private Damage _damage = null;
        public bool IsShot { get; private set; }

        public event Action<bool, Vector2> Exploded;

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

        public void SetDamage(Damage funcDamage)
        {
            _damage ??= funcDamage;
        }

        public int GetDamage()
        {
            return _damage?.Invoke() ?? 0;
        }

        public void Shot(Vector2 pos, Quaternion rotation)
        {
            transform.position = _basePosition;
            _flightEffect.transform.position = _basePosition;
            transform.rotation = rotation;
            _enemyPosition = pos;
            _flightEffect.gameObject.SetActive(true);
            IsShot = true;
        }    

        private void CallExplosion()
        {
            _rigidbody2D.velocity = Vector2.zero;
            _flightEffect.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (IsShot)
            {
                var position = transform.position;
                _rigidbody2D.AddForce(_enemyPosition - (Vector2) position, ForceMode2D.Force);
                _flightEffect.transform.position = position;
                if (Vector2.Distance(transform.position,_enemyPosition) < 0.01f)
                {
                    IsShot = false;
                    Exploded?.Invoke(false, transform.position);
                    CallExplosion();
                }
            }
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Asteroid"))
            {
                IsShot = false;
                CallExplosion();
                Exploded?.Invoke(true, transform.position);
            }
        }
    }
}