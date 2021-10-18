using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics.AsteroidMechanics
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private GameObject[] explosionArray = new GameObject[3];

        private void OnEnable()
        {
            foreach (GameObject explosion in explosionArray)
            {
                explosion.SetActive(false);
            }
            
            explosionArray[Random.Range(0, 3)].SetActive(true);
        }

        public void SetPosition(Vector2 pos)
        {
            transform.position = pos;
        }
    }
}
