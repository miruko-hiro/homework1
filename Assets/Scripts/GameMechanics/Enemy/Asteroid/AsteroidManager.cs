using System;
using UnityEngine;

namespace GameMechanics.Enemy.Asteroid
{
    public class AsteroidManager : MonoBehaviour
    {
        [SerializeField] private GameObject asteroidController;
        [SerializeField] private GameObject asteroidPrefab;
        [SerializeField] private GameObject healthBarPrefab;
        private AsteroidController _controller;
        public AsteroidModel Model { get; private set; }
        public AsteroidView View { get; private set; }
        public event Action<AsteroidManager> Died;
        public event Action<AsteroidManager> ReachedLineOfDestroy;
        public void Init()
        {
            _controller = new AsteroidFactory().Load(asteroidController, asteroidPrefab, healthBarPrefab, transform);
            Model = _controller.Model;
            View = _controller.View;
            Model.Died += NotifyAboutDeath;
            Model.ReachedLineOfDestroy += NotifyAboutEndOfCard;
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