using System;
using UI.Interfaces;
using UI.Interfaces.SwitchButton;
using UI.Panels.StartMenu.SettingsButton.MusicButton;
using UI.Panels.StartMenu.SettingsButton.SoundButton;
using UnityEngine;

namespace UI.Panels.StartMenu.SettingsButton
{
    public class SettingsMenuManager: IManager
    {
        private readonly ISpawner<SettingsMenuView> _spawner;
        private readonly IButton _button;
        private SettingsMenuView _settingsMenu;
        private IButton _backButton;
        
        public SoundButtonManager SoundButtonManager { get; private set; }
        public MusicButtonManager MusicButtonManager { get; private set; }

        public event Action ComeBack;
        public SettingsMenuManager(ISpawner<SettingsMenuView> spawner, IButton button)
        {
            _spawner = spawner;
            _button = button;
        }

        public void OnOpen()
        {
            _button.Click += Init;
        }

        private void Init()
        {
            if (_settingsMenu != null) return;
            
            _settingsMenu = _spawner.Spawn();
            InitSoundButton(_settingsMenu.GetSoundButtonView());
            InitMusicButton(_settingsMenu.GetMusicButtonView());
            InitBackButton(_settingsMenu.GetBackButton());
        }

        private void InitSoundButton(ISwitchButtonView view)
        {
            SoundButtonManager = new SoundButtonManager(view);
            SoundButtonManager.OnOpen();
        }

        private void InitMusicButton(ISwitchButtonView view)
        {
            MusicButtonManager = new MusicButtonManager(view);
            MusicButtonManager.OnOpen();
        }

        private void InitBackButton(IButton view)
        {
            _backButton = view;
            _backButton.Click += Deinit;
        }

        private void Deinit()
        {
            ComeBack?.Invoke();
            UnityEngine.Object.Destroy(_settingsMenu.gameObject);
            MusicButtonManager?.OnClose();
            SoundButtonManager?.OnClose();
        }

        public void OnClose()
        {
            if(_backButton != null) _backButton.Click -= Deinit;
            MusicButtonManager?.OnClose();
            SoundButtonManager?.OnClose();
            _button.Click -= Init;
        }
    }
}