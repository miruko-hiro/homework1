using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Space(10)]
        [SerializeField] private Sprite[] asteroids = new Sprite[10];
        
        public event Action ChangeHealth;

        public static string Tag = "Asteroid";

        public int MaxHealth { get; private set; }
        public int Health { get; private set; }

        private MainMechanics _controller;
        private void Start()
        {
            MaxHealth = 10;
            Health = 10;
            
            spriteRenderer.sprite = asteroids[Random.Range(0, 11)];
            
            _controller = GetComponentInParent<MainMechanics>();
            _controller.AsteroidClick += TakeDamage;
        }
        
        private void TakeDamage(int hit)
        {
            Health -= hit;
            ChangeHealth?.Invoke();
            if (Health == 0)
            {
                StartCoroutine(Death());
            }
        }

        private IEnumerator Death()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _controller.AsteroidClick -= TakeDamage;
        }
    }
}
