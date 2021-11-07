using GameMechanics.Interfaces;
using GameMechanics.Sound;
using UI.Interfaces;
using UI.Interfaces.SwitchButton;

namespace UI.Panels.StartMenu.SettingsButton.SoundButton
{
    public class SoundButtonManager: IManager
    {
        private readonly IPresenter _presenter;
        public ISwitchButtonModel SoundButtonModel { get; }

        public SoundButtonManager(SoundManager soundManager, ISwitchButtonView view)
        {
            SoundButtonModel = new SoundButtonModel(soundManager);
            _presenter = new SoundButtonPresenter(SoundButtonModel, view);
        }

        public void OnOpen()
        {
            _presenter.OnOpen();
        }

        public void OnClose()
        {
            _presenter.OnClose();
        }
    }
}