using GameMechanics.Behaviors;
using UnityEngine;

namespace GameMechanics.Player.Weapon.Rocket
{
    public class RocketModel
    {
        public Attack Attack { get; set; }
        public Cooldown Cooldown { get; set; }
        public Vector2 Position { get; set; }
    }
}