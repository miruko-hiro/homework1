using System;
using GameMechanics.Player.Planet;
using GameMechanics.Player.Weapon;
using GameMechanics.Player.Weapon.Rocket;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics.Enemy.Asteroid
{
    public class AsteroidView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Space(10)]
        [SerializeField] private Sprite[] asteroids = new Sprite[10];

        private AsteroidAnimation _animation;
        
        public event Action Died;
        public event Action ReachedLineOfDestroy;
        public event Action<int> ReceivedDamage;
        private void Start()
        {
            _animation = GetComponent<AsteroidAnimation>();
        }

        private void OnEnable()
        {
            spriteRenderer.sprite = asteroids[Random.Range(0, asteroids.Length)];
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }
        
        public void SetLocalScale(Vector2 scale)
        {
            transform.localScale = scale;
        }

        public void TakeDamage(int hit)
        {
            ReceivedDamage?.Invoke(hit);
        }

        public void ReAnimation(int health)
        {
            if (!_animation) return;
            _animation.Hit();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Rocket"))
            {
                TakeDamage(other.gameObject.GetComponent<RocketView>().GetDamage());
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(PlayerModel.Tag))
            {
                Died?.Invoke();
            } else if (other.CompareTag("DestroyAsteroid"))
            {
                ReachedLineOfDestroy?.Invoke();
            }
        }
    }
}