using System;
using System.Collections;
using GameMechanics.Behaviors;
using UnityEngine;

namespace GameMechanics
{
    [RequireComponent(typeof(TurnBehavior))]
    public class Spaceship : MonoBehaviour
    {
        [SerializeField] private GameObject[] spaceships = new GameObject[5];
        [SerializeField] private LineRenderer laserRenderer;
        [SerializeField] private SpriteRenderer shotEffect;
        private TurnBehavior _turnBehavior;
        
        private int _lvlIndex = -1;

        private void Start()
        {
            _turnBehavior = GetComponent<TurnBehavior>();
            laserRenderer.useWorldSpace = true;
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

        public void ShotLaser(Vector2 posEnemy)
        {
            if(!laserRenderer.enabled)
                laserRenderer.enabled = true;
            if (!shotEffect.enabled)
                shotEffect.enabled = true;
            
            laserRenderer.SetPositions(new Vector3[]{transform.position, posEnemy});
            
            transform.rotation = _turnBehavior.GetRotationRelativeToAnotherObject(
                transform.position,
                posEnemy);

            StartCoroutine(StopLaser());
        }

        private IEnumerator StopLaser()
        {
            yield return new WaitForSeconds(0.1f);
            if(laserRenderer.enabled)
                laserRenderer.enabled = false;
            if (shotEffect.enabled)
                shotEffect.enabled = false;
        }
    }
}
