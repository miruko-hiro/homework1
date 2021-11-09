using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Player.PlayerMoney
{
    public class MoneyUIView : MonoBehaviour, IMoneyUIView
    {
        [SerializeField] private Text amountOfMoney;
        [SerializeField] private Text amountOfAddedMoney;
        [SerializeField] private Image imageAddedMoney;
        private RectTransform _rectTransformImage;
        private RectTransform _rectTransformText;
        private Sequence _sequence;
        private const float Duration = 1f;

        private void Start()
        {
            _rectTransformImage = imageAddedMoney.GetComponent<RectTransform>();
            _rectTransformText = amountOfAddedMoney.GetComponent<RectTransform>();
        }

        public void ChangeAmountOfAddedMoney(string money)
        {
            InitSequence();
            amountOfAddedMoney.text = "+" + money;
        }

        private void InitSequence()
        {
            if (_sequence != null)
            {
                _sequence.Restart();
                return;
            }

            _sequence = DOTween.Sequence()
                .Insert(0.9f, imageAddedMoney.DOFade(1f, 0.1f))
                .Insert(0.9f, amountOfAddedMoney.DOFade(1f, 0.1f))
                .Insert(1.2f, _rectTransformImage.DOAnchorPosY(80f, Duration))
                .Insert(1.2f, _rectTransformText.DOAnchorPosY(80f, Duration))
                .Insert(1.2f, imageAddedMoney.DOFade(0f, Duration))
                .Insert(1.2f, amountOfAddedMoney.DOFade(0f, Duration))
                .SetAutoKill(false)
                .Play();
        }
        
        public void ChangeAmountOfMoney(string money)
        {
            amountOfMoney.text = money;
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}
