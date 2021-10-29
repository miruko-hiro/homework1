using System;
using System.Collections;
using GameMechanics.Enemy.Asteroid;
using UnityEngine;

namespace GameMechanics.Player.Weapon
{
    [RequireComponent(typeof(Rigidbody2D),
        typeof(CircleCollider2D))]
    public class Rocket : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private GameObject explosionPrefab;
        private Transform _transformExplosion;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _basePosition;
        private Vector2 _enemyPosition;
        private bool _isShot;

        public event Action<int, Vector2> Exploded;
        public int Damage { get; private set; } = 10; 

        public void Init()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transformExplosion = Instantiate(explosionPrefab).GetComponent<Transform>();
            _transformExplosion.position = new Vector2(-10f, 0f);
            _transformExplosion.gameObject.SetActive(false);
        }

        public void SetBasicPosition(Vector2 pos)
        {
            _basePosition = pos;
            transform.position = _basePosition;
        }

        public void Shot(Vector2 pos, Quaternion rotation)
        {
            DefaultAppearance();
            transform.rotation = rotation;
            _enemyPosition = pos;
            _isShot = true;
        }    

        private IEnumerator CallExplosion()
        {
            _rigidbody2D.velocity = Vector2.zero;
            ExplosionAppearance(transform.position);
            yield return new WaitForSeconds(0.3f);
            transform.position = _basePosition;
            DefaultAppearance();
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (_isShot)
            {
                _rigidbody2D.AddForce(_enemyPosition - (Vector2) transform.position, ForceMode2D.Force);
                if (Vector2.Distance(transform.position,_enemyPosition) < 0.01f)
                {
                    _isShot = false;
                    StartCoroutine(CallExplosion());
                }
            }
        }

        private void DefaultAppearance()
        {
            _transformExplosion.gameObject.SetActive(false);
            _spriteRenderer.enabled = true;
        }

        private void ExplosionAppearance(Vector2 pos)
        {
            _spriteRenderer.enabled = false;
            _transformExplosion.position = pos;
            _transformExplosion.gameObject.SetActive(true);
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Asteroid"))
            {
                _isShot = false;
                Exploded?.Invoke(Damage, transform.position);
                StartCoroutine(CallExplosion());
            }
        }
    }
}