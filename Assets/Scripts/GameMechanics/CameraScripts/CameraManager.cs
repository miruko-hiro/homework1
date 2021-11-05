using UnityEngine;

namespace GameMechanics.CameraScripts
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        private Animator _animatorMainCamera;
        private CameraShake _cameraShake;
        private static readonly int GoldenMode = Animator.StringToHash("GoldenMode");
        private static readonly int IsGoldenMode = Animator.StringToHash("isGoldenMode");

        public void Init()
        {
            _animatorMainCamera = mainCamera.GetComponent<Animator>();
            _cameraShake = mainCamera.GetComponent<CameraShake>();
        }

        public void SwitchToGoldMode()
        {
            _animatorMainCamera.SetTrigger(GoldenMode);
        }

        public void EnableGoldenModeAnimation(bool isValue)
        {
            _animatorMainCamera.SetBool(IsGoldenMode, isValue);
        }

        public Vector3 GetValueToWorldPoint(Vector3 pos)
        {
            return mainCamera.ScreenToWorldPoint(pos);
        }

        public void CameraShakeDueToAsteroidExplosion(Vector2 pos)
        {
            _cameraShake.StartShaking(0.4f, 0.025f);
        }

        public void CameraShakeDueToGoldenAsteroidExplosion(Vector2 pos)
        {
            _cameraShake.StartShaking(0.4f, 0.02f);
        }
    }
}