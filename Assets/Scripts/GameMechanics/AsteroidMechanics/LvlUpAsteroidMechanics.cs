using UnityEngine;

namespace GameMechanics.AsteroidMechanics
{
    public class LvlUpAsteroidMechanics : MonoBehaviour
    {
        private int _lvlAsteroids = 1;

        public int LvlAsteroids
        {
            get => _lvlAsteroids;
            set => _lvlAsteroids = value;
        }

        private int _healthAsteroid = 10;

        public int HealthAsteroid
        {
            get => _healthAsteroid;
            set => _healthAsteroid = value;
        }

        private int _moneyAsteroid = 1;

        public int MoneyAsteroid
        {
            get => _moneyAsteroid;
            set => _moneyAsteroid = value;
        }

        public void HealthUp()
        {
            int i = 10;
            if (LvlAsteroids > 70) i = 70;
            else if (LvlAsteroids > 60) i = 60;
            else if (LvlAsteroids > 50) i = 50;
            else if (LvlAsteroids > 30) i = 40;
            else if (LvlAsteroids > 20) i = 30;
            else if (LvlAsteroids > 10) i = 20;
            HealthAsteroid = i;
        }

        public void MoneyUp()
        {
            int i = 1;
            if (LvlAsteroids > 70) i = 7;
            else if (LvlAsteroids > 60) i = 6;
            else if (LvlAsteroids > 50) i = 5;
            else if (LvlAsteroids > 30) i = 4;
            else if (LvlAsteroids > 20) i = 3;
            else if (LvlAsteroids > 10) i = 2;
            MoneyAsteroid = i;
        }
    }
}
