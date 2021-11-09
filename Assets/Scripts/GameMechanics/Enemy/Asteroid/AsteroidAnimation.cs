using System;
using DG.Tweening;
using UnityEngine;

namespace GameMechanics.Enemy.Asteroid
{
    public class AsteroidAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer rendererImage;
        private Transform _transform;
        
        private const float Duration = 0.25f;
        private Sequence _sequence;

        private void Start()
        {
            _transform = rendererImage.GetComponent<Transform>();
        }

        public void Hit()
        {
            if (_sequence == null) InitSequence();
            _sequence.Restart();
        }

        private void InitSequence()
        {
            _sequence = DOTween.Sequence()
                .Insert(0f, _transform.DOShakeRotation(Duration, new Vector3(0f, 0f, 50f)))
                .Insert(0f, _transform.DOScale(new Vector3(0.5f, 0.5f, 1f), Duration))
                .Insert(0f, rendererImage.DOColor(new Color(100f, 100f, 100f), Duration))
                .Insert(Duration, _transform.DOScale(Vector3.one, Duration))
                .Insert(Duration, rendererImage.DOColor(new Color(255f, 255f, 255f), Duration))
                .SetAutoKill(false);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}