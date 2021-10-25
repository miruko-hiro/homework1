using System;
using GameMechanics.PlayerMechanics;
using GameMechanics.PlayerMechanics.Planet;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics.AsteroidMechanics.CommonAsteroid
{
    public class AsteroidView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Animation _animation;
        
        [Space(10)]
        [SerializeField] private Sprite[] asteroids = new Sprite[10];
        
        public event Action Died;
        public event Action ReachedLineOfDestroy;
        public event Action<int> ReceivedDamage;
        private void Start()
        {
            _animation = GetComponent<Animation>();
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
            _animation.Stop();
            _animation.Play();
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