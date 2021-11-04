using System;
using GameMechanics.Behaviors;
using UnityEngine;

namespace GameMechanics.Enemy.Asteroid
{
    public class AsteroidModel
    {
        public HealthBar HpBar { get; set; }
        public Health Health { get; set; }
        public Attack Attack { get; set; }
        public MovementBehavior Movement { get; set; }
        public ScaleBehavior Scale  { get; set; }
        public event Action<Vector2> ChangePosition;
        public event Action Died;
        public event Action<Vector2> ChangeLocalScale;
        public event Action ReachedLineOfDestroy;

        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                ChangePosition?.Invoke(_position);
            }
        }

        private bool _isLive;

        public bool IsLive
        {
            get => _isLive;
            set
            {
                _isLive = value;
                if(!_isLive) Died?.Invoke();
            }
        }

        private Vector2 _localScale;

        public Vector2 LocalScale
        {
            get => _localScale;
            set
            {
                _localScale = value;
                ChangeLocalScale?.Invoke(_localScale);
            }
        }
        
        private bool _isEndCard;

        public bool IsEndCard
        {
            get => _isEndCard;
            set
            {
                _isEndCard = value;
                if(_isEndCard) ReachedLineOfDestroy?.Invoke();
            }
        }
    }
}