using System;
using GameMechanics.Helpers;
using GameMechanics.Sound;
using UI.Buttons;
using UI.Panels.StartMenu.CreatorsButton;
using UI.Panels.StartMenu.ExitButton;
using UI.Panels.StartMenu.LaunchButton;
using UI.Panels.StartMenu.SettingsButton;
using UI.Sound;
using UnityEngine;
using Zenject;

namespace UI.Panels.StartMenu
{
    public class StartMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject startMenu;
        [Space(10)] 
        [SerializeField] private UIButton launchButton;
        [SerializeField] private UIButton exitButton;
        [Space(10)] 
        [SerializeField] private SettingsMenuManager settingsMenuManager;
        [SerializeField] private UIButton settingsButton;
        [Space(10)] 
        [SerializeField] private CreatorsMenuManager creatorsMenuManager;
        [SerializeField] private UIButton creatorsButton;
        [Space(10)] 
        [SerializeField] private AudioClip clickSound;
        [SerializeField] private AudioClip menuMusic;

        private InjectionObjectFactory _factory;
        private SoundClaspRepository _soundClaspRepository;
        private MusicClaspRepository _musicClaspRepository;
        
        public LaunchManager LaunchManager { get; private set; }
        public ExitManager ExitManager { get; private set; }

        [Inject]
        private void Construct(InjectionObjectFactory factory, MusicClaspRepository musicClaspRepository)
        {
            _factory = factory;
            _musicClaspRepository = musicClaspRepository;
        }
        public void Init()
        {
            InitSound();
            InitLaunchButton();
            InitSettingsButton();
            InitCreatorsButton();
            InitExitButton();
            InitMusic();
        }

        private void InitSound()
        {
            _soundClaspRepository = _factory.Create<SoundClaspRepository>();
        }

        private void InitMusic()
        {
            _musicClaspRepository.AddMusic(menuMusic);
        }

        private void InitLaunchButton()
        {
            LaunchManager = _factory.Create<LaunchManager>();
            LaunchManager.SetView(launchButton);
            LaunchManager.LaunchGame += DisableStartMenu;
            LaunchManager.OnOpen();
            _soundClaspRepository.AddSoundToButton(clickSound, launchButton);
        }

        private void InitSettingsButton()
        {
            settingsMenuManager.ComeBack += EnableStartMenu;
            settingsMenuManager.OnOpen();
            _soundClaspRepository.AddSoundToButton(clickSound, settingsButton);
        }

        private void InitCreatorsButton()
        {
            creatorsMenuManager.ComeBack += EnableStartMenu;
            creatorsMenuManager.OnOpen();
            _soundClaspRepository.AddSoundToButton(clickSound, creatorsButton);
        }

        private void InitExitButton()
        {
            ExitManager = _factory.Create<ExitManager>();
            ExitManager.SetView(exitButton);
            ExitManager.OnOpen();
            _soundClaspRepository.AddSoundToButton(clickSound, exitButton);
        }

        private void DisableStartMenu()
        {
            startMenu.SetActive(false);
        }

        public void EnableStartMenu()
        {
            startMenu.SetActive(true);
            InitMusic();
        }

        private void OnDestroy()
        {
            ExitManager?.OnClose();

            if (LaunchManager != null)
            {
                LaunchManager.LaunchGame -= DisableStartMenu;
                LaunchManager.OnClose();
            }
            
            _soundClaspRepository?.OnClose();
        }
    }
}