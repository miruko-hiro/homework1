using UnityEngine;
using Zenject;

namespace GameMechanics.Sound
{
    public class MusicClaspRepository
    {
        private MusicClasp _musicClasp;
        private readonly MusicManager _musicManager;
        
        [Inject]
        public MusicClaspRepository(MusicManager musicManager)
        {
            _musicManager = musicManager;
        }

        public void AddMusic(AudioClip music)
        {
            if (_musicClasp != null)
            {
                if(_musicClasp.GetMusic() == music) return;
                OnClose();
            }
            _musicClasp = new MusicClasp(music, _musicManager);
            _musicClasp.OnOpen();
        }

        public void OnOpen()
        {
            _musicClasp.OnOpen();
        }

        public void OnClose()
        {
            _musicClasp.OnClose();
        }
    }
}