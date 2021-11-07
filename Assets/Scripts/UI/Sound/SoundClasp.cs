using GameMechanics.Interfaces;
using GameMechanics.Sound;
using UI.Interfaces;
using UnityEngine;

namespace UI.Sound
{
    public class SoundClasp: IManager
    {
        private readonly AudioClip _sound;
        private readonly IButton _button;
        private readonly SoundManager _soundManager;
        
        public SoundClasp(AudioClip sound, IButton button, SoundManager soundManager)
        {
            _sound = sound;
            _button = button;
            _soundManager = soundManager;
        }

        public void OnOpen()
        {
            _button.Click += EnableSound;
        }

        private void EnableSound()
        {
            _soundManager.CreateSoundObjectDontDestroy()?.Play(_sound);
        }

        public void OnClose()
        {
            _button.Click -= EnableSound;
        }
    }
}