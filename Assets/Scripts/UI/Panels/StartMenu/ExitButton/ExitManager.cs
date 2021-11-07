using System;
using GameMechanics.Helpers;
using GameMechanics.Interfaces;
using UI.Buttons;
using UI.Interfaces;
using Zenject;

namespace UI.Panels.StartMenu.ExitButton
{
    public class ExitManager: IManager
    {
        private UIButton _button;
        private readonly ExitHelper _exitHelper;
        
        public event Action ExitGame;
        
        [Inject]
        private ExitManager(ExitHelper exitHelper)
        {
            _exitHelper = exitHelper;
        }
        
        public void SetView(UIButton button)
        {
            _button = button;
        }

        public void OnOpen()
        {
            _button.Click += Exit;
        }

        private void Exit()
        {
            ExitGame?.Invoke();
            _exitHelper.Exit();
        }

        public void OnClose()
        {
            _button.Click -= Exit;
        }
    }
}