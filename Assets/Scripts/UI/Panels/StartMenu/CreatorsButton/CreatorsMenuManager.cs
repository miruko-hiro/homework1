using System;
using GameMechanics.Helpers;
using GameMechanics.Interfaces;
using UI.Buttons;
using UI.Interfaces;
using UI.Panels.StartMenu.CreatorsButton.GooglePlayButton;
using UI.Sound;
using UnityEngine;
using Zenject;

namespace UI.Panels.StartMenu.CreatorsButton
{
    public class CreatorsMenuManager: MonoBehaviour, IManager
    {
        [SerializeField] private CreatorsMenuSpawner spawner;
        [SerializeField] private UIButton button;
        [Space(10)] 
        [SerializeField] private AudioClip clickSound;
        [SerializeField] private AudioClip backSound;
        
        private CreatorsMenuView _creatorsMenu;
        private IButton _backButton;
        private SoundClaspRepository _soundClaspRepository;
        private InjectionObjectFactory _factory;
         
        public GooglePlayButtonManager GooglePlayButtonManager { get; private set; }

        public event Action ComeBack;
        
        [Inject]
        private void Construct(InjectionObjectFactory factory)
        {
            _factory = factory;
        }
        
        public void OnOpen()
        {
            button.Click += Init;
        }

        private void Init()
        {
            if (_creatorsMenu != null) return;
            
            _creatorsMenu = spawner.Spawn();
            InitMenuSoundManager();
            InitGooglePlayButton(_creatorsMenu.GetGooglePlayButtonView());
            InitBackButton(_creatorsMenu.GetBackButton());
        }

        private void InitGooglePlayButton(IButton view)
        {
            GooglePlayButtonManager = new GooglePlayButtonManager(view);
            GooglePlayButtonManager.OnOpen();
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
            Destroy(_creatorsMenu.gameObject);
            GooglePlayButtonManager?.OnClose();
        }

        public void OnClose()
        {
            if(_backButton != null) _backButton.Click -= Deinit;
            GooglePlayButtonManager?.OnClose();
            button.Click -= Init;
        }

        private void OnDestroy()
        {
            OnClose();
        }
    }
}