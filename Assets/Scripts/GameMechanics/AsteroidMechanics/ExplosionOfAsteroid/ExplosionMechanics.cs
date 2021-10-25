using System.Collections;
using UnityEngine;

namespace GameMechanics.AsteroidMechanics.ExplosionOfAsteroid
{
    public class ExplosionMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private GameObject explosionParent;
        private Explosion[] _explosionArray = new Explosion[5];
        private int _index = 0;

        private void Start()
        {
            InitExplosions();
        }

        private void InitExplosions()
        {
            for (int i = 0; i < _explosionArray.Length; i++)
            {
                _explosionArray[i] = Instantiate(explosionPrefab, explosionParent.transform).GetComponent<Explosion>();
                _explosionArray[i].gameObject.SetActive(false);
            }
        }

        private IEnumerator WaitForEndOfExplosion(Vector2 pos)
        {
            Explosion explosion = _explosionArray[_index];
            _index = _index < _explosionArray.Length - 1 ? _index += 1 : _index = 0;
            
            explosion.gameObject.SetActive(true);
            explosion.SetPosition(pos);
            
            yield return new WaitForSeconds(2f);
            
            explosion.gameObject.SetActive(false);
        }

        public void EnableExplosion(Vector2 pos)
        {
            StartCoroutine(WaitForEndOfExplosion(pos));
        }
    }
}