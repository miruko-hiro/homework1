using UnityEngine;

namespace GameMechanics.CameraScripts
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        private CameraShake _cameraShake;
        private CameraAnimation _cameraAnimation;

        public void Init()
        {
            _cameraShake = mainCamera.GetComponent<CameraShake>();
            _cameraAnimation = mainCamera.GetComponent<CameraAnimation>();
        }

        public void CameraToDefaultPosition()
        {
            _cameraAnimation.GameLaunchAnimation();
        }

        public void CameraToGoldenModePosition()
        {
            _cameraAnimation.EnableGoldenMode();
        }

        public Vector3 GetValueToWorldPoint(Vector3 pos)
        {
            return mainCamera.ScreenToWorldPoint(pos);
        }

        public void CameraShakeDueToAsteroidExplosion(Vector2 pos)
        {
            _cameraShake.StartShaking(0.6f, 0.05f);
        }

        public void CameraShakeDueToGoldenAsteroidExplosion(Vector2 pos)
        {
            _cameraShake.StartShaking(0.6f, 0.04f);
        }
    }
}