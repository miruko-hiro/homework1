using System;

namespace UI.Panels.StartMenu
{
    public interface IStartMenu
    {
        public event Action ClickPlayButton;
        public event Action ClickSettingsButton;
        public event Action ClickCreatorsButton;
        public event Action ClickExitButton;
        public void OnClickPlay();
        public void OnClickSettings();
        public void OnClickCreators();
        public void OnClickExit();
    }
}