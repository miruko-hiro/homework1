using UI.Interfaces;
using UI.Interfaces.SwitchButton;

namespace UI.Panels.StartMenu.SettingsButton.MusicButton
{
    public class MusicButtonManager: IManager
    {
        private readonly IPresenter _presenter;
        public ISwitchButtonModel MusicButtonModel { get; }

        public MusicButtonManager(ISwitchButtonView view)
        {
            MusicButtonModel = new MusicButtonModel();
            _presenter = new MusicButtonPresenter(MusicButtonModel, view);
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