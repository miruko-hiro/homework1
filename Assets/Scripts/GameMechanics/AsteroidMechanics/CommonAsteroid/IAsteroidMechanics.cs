using System;
using UnityEngine;

namespace GameMechanics.AsteroidMechanics.CommonAsteroid
{
    public interface IAsteroidMechanics
    {
        public event Action<int> AsteroidDroppedMoney;
        public event Action<Vector2> AsteroidExploded;

        public void LaunchAsteroid();
        public void DisableAsteroids();
    }
}