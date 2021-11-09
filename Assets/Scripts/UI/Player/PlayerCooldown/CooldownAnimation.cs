using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Player.PlayerCooldown
{
    public class CooldownAnimation : MonoBehaviour
    {
        [SerializeField] private Image imagePanel; 
        [SerializeField] private Image imageIconRocket; 
        [SerializeField] private Image imageCooldown;
        [SerializeField] private Text textCooldown;
        private const float Duration = 0.3f;

        private Sequence _sequenceSpawnAnimation;
        private Sequence _sequenceFadeAnimation;

        private void Start()
        {
            Color imageColor = imagePanel.color;
            imageColor.a = 0;
            imagePanel.color = imageColor;
                
            imageColor = imageIconRocket.color;
            imageColor.a = 0;
            imageIconRocket.color = imageColor;
                
            imageColor = imageCooldown.color;
            imageColor.a = 0;
            imageCooldown.color = imageColor;
            
            imageColor = textCooldown.color;
            imageColor.a = 0;
            textCooldown.color = imageColor;

            textCooldown.text = "";
        }

        public void SpawnAnimation()
        {
            if (_sequenceSpawnAnimation == null)
                _sequenceSpawnAnimation = DOTween.Sequence()
                    .Insert(0f, imagePanel.DOFade(1f, Duration))
                    .Insert(0f, imageIconRocket.DOFade(1f, Duration))
                    .Insert(0f, imageCooldown.DOFade(0.5f, Duration))
                    .Insert(0f, textCooldown.DOFade(1f, Duration))
                    .SetAutoKill(false)
                    .Play();
            
            else _sequenceSpawnAnimation.Restart();
        }

        public void FadeAnimation()
        {
            if (_sequenceFadeAnimation == null)
                _sequenceFadeAnimation = DOTween.Sequence()
                    .Insert(0f, imagePanel.DOFade(0f, Duration))
                    .Insert(0f, imageIconRocket.DOFade(0f, Duration))
                    .Insert(0f, imageCooldown.DOFade(0f, Duration))
                    .Insert(0f, textCooldown.DOFade(0f, Duration))
                    .SetAutoKill(false)
                    .Play();
            
            else _sequenceFadeAnimation.Restart();
        }

        public void EnableCooldownAnimation(float duration)
        {
            imageCooldown.fillAmount = 1f;
            imageCooldown.DOFillAmount(0f, duration);
        }

        private void OnDestroy()
        {
            _sequenceSpawnAnimation?.Kill();
            _sequenceFadeAnimation?.Kill();
        }
    }
}