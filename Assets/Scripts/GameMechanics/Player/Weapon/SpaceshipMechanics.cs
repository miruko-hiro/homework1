using System;
using System.Collections.Generic;
using GameMechanics.Behaviors;
using GameMechanics.Player.Planet;
using UnityEngine;

namespace GameMechanics.Player.Weapon
{
    public class SpaceshipMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject spaceshipPrefab;
        private List<Spaceship> _spaceshipList = new List<Spaceship>();
        private int _numberSpaceships = 1;

        [SerializeField] private GameObject spaceshipWithRocketsPrefab;
        private SpaceshipWithRockets _spaceshipWithRockets;

        private event Action<Vector2> Shoot;
        public event Action<int> RocketCooldown;
        public void Init()
        {
            _spaceshipList.Add(Instantiate(spaceshipPrefab).GetComponent<Spaceship>());
            _spaceshipList[0].UpLvl();
            _spaceshipList[0].SetPosition(new Vector2(-2f, -1.1f));
            Shoot += _spaceshipList[0].ShotLaser;
        }

        public void SpaceshipsShoot(Vector2 posEnemy)
        {
            Shoot?.Invoke(posEnemy);
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
                
                Shoot += _spaceshipList[_numberSpaceships].ShotLaser;
                _numberSpaceships += 1;
            }
        }

        public void AddSpaceshipWithRocket(CooldownBehavior cooldownBehavior)
        {
            _spaceshipWithRockets = Instantiate(spaceshipWithRocketsPrefab).GetComponent<SpaceshipWithRockets>();
            _spaceshipWithRockets.Init(new Vector2(-1f, -1.2f), cooldownBehavior);
            _spaceshipWithRockets.RocketCooldown += RocketCooldown;
            Shoot += _spaceshipWithRockets.ShotRacket;
        }

        private void OnDestroy()
        {
            if (_spaceshipList.Count > 0)
            {
                foreach (Spaceship spaceship in _spaceshipList)
                {
                    Shoot -= spaceship.ShotLaser;
                }
            }

            if (_spaceshipWithRockets)
            {
                Shoot -= _spaceshipWithRockets.ShotRacket;
                _spaceshipWithRockets.RocketCooldown -= RocketCooldown;
            }
        }
    }
}