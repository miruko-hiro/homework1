using System;
using UnityEngine;

namespace UI.Buttons
{
    public class LvlUpButton : MonoBehaviour, IButton
    {
        [SerializeField] private GameObject notification;

        public void Notify()
        {
            notification.SetActive(true);
        }

        public event Action Click;
        public void OnClick()
        {
            notification.SetActive(false);
            Click?.Invoke();
        }
    }
}
