using System;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ImprovementUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Text damageText;
        [SerializeField] private Text damageUpText;
        [SerializeField] private Text money;
        [SerializeField] private Button button;
        public UIButton LvlButton { get; private set; }

        private bool _isEnable;
        public bool Enable
        {
            get => _isEnable;
            set
            {
                button.interactable = value;
                _isEnable = value;
            }
        }

        private void Start()
        {
            LvlButton = button.GetComponent<UIButton>();
        }

        public Sprite Icon
        {
            get => icon.sprite;
            set => icon.sprite = value;
        }
        
        public string DamageText
        {
            get => damageText.text;
            set => damageText.text = value;
        }
        
        public string DamageUpText
        {
            get => damageUpText.text;
            set => damageUpText.text = value;
        }
        
        public string Money
        {
            get => money.text;
            set => money.text = value;
        }

        public int Index
        {
            get;
            set;
        }
    }
}
