using System;
using UnityEngine;

namespace UI
{
    public class LoserPanel : MonoBehaviour
    {
        [SerializeField] private GameObject continueButtonGameObject;
        public ContinueButton ContinueButton { get; private set; }
        
        [SerializeField] private GameObject exitButtonGameObject;
        public ExitButton ExitButton { get; private set; }

        private void Start()
        {
            ContinueButton = continueButtonGameObject.GetComponent<ContinueButton>();
            ExitButton = exitButtonGameObject.GetComponent<ExitButton>();
        }
    }
}
