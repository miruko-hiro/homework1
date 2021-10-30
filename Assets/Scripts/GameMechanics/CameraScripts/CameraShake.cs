using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics.CameraScripts
{
    public class CameraShake : MonoBehaviour
    {
        private Coroutine _coroutineShake;
        private bool _isCoroutineRunning = false;
        private Vector3 _originalLocalPosition;

        private void Start()
        {
            _originalLocalPosition = transform.localPosition;
        }

        private IEnumerator Shaking(float duration, float strength)
        {
            _isCoroutineRunning = true;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.fixedDeltaTime;
                Vector3 newVector = _originalLocalPosition + Random.insideUnitSphere * strength;
                transform.localPosition = new Vector3(newVector.x, newVector.y, _originalLocalPosition.z);
                yield return new WaitForFixedUpdate();
            }

            transform.localPosition = _originalLocalPosition;
            _isCoroutineRunning = false;
        }
        
        public void StartShaking(float duration, float strength)
        {
            if(_isCoroutineRunning)
                StopCoroutine(_coroutineShake);
            
            _coroutineShake = StartCoroutine(Shaking(duration, strength));
        }
    }
}