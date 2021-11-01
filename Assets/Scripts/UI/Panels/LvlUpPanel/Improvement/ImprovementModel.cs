using System;
using UnityEngine;

namespace UI.Panels.LvlUpPanel.Improvement
{
    public class ImprovementModel
    {
        private bool _isEnable;
        public event Action ChangeEnable;
        public bool Enable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                ChangeEnable?.Invoke();
            }
        }

        private Sprite _icon;
        public event Action ChangeIcon;
        public Sprite Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                ChangeIcon?.Invoke();
            }
        }

        private string _text;
        public event Action ChangeText;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                ChangeText?.Invoke();
            }
        }

        private string _upText;
        public event Action ChangeUpText;
        public string UpText
        {
            get => _upText;
            set
            {
                _upText = value;
                ChangeUpText?.Invoke();
            }
        }

        private int _money;
        public event Action ChangeMoney;
        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                ChangeMoney?.Invoke();
            }
        }

        public int Level { get; set; }

        public ImprovementType Type { get; set; }
        public int SpentMoney { get; set; }
        public int Damage { get; set; }
        public int Cooldown { get; set; }
    }
}