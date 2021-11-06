using System;
using UI.Interfaces.SwitchButton;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.StartMenu.SettingsButton.SoundButton
{
    public class SoundButtonView: MonoBehaviour, ISwitchButtonView
    {
        [SerializeField] private Image image;

        public event Action Click;

        public void Enable()
        {
            Color32 tempColor = image.color;
            tempColor.a = 255;
            image.color = tempColor;
        }

        public void Disable()
        {
            Color32 tempColor = image.color;
            tempColor.a = 100;
            image.color = tempColor;
        }
        
        public void OnClick()
        {
            Click?.Invoke();
        }
    }
}