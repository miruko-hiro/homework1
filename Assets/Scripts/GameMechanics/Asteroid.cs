using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    [RequireComponent(
        typeof(HealthEntity), 
        typeof(AttackEntity),
        typeof(MovementEntity)
    )]
    [RequireComponent(typeof(ScaleEntity))]
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Animation _animation;
        
        [Space(10)]
        [SerializeField] private Sprite[] asteroids = new Sprite[10];

        private HealthEntity _healthEntity;
        private AttackEntity _attackEntity;
        private MovementEntity _movementEntity;
        private ScaleEntity _scaleEntity;

        public static string Tag = "Asteroid";

        public event Action DeathAsteroid;
        
        public bool Enable{ get; private set; }

        private void Start()
        {
            _animation = GetComponent<Animation>();

            DefiningEntities();
            
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
            _healthEntity.SetHealthOfEntity(health);
        }

        public void SetDamage(int damage)
        {
            _attackEntity.SetAttackOfEntity(damage);
        }

        public void Move(Vector2 pos, float speed)
        {
            _movementEntity.Move(pos, speed);
        }

        public void SetScale(float scaleX, float scaleY, float maxScaleX, float maxScaleY)
        {
            _scaleEntity.SetScale(scaleX, scaleY);
            _scaleEntity.SetMaxScale(maxScaleX, maxScaleY);
            _scaleEntity.ActiveScale(true);
        }
        
        private void DefiningEntities()
        {
            _healthEntity = GetComponent<HealthEntity>();
            
            _attackEntity = GetComponent<AttackEntity>();

            _movementEntity = GetComponent<MovementEntity>();

            _scaleEntity = GetComponent<ScaleEntity>();
        }
        
        public void TakeDamage(int hit)
        {
            if (_healthEntity.Health > 0)
            {
                _animation.Stop();
                _animation.Play();
                _healthEntity.TakeDamage(hit);
                
                if (_healthEntity.Health == 0)
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
            _movementEntity.StopMove();
            DeathAsteroid?.Invoke();
        }
    }
}
