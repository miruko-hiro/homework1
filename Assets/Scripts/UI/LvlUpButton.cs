using System;
using UnityEngine;

namespace UI
{
    public class LvlUpButton : MonoBehaviour
    {
        [SerializeField] private GameObject notification;
        
        public event Action ClickLvlUp;

        public void OnClickLvlUp()
        {
            notification.SetActive(false);
            ClickLvlUp?.Invoke();
        }

        public void Notify()
        {
            notification.SetActive(true);
        }
    }
}
