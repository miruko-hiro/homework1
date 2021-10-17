using System;
using UnityEngine;

namespace UI
{
    public class ComeBack : MonoBehaviour
    {
        public event Action ClickComeBack;
        
        public void OnClickComeBack()
        {
            ClickComeBack?.Invoke();
        }
    }
}
