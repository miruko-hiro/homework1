using GameMechanics.Interfaces;
using UnityEngine;

namespace GameMechanics.Sound
{
    public class MusicClasp: IManager
    {
        private readonly AudioClip _music;
        private readonly MusicManager _musicManager;
        private MusicObject _musicObject;
        
        public MusicClasp(AudioClip music, MusicManager musicManager)
        {
            _music = music;
            _musicManager = musicManager;
        }

        public void OnOpen()
        {
            EnableMusic();
        }

        public AudioClip GetMusic()
        {
            return _music;
        }

        private void EnableMusic()
        {
            _musicObject = _musicManager.CreateMusicObject();
            if (_musicObject == null) return;
            _musicObject.Play(_music, 1f, true);
        }

        public void OnClose()
        {
            if (_musicObject == null) return;
            _musicObject.Stop();
            Object.Destroy(_musicObject.gameObject);
        }
    }
}