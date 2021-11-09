using DG.Tweening;
using UnityEngine;

namespace UI.Panels.GoldenMode
{
    public class GoldenModePanelAnimation : MonoBehaviour
    {
        private const float Duration = 0.7f;
        private RectTransform _rectTransform;

        private void OnEnable()
        {
            InitRectTransform();
            _rectTransform.DOAnchorPosX(-300f, Duration).From();
        }

        public void DisableAnimation()
        {
            _rectTransform.DOAnchorPosX(-300f, Duration);
        }

        private void InitRectTransform()
        {
            if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
        }
    }
}