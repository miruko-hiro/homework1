using UI.Buttons;
using UnityEngine;

namespace UI.Panels
{
    public class LoserPanel : MonoBehaviour
    {
        [SerializeField] private GameObject continueButtonGameObject;
        public UIButton ContinueButton { get; private set; }
        
        [SerializeField] private GameObject exitButtonGameObject;
        public UIButton ExitButton { get; private set; }

        private void Start()
        {
            ContinueButton = continueButtonGameObject.GetComponent<UIButton>();
            ExitButton = exitButtonGameObject.GetComponent<UIButton>();
        }
    }
}
