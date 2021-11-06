using System;
using UI.Interfaces.SwitchButton;

namespace UI.Panels.StartMenu.SettingsButton.SoundButton
{
    public class SoundButtonModel: ISwitchButtonModel
    {
        public event Action ChangeEnable;

        private bool _enable;
        
        public bool Enable
        {
            get => _enable;
            set
            {
                _enable = value;
                ChangeEnable?.Invoke();
            }
        }

        public SoundButtonModel()
        {
            _enable = true;
        }
    }
}