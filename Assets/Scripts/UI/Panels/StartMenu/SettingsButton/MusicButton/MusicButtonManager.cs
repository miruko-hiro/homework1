using GameMechanics.Interfaces;
using GameMechanics.Sound;
using UI.Interfaces;
using UI.Interfaces.SwitchButton;
using UI.Sound;

namespace UI.Panels.StartMenu.SettingsButton.MusicButton
{
    public class MusicButtonManager: IManager
    {
        private readonly IPresenter _presenter;
        public ISwitchButtonModel MusicButtonModel { get; }

        public MusicButtonManager(MusicClaspRepository musicClaspRepository, MusicManager musicManager, ISwitchButtonView view)
        {
            MusicButtonModel = new MusicButtonModel(musicClaspRepository, musicManager);
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