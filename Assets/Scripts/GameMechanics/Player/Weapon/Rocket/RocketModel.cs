using GameMechanics.Behaviors;
using UnityEngine;

namespace GameMechanics.Player.Weapon.Rocket
{
    public class RocketModel
    {
        public AttackBehavior Attack { get; set; }
        public CooldownBehavior Cooldown { get; set; }
        public Vector2 Position { get; set; }
    }
}