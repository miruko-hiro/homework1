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

        private HealthBehavior _healthBehavior;
        private AttackBehavior _attackBehavior;
        private MovementBehavior _movementBehavior;
        private ScaleBehavior _scaleBehavior;

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

        public void SetHealth(int health)
        {
            _healthBehavior.SetHealthOfEntity(health);
        }

        public void SetDamage(int damage)
        {
            _attackBehavior.SetAttackOfEntity(damage);
        }

        public void Move(Vector2 pos, float speed)
        {
            _movementBehavior.Move(pos, speed);
        }

        public void SetScale(float scaleX, float scaleY, float maxScaleX, float maxScaleY)
        {
            _scaleBehavior.SetScale(scaleX, scaleY);
            _scaleBehavior.SetMaxScale(maxScaleX, maxScaleY);
            _scaleBehavior.ActiveScale(true);
        }
        
        private void DefiningBehaviors()
        {
            _healthBehavior = GetComponent<HealthBehavior>();
            
            _attackBehavior = GetComponent<AttackBehavior>();

            _movementBehavior = GetComponent<MovementBehavior>();

            _scaleBehavior = GetComponent<ScaleBehavior>();
        }
        
        public void TakeDamage(int hit)
        {
            if (_healthBehavior.Health > 0)
            {
                _animation.Stop();
                _animation.Play();
                _healthBehavior.TakeDamage(hit);
                
                if (_healthBehavior.Health == 0)
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
            _movementBehavior.StopMove();
            DeathAsteroid?.Invoke();
        }
    }
}
