using System;
using UnityEngine;

namespace UI.Panels.StartMenu
{
    public class StartMenu : MonoBehaviour, IStartMenu
    {
        public event Action ClickPlayButton;
        public event Action ClickSettingsButton;
        public event Action ClickCreatorsButton;
        public event Action ClickExitButton;

        public void OnClickPlay()
        {
            ClickPlayButton?.Invoke();
        }
        
        public void OnClickSettings()
        {
            ClickSettingsButton?.Invoke();
        }
        
        public void OnClickCreators()
        {
            ClickCreatorsButton?.Invoke();
        }
        
        public void OnClickExit()
        {
            ClickExitButton?.Invoke();
        }
    }
}
