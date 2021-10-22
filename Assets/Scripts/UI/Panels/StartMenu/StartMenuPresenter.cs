using System;

namespace UI.Panels.StartMenu
{
    public class StartMenuPresenter
    {
        private readonly IStartMenu _startMenu;

        public StartMenuPresenter(IStartMenu startMenu)
        {
            _startMenu = startMenu;
        }
        
        public void OnOpen(Action actionClickPlay, 
            Action actionClickSettings, 
            Action actionClickCreators, 
            Action actionClickExit)
        {
            _startMenu.ClickPlayButton += actionClickPlay;
            _startMenu.ClickSettingsButton += actionClickSettings;
            _startMenu.ClickCreatorsButton += actionClickCreators;
            _startMenu.ClickExitButton += actionClickExit;
        }

        public void OnClose(Action actionClickPlay, 
            Action actionClickSettings, 
            Action actionClickCreators, 
            Action actionClickExit)
        {
            _startMenu.ClickPlayButton -= actionClickPlay;
            _startMenu.ClickSettingsButton -= actionClickSettings;
            _startMenu.ClickCreatorsButton -= actionClickCreators;
            _startMenu.ClickExitButton -= actionClickExit;
        }
    }
}