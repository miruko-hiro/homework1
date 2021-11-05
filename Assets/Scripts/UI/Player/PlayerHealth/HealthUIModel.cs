using GameMechanics.Behaviors;

namespace UI.Player.PlayerHealth
{
    public class HealthUIModel
    {
        public Health Health;
        public int IndexHpIcon { get; set; }

        public HealthUIModel(Health health)
        {
            Health = health;
            IndexHpIcon = 2;
        }
    }
}