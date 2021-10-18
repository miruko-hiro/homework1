using System;
using UnityEngine;

namespace UI.Buttons
{
    public class UIButton: MonoBehaviour, IButton
    {
        public event Action Click;
        public void OnClick()
        {
            Click?.Invoke();
        }
    }
}