using System;

namespace UI.Panels.Settings
{
    public interface ISettingsMenu
    {
        public event Action ClickSoundButton;
        public event Action ClickMusicButton;
        public event Action ClickReturnButton;
        public void OnClickSound();
        public void OnClickMusic();
        public void OnClickReturn();
    }
}