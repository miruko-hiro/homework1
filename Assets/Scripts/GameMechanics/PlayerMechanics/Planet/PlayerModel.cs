using System;
using GameMechanics.Behaviors;

namespace GameMechanics.PlayerMechanics.Planet
{
    public class PlayerModel
    {
        public static string Tag = "Planet";
        public event Action Died;
        
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
        
        public HealthBehavior Health { get; set; }
        public AttackBehavior Attack { get; set; }
        public MoneyBehavior Money { get; set; }
    }
}