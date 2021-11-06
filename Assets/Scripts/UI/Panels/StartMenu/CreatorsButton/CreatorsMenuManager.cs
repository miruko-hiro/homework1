using System;
using UI.Interfaces;
using UI.Panels.StartMenu.CreatorsButton.GooglePlayButton;

namespace UI.Panels.StartMenu.CreatorsButton
{
    public class CreatorsMenuManager: IManager
    {
        private readonly ISpawner<CreatorsMenuView> _spawner;
        private readonly IButton _button;
        private CreatorsMenuView _creatorsMenu;
        private IButton _backButton;
         
        public GooglePlayButtonManager GooglePlayButtonManager { get; private set; }

        public event Action ComeBack;
        public CreatorsMenuManager(ISpawner<CreatorsMenuView> spawner, IButton button)
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
            if (_creatorsMenu != null) return;
            
            _creatorsMenu = _spawner.Spawn();
            InitGooglePlayButton(_creatorsMenu.GetGooglePlayButtonView());
            InitBackButton(_creatorsMenu.GetBackButton());
        }

        private void InitGooglePlayButton(IButton view)
        {
            GooglePlayButtonManager = new GooglePlayButtonManager(view);
            GooglePlayButtonManager.OnOpen();
        }

        private void InitBackButton(IButton view)
        {
            _backButton = view;
            _backButton.Click += Deinit;
        }

        private void Deinit()
        {
            ComeBack?.Invoke();
            UnityEngine.Object.Destroy(_creatorsMenu.gameObject);
            GooglePlayButtonManager?.OnClose();
        }

        public void OnClose()
        {
            if(_backButton != null) _backButton.Click -= Deinit;
            GooglePlayButtonManager?.OnClose();
            _button.Click -= Init;
        }
    }
}