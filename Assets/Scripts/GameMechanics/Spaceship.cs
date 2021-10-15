using System;
using UnityEngine;

namespace GameMechanics
{
    public class Spaceship : MonoBehaviour
    {
        [SerializeField] private GameObject[] spaceships = new GameObject[5];
        [SerializeField] private LineRenderer laser;
        [SerializeField] private SpriteRenderer shotEffect;
        
        private int _lvlIndex = -1;

        private void Start()
        {
            laser.useWorldSpace = true;
            shotEffect.enabled = false;
        }

        public void UpLvl()
        {
            if (_lvlIndex < 5)
            {
                _lvlIndex += 1;
                
                foreach (GameObject spaceship in spaceships)
                {
                    spaceship.SetActive(false);
                }

                spaceships[_lvlIndex].SetActive(true);
            }
        }

        public void SetPosition(Vector2 pos)
        {
            transform.position = pos;
        }

        public void LaserShot(Vector2 posEnemy)
        {
            if(!laser.enabled)
                laser.enabled = true;
            if (!shotEffect.enabled)
                shotEffect.enabled = true;
            
            laser.SetPositions(new Vector3[]{transform.position, posEnemy});

            Vector3 v1 = transform.InverseTransformDirection(posEnemy);
            Vector3 v2 = transform.position;
            transform.up =  new Vector3(v1.x - v2.x, v1.y - v2.y, 0f);
        }

        public void LaserStop()
        {
            if(laser.enabled)
                laser.enabled = false;
            if (shotEffect.enabled)
                shotEffect.enabled = false;
        }
    }
}
