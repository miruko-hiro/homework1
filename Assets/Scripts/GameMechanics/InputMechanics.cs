using System;
using UnityEngine;

namespace GameMechanics
{
    public class InputMechanics : MonoBehaviour
    {
        public event Action OnTouch;
        public event Action OffTouch;
        public event Action OnClick;
        public event Action OffClick;
        
        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                OffTouch?.Invoke();
            }
            
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                OnTouch?.Invoke();
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnClick?.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OffClick?.Invoke();
            }
        }
    }
}
