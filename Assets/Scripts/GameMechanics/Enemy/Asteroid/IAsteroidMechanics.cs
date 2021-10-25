using System;
using UnityEngine;

namespace GameMechanics.Enemy.Asteroid
{
    public interface IAsteroidMechanics
    {
        public event Action<int> AsteroidDroppedMoney;
        public event Action<Vector2> AsteroidExploded;

        public void LaunchAsteroid();
        public void DisableAsteroids();
    }
}