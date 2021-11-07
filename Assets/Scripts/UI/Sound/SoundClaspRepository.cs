using System.Collections.Generic;
using GameMechanics.Interfaces;
using GameMechanics.Sound;
using UI.Interfaces;
using UI.Panels.StartMenu;
using UnityEngine;
using Zenject;

namespace UI.Sound
{
    public class SoundClaspRepository: IManager
    {
        private List<SoundClasp> _soundClasps = new List<SoundClasp>();
        private SoundManager _soundManager;
        
        [Inject]
        public SoundClaspRepository(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }

        public void AddSoundToButton(AudioClip sound, IButton button)
        {
            SoundClasp soundClasp = new SoundClasp(sound, button, _soundManager);
            soundClasp.OnOpen();
            _soundClasps.Add(soundClasp);
        }

        public void OnOpen()
        {
            foreach (var soundClasp in _soundClasps)
            {
                soundClasp.OnOpen();
            }
        }

        public void OnClose()
        {
            foreach (var soundClasp in _soundClasps)
            {
                soundClasp.OnClose();
            }
        }
    }
}