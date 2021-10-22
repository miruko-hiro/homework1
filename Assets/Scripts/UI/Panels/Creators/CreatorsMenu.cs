using System;
using UnityEngine;

namespace UI.Panels.Creators
{
    public class CreatorsMenu : MonoBehaviour, ICreatorsMenu
    {
        public event Action ClickGooglePlayButton;
        public event Action ClickReturnButton;

        public void OnClickGooglePlay()
        {
            ClickGooglePlayButton?.Invoke();
        }

        public void OnClickReturn()
        {
            ClickReturnButton?.Invoke();
        }
    }
}