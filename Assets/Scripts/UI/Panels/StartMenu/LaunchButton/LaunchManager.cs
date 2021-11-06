using System;
using GameMechanics.Helpers;
using UI.Interfaces;
using Zenject;

namespace UI.Panels.StartMenu.LaunchButton
{
    public class LaunchManager: IManager
    {
        private IButton _button;
        private readonly GameStateHelper _gameStateHelper;
        
        public event Action LaunchGame;

        [Inject]
        public LaunchManager(GameStateHelper gameStateHelper)
        {
            _gameStateHelper = gameStateHelper;
        }

        public void SetView(IButton button)
        {
            _button = button;
        }
        
        public void OnOpen()
        {
            _button.Click += Launch;
        }

        private void Launch()
        {
            LaunchGame?.Invoke();
            _gameStateHelper.Play();
        }

        public void OnClose()
        {
            _button.Click -= Launch;
        }
    }
}