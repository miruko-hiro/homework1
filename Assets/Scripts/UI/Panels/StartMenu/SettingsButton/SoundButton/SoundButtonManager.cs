using UI.Interfaces;
using UI.Interfaces.SwitchButton;

namespace UI.Panels.StartMenu.SettingsButton.SoundButton
{
    public class SoundButtonManager: IManager
    {
        private readonly IPresenter _presenter;
        public ISwitchButtonModel SoundButtonModel { get; }

        public SoundButtonManager(ISwitchButtonView view)
        {
            SoundButtonModel = new SoundButtonModel();
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