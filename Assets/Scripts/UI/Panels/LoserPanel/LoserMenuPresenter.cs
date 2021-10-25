using System;

namespace UI.Panels.LoserPanel
{
    public class LoserMenuPresenter
    {
        private readonly LoserMenu _loserMenu;

        public LoserMenuPresenter(LoserMenu loserMenu)
        {
            _loserMenu = loserMenu;
        }

        public void OnOpen(Action actionContinueButton, Action actionExitButton)
        {
            _loserMenu.ContinueButton.Click += actionContinueButton;
            _loserMenu.ExitButton.Click += actionExitButton;
        }

        public void OnClose(Action actionContinueButton, Action actionExitButton)
        {
            _loserMenu.ContinueButton.Click -= actionContinueButton;
            _loserMenu.ExitButton.Click -= actionExitButton;
        }
    }
}