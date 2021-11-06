using UI.Buttons;
using UI.Panels.StartMenu.SettingsButton.MusicButton;
using UI.Panels.StartMenu.SettingsButton.SoundButton;
using UnityEngine;

namespace UI.Panels.StartMenu.SettingsButton
{
    public class SettingsMenuView : MonoBehaviour
    {
        [SerializeField] private SoundButtonView soundButtonView;
        [SerializeField] private MusicButtonView musicButtonView;
        [SerializeField] private UIButton backButtonView;

        public SoundButtonView GetSoundButtonView()
        {
            return soundButtonView;
        }

        public MusicButtonView GetMusicButtonView()
        {
            return musicButtonView;
        }

        public UIButton GetBackButton()
        {
            return backButtonView;
        }
    }
}