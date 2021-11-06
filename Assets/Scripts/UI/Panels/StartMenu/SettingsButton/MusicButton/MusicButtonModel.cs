using System;
using UI.Interfaces.SwitchButton;

namespace UI.Panels.StartMenu.SettingsButton.MusicButton
{
    public class MusicButtonModel: ISwitchButtonModel
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

        public MusicButtonModel()
        {
            _enable = true;
        }
    }
}