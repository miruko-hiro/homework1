using UI.Buttons;
using UnityEngine;

namespace UI.Panels.StartMenu.CreatorsButton
{
    public class CreatorsMenuView : MonoBehaviour
    {
        [SerializeField] private UIButton googlePlayButton;
        [SerializeField] private UIButton backButtonView;
        public UIButton GetGooglePlayButtonView()
        {
            return googlePlayButton;
        }

        public UIButton GetBackButton()
        {
            return backButtonView;
        }
    }
}