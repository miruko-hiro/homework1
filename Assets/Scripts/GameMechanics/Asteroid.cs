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

        public HealthBehavior Health { get; private set; }
        public AttackBehavior Attack { get; private set; }
        public MovementBehavior Movement { get; private set; }
        public ScaleBehavior Scale  { get; private set; }
        public int Count { get; set; }
        public bool IsCollision { get; private set; }

        public event Action Died;
        
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
            IsCollision = false;
            transform.position = position;
        }

        public void SetLocalScale(Vector2 scale)
        {
            transform.localScale = scale;
        }
        
        private void DefiningBehaviors()
        {
            Health = GetComponent<HealthBehavior>();
            
            Attack = GetComponent<AttackBehavior>();

            Movement = GetComponent<MovementBehavior>();

            Scale = GetComponent<ScaleBehavior>();
        }
        
        public void TakeDamage(int hit)
        {
            if (Health.Amount > 0)
            {
                _animation.Stop();
                _animation.Play();
                Health.TakeDamage(hit);
                
                if (Health.Amount == 0)
                {
                    StartCoroutine(Death());
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Player.Tag))
            {
                IsCollision = true;
                Died?.Invoke();
            }
        }

        private IEnumerator Death()
        {
            yield return new WaitForSeconds(0.3f);
            Movement.StopMove();
            Died?.Invoke();
        }
    }
}
