using System;
using System.Collections;
using GameMechanics.Behaviors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    [RequireComponent(
        typeof(HealthBehavior), 
        typeof(AttackBehavior),
        typeof(MovementBehavior)
    )]
    [RequireComponent(typeof(ScaleBehavior))]
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Animation _animation;
        
        [Space(10)]
        [SerializeField] private Sprite[] asteroids = new Sprite[10];

        public HealthBehavior HealthBehavior { get; private set; }
        public AttackBehavior AttackBehavior { get; private set; }
        public MovementBehavior MovementBehavior { get; private set; }
        public ScaleBehavior ScaleBehavior  { get; private set; }

        public static string Tag = "Asteroid";

        public event Action DeathAsteroid;
        
        public bool Enable{ get; private set; }

        private void Start()
        {
            _animation = GetComponent<Animation>();

            DefiningBehaviors();
            
            spriteRenderer.sprite = asteroids[Random.Range(0, 10)];

            Enable = true;
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetLocalScale(Vector2 scale)
        {
            transform.localScale = scale;
        }
        
        private void DefiningBehaviors()
        {
            HealthBehavior = GetComponent<HealthBehavior>();
            
            AttackBehavior = GetComponent<AttackBehavior>();

            MovementBehavior = GetComponent<MovementBehavior>();

            ScaleBehavior = GetComponent<ScaleBehavior>();
        }
        
        public void TakeDamage(int hit)
        {
            if (HealthBehavior.Health > 0)
            {
                _animation.Stop();
                _animation.Play();
                HealthBehavior.TakeDamage(hit);
                
                if (HealthBehavior.Health == 0)
                {
                    StartCoroutine(Death());
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Player.Tag))
            {
                Debug.Log("Bum");
                DeathAsteroid?.Invoke();
            }
        }

        private IEnumerator Death()
        {
            yield return new WaitForSeconds(0.3f);
            MovementBehavior.StopMove();
            DeathAsteroid?.Invoke();
        }
    }
}
