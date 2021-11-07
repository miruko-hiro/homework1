using System;
using GameMechanics.Sound;
using UI.Interfaces.SwitchButton;
using UI.Sound;

namespace UI.Panels.StartMenu.SettingsButton.MusicButton
{
    public class MusicButtonModel: ISwitchButtonModel
    {
        public event Action ChangeEnabled;

        private readonly MusicManager _musicManager;
        private readonly MusicClaspRepository _musicClaspRepository;
        public bool Enabled
        {
            get => _musicManager.OnMusic;
            set
            {
                _musicManager.OnMusic = value;
                if(_musicManager.OnMusic) _musicClaspRepository.OnOpen();
                else _musicClaspRepository.OnClose();
                ChangeEnabled?.Invoke();
            }
        }

        public MusicButtonModel(MusicClaspRepository musicClaspRepository, MusicManager musicManager)
        {
            _musicClaspRepository = musicClaspRepository;
            _musicManager = musicManager;
        }
    }
}