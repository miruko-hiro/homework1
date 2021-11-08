using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics.CameraScripts
{
    public class CameraShake : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }
        
        public void StartShaking(float duration, float strength)
        {
            _camera.DOShakePosition(duration, strength);
        }
    }
}