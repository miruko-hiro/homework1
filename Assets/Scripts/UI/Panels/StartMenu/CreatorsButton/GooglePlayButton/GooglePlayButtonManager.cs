using UI.Interfaces;

namespace UI.Panels.StartMenu.CreatorsButton.GooglePlayButton
{
    public class GooglePlayButtonManager: IManager
    {
        private readonly IPresenter _presenter;

        public GooglePlayButtonManager(IButton view)
        {
            _presenter = new GooglePlayButtonPresenter(new GooglePlayButtonModel(), view);
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