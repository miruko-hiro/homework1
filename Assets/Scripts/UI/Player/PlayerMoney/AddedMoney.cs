using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Player.PlayerMoney
{
    public class AddedMoney : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Image _image;
        private readonly Vector2 _endPosition = new Vector2(-300f, 600f);
        private const float Duration = 2f;

        public void Init()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            _rectTransform.anchoredPosition = new Vector2(-1000f, 0f);
            _image.DOFade(0f, 0.1f);
        }

        private void InitSequence()
        {
            DOTween.Sequence()
                .Insert(0f,
                    _rectTransform.DOAnchorPos(_rectTransform.anchoredPosition + 
                                               new Vector2(Random.Range(-150f, 150f), Random.Range(-150f, 150f)), Duration / 2))
                .Insert(Duration / 2, _rectTransform.DOAnchorPos(_endPosition, Duration / 2))
                .Insert(Duration / 2, _image.DOFade(0f, Duration / 2))
                .Play();
        }

        public void StartAnimation(Vector2 pos)
        {
            _rectTransform.anchoredPosition = pos;
            
            _image.DOFade(1f, 0.5f);

            InitSequence();
        }
    }
}