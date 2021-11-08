using DG.Tweening;
using UnityEngine;

namespace GameMechanics.CameraScripts
{
    public class CameraAnimation: MonoBehaviour
    {
        private Camera _camera;
        private const float Duration = 0.5f;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            _camera.orthographicSize = 6;
        }

        public void GameLaunchAnimation()
        {
            _camera.DOOrthoSize(5, Duration);
        }

        public void EnableGoldenMode()
        {
            _camera.DOOrthoSize(4, Duration);
        }
    }
}