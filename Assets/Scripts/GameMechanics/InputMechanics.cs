using System;
using UnityEngine;

namespace GameMechanics
{
    public class InputMechanics : MonoBehaviour
    {
        public event Action OnTouch;
        public event Action OnClick;
        
        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                OnTouch?.Invoke();
            } else if (Input.GetMouseButtonDown(0))
            {
                OnClick?.Invoke();
            }
        }
    }
}
