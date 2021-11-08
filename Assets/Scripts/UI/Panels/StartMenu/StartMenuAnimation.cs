using DG.Tweening;
using UnityEngine;

namespace UI.Panels.StartMenu
{
    public class StartMenuAnimation : MonoBehaviour
    {
        private const float Duration = 1.5f;
        private void Start()
        {
            GetComponent<RectTransform>().DOAnchorPosY(1100f, Duration).SetEase(Ease.OutBounce).From();
        }
    }
}