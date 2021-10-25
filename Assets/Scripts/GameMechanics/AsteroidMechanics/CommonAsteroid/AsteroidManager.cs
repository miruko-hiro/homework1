using System;
using UnityEngine;

namespace GameMechanics.AsteroidMechanics.CommonAsteroid
{
    public class AsteroidManager : MonoBehaviour
    {
        [SerializeField] private GameObject asteroidController;
        [SerializeField] private GameObject asteroidPrefab;
        [SerializeField] private GameObject healthBarPrefab;
        private AsteroidController _controller;
        public event Action<AsteroidManager> Died;
        public event Action<AsteroidManager> ReachedLineOfDestroy;
        public bool Enable { get; set; } = false;
        private void Start()
        {
            _controller = new AsteroidFactory().Load(asteroidController, asteroidPrefab, healthBarPrefab, transform);
            _controller.Model.Died += NotifyAboutDeath;
            _controller.Model.ReachedLineOfDestroy += NotifyAboutEndOfCard;
            Enable = true;
        }

        public void TakeDamage(int hit)
        {
            _controller.Health.Decrease(hit);
        }

        public void SetPosition(Vector2 pos)
        {
            _controller.Model.Position = pos;
        }

        public Vector2 GetPosition()
        {
            return _controller.View.transform.position;
        }

        public void SetLocalScale(Vector2 localScale)
        {
            _controller.Model.LocalScale = localScale;
        }

        public void SetScale(Vector2 scale)
        {
            _controller.Scale.Scale = scale;
        }

        public void SetMaxScale(Vector2 maxScale)
        {
            _controller.Scale.MaxScale = maxScale;
        }

        public void ActiveScale()
        {
            _controller.Scale.Active(true, _controller.View.transform);
        }

        public void SetHealth(int health)
        {
            _controller.Health.SetAmount(health);
        }

        public void SetAttack(int damage)
        {
            _controller.Attack.SetAmount(damage);
        }

        public void SetMotionParameters(Vector2 endPointOfMovement, float speed)
        {
            _controller.Movement.Move(endPointOfMovement, speed, _controller.View.transform);
        }


        private void NotifyAboutDeath()
        {
            Died?.Invoke(this);
        }

        private void NotifyAboutEndOfCard()
        {
            ReachedLineOfDestroy?.Invoke(this);
        }
        
        private void OnDestroy()
        {
            _controller.Model.Died -= NotifyAboutDeath;
            _controller.Model.ReachedLineOfDestroy -= NotifyAboutEndOfCard;
            _controller.OnClose();
        }
    }
}