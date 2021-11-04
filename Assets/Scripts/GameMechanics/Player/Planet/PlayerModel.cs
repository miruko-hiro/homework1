using System;
using GameMechanics.Behaviors;

namespace GameMechanics.Player.Planet
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
        
        public Health Health { get; set; }
        public Attack LaserAttack { get; set; }
        public Money Money { get; set; }
    }
}