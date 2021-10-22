using System;
using UnityEngine;

namespace UI.Panels.Settings
{
    public class SettingsMenu : MonoBehaviour, ISettingsMenu
    {
        public event Action ClickSoundButton;
        public event Action ClickMusicButton;
        public event Action ClickReturnButton;
        
        public void OnClickSound()
        {
            ClickSoundButton?.Invoke();
        }

        public void OnClickMusic()
        {
            ClickMusicButton?.Invoke();
        }

        public void OnClickReturn()
        {
            ClickReturnButton?.Invoke();
        }
    }
}