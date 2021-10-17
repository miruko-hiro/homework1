using System;
using UnityEngine;

namespace UI
{
    public class ExitButton : MonoBehaviour
    {
        public event Action ClickExit;
        
        public void OnClickExit()
        {
            ClickExit?.Invoke();
        }
    }
}
