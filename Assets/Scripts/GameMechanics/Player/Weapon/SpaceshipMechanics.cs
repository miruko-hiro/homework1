using System.Collections.Generic;
using UnityEngine;

namespace GameMechanics.Player.Weapon
{
    public class SpaceshipMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject spaceshipPrefab;
        private List<Spaceship> _spaceshipList = new List<Spaceship>();
        private int _numberSpaceships = 1;
        public void Init()
        {
            _spaceshipList.Add(Instantiate(spaceshipPrefab).GetComponent<Spaceship>());
            _spaceshipList[0].UpLvl();
            _spaceshipList[0].SetPosition(new Vector2(-2f, -1.1f));
        }

        public void SpaceshipsShoot(Vector2 posEnemy)
        {
            foreach (var spaceship in _spaceshipList)
            {
                spaceship.ShotLaser(posEnemy);
            }
        }

        public void SpaceshipLvlUp()
        {
            foreach (Spaceship spaceship in _spaceshipList)
            {
                spaceship.UpLvl();
            }
        }

        public void AddSpaceship()
        {
            if (_numberSpaceships == 1 || _numberSpaceships == 2)
            {
                _spaceshipList.Add(Instantiate(spaceshipPrefab).GetComponent<Spaceship>());
                _spaceshipList[_numberSpaceships].UpLvl();
                if (_numberSpaceships == 1)
                    _spaceshipList[_numberSpaceships].SetPosition(new Vector2(1f, -2.5f));
                if (_numberSpaceships == 2)
                    _spaceshipList[_numberSpaceships].SetPosition(new Vector2(-0.3f, -1.3f));
                _numberSpaceships += 1;
            }
        }
    }
}