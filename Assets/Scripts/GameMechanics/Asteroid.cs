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

        private MainMechanics _controller;
        private Transform _transform;

        private void Start()
        {

            _transform = GetComponent<Transform>();
            _transform.localScale = new Vector2(0.1f, 0.1f);
            _transform.position = new Vector2(Random.Range(-1.5f, 1.6f), Random.Range(1.5f, 4f));
            
            _healthEntity = GetComponent<HealthEntity>();
            _healthEntity.SetHealthOfEntity(10);
            
            _attackEntity = GetComponent<AttackEntity>();
            _attackEntity.SetAttackOfEntity(1);

            _movementEntity = GetComponent<MovementEntity>();
            _movementEntity.Move(new Vector2(-0.5f, -3f), 0.2f);

            _scaleEntity = GetComponent<ScaleEntity>();
            _scaleEntity.SetScale(0.005f, 0.005f);
            _scaleEntity.SetMaxScale(1.7f, 1.7f);
            _scaleEntity.ActiveScale(true);
            
            spriteRenderer.sprite = asteroids[Random.Range(0, 10)];

            _animation = GetComponent<Animation>();
            
            _controller = GetComponentInParent<MainMechanics>();
            _controller.AsteroidClick += TakeDamage;
        }
        
        private void TakeDamage(int hit)
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

        private IEnumerator Death()
        {
            yield return new WaitForSeconds(0.3f);
            _movementEntity.StopMove();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _controller.AsteroidClick -= TakeDamage;
        }
    }
}
