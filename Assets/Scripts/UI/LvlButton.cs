using System;
using UnityEngine;

namespace UI
{
    public class LvlButton : MonoBehaviour
    {
        public event Action ClickButton;
        
        public void OnClickButton()
        {
            ClickButton?.Invoke();
        }
    }
}
