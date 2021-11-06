using UI.Buttons;
using UnityEngine;

namespace UI.Panels.LoserPanel
{
    public class LoserMenu : MonoBehaviour
    {
        [SerializeField] private GameObject continueButtonGameObject;
        public UIButton ContinueButton { get; private set; }
        
        [SerializeField] private GameObject exitButtonGameObject;
        public UIButton ExitButton { get; private set; }

        public void Init()
        {
            ContinueButton = continueButtonGameObject.GetComponent<UIButton>();
            ExitButton = exitButtonGameObject.GetComponent<UIButton>();
        }
    }
}
