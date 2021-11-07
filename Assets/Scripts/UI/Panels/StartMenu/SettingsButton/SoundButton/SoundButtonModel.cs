using System;
using GameMechanics.Sound;
using UI.Interfaces.SwitchButton;

namespace UI.Panels.StartMenu.SettingsButton.SoundButton
{
    public class SoundButtonModel: ISwitchButtonModel
    {
        public event Action ChangeEnabled;

        private readonly SoundManager _soundManager;
        
        public bool Enabled
        {
            get => _soundManager.OnSound;
            set
            {
                _soundManager.OnSound = value;
                ChangeEnabled?.Invoke();
            }
        }

        public SoundButtonModel(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }
    }
}