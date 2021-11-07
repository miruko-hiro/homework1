using System;
using GameMechanics.Helpers;
using GameMechanics.Interfaces;
using GameMechanics.Sound;
using UI.Buttons;
using UI.Interfaces;
using UI.Interfaces.SwitchButton;
using UI.Panels.StartMenu.SettingsButton.MusicButton;
using UI.Panels.StartMenu.SettingsButton.SoundButton;
using UI.Sound;
using UnityEngine;
using Zenject;

namespace UI.Panels.StartMenu.SettingsButton
{
    public class SettingsMenuManager: MonoBehaviour, IManager
    {
        [SerializeField] private SettingsMenuSpawner spawner;
        [SerializeField] private UIButton button;
        [Space(10)] 
        [SerializeField] private AudioClip clickSound;
        [SerializeField] private AudioClip backSound;
        
        private SettingsMenuView _settingsMenu;
        private IButton _backButton;
        private SoundClaspRepository _soundClaspRepository;
        private MusicClaspRepository _musicClaspRepository;
        private InjectionObjectFactory _factory;
        private SoundManager _soundManager;
        private MusicManager _musicManager;
        
        public SoundButtonManager SoundButtonManager { get; private set; }
        public MusicButtonManager MusicButtonManager { get; private set; }

        public event Action ComeBack;
        
        [Inject]
        private void Construct(InjectionObjectFactory factory, SoundManager soundManager, MusicManager musicManager, MusicClaspRepository musicClaspRepository)
        {
            _factory = factory;
            _soundManager = soundManager;
            _musicManager = musicManager;
            _musicClaspRepository = musicClaspRepository;
        }

        public void OnOpen()
        {
            button.Click += Init;
        }

        private void Init()
        {
            if (_settingsMenu != null) return;
            
            _settingsMenu = spawner.Spawn();
            InitMenuSoundManager();
            InitSoundButton(_settingsMenu.GetSoundButtonView());
            InitMusicButton(_settingsMenu.GetMusicButtonView());
            InitBackButton(_settingsMenu.GetBackButton());
        }

        private void InitSoundButton(ISwitchButtonView view)
        {
            SoundButtonManager = new SoundButtonManager(_soundManager, view);
            SoundButtonManager.OnOpen();
            _soundClaspRepository.AddSoundToButton(clickSound, view);
        }

        private void InitMusicButton(ISwitchButtonView view)
        {
            MusicButtonManager = new MusicButtonManager(_musicClaspRepository, _musicManager, view);
            MusicButtonManager.OnOpen();
            _soundClaspRepository.AddSoundToButton(clickSound, view);
        }

        private void InitBackButton(IButton view)
        {
            _backButton = view;
            _backButton.Click += Deinit;
            _soundClaspRepository.AddSoundToButton(backSound, view);
        }

        private void InitMenuSoundManager()
        {
            _soundClaspRepository = _factory.Create<SoundClaspRepository>();
        }

        private void Deinit()
        {
            ComeBack?.Invoke();
            Destroy(_settingsMenu.gameObject);
            MusicButtonManager?.OnClose();
            SoundButtonManager?.OnClose();
        }

        public void OnClose()
        {
            if(_backButton != null) _backButton.Click -= Deinit;
            MusicButtonManager?.OnClose();
            SoundButtonManager?.OnClose();
            button.Click -= Init;
        }

        private void OnDestroy()
        {
            OnClose();
        }
    }
}