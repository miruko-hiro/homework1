using System;
using UnityEngine;

namespace UI
{
    public class ContinueButton : MonoBehaviour
    {
        public event Action ClickContinue;
        
        public void OnClickContinue()
        {
            ClickContinue?.Invoke();
        }
    }
}
