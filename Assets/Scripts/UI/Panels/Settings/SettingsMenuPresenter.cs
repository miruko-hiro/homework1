using System;

namespace UI.Panels.Settings
{
    public class SettingsMenuPresenter
    {
        private readonly ISettingsMenu _settingsMenu;

        public SettingsMenuPresenter(ISettingsMenu settingsMenu)
        {
            _settingsMenu = settingsMenu;
        }

        public void OnOpen(Action actionClickSound, Action actionClickMusic, Action actionClickReturn)
        {
            _settingsMenu.ClickSoundButton += actionClickSound;
            _settingsMenu.ClickMusicButton += actionClickMusic;
            _settingsMenu.ClickReturnButton += actionClickReturn;
        }

        public void OnClose(Action actionClickSound, Action actionClickMusic, Action actionClickReturn)
        {
            _settingsMenu.ClickSoundButton -= actionClickSound;
            _settingsMenu.ClickMusicButton -= actionClickMusic;
            _settingsMenu.ClickReturnButton -= actionClickReturn;
        }
    }
}