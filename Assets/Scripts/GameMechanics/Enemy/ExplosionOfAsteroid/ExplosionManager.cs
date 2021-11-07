using System.Collections;
using GameMechanics.Sound;
using UnityEngine;
using Zenject;

namespace GameMechanics.Enemy.ExplosionOfAsteroid
{
    public class ExplosionManager : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private GameObject explosionParent;
        [SerializeField] private AudioClip explosionSound;
        private Explosion[] _explosionArray = new Explosion[5];
        private SoundManager _soundManager;
        private int _index = 0;

        [Inject]
        private void Construct(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }
        
        public void Init()
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
            _soundManager.CreateSoundObjectDontDestroy()?.Play(explosionSound);
            
            yield return new WaitForSeconds(2f);
            
            explosion.gameObject.SetActive(false);
        }

        public void EnableExplosion(Vector2 pos)
        {
            StartCoroutine(WaitForEndOfExplosion(pos));
        }
    }
}