using UI.Interfaces;
using UI.Interfaces.SwitchButton;

namespace UI.Panels.StartMenu.SettingsButton.MusicButton
{
    public class MusicButtonPresenter: IPresenter
    {
        private readonly ISwitchButtonModel _model;
        private readonly ISwitchButtonView _view;

        public MusicButtonPresenter(ISwitchButtonModel model, ISwitchButtonView view)
        {
            _model = model;
            _view = view;
        }

        public void OnOpen()
        {
            _view.Click += ChangeEnableModel;
            _model.ChangeEnable += ChangeEnableView;
        }

        private void ChangeEnableModel()
        {
            _model.Enable = !_model.Enable;
        }

        private void ChangeEnableView()
        {
            if(_model.Enable) _view.Enable();
            else _view.Disable();
        }

        public void OnClose()
        {
            _view.Click -= ChangeEnableModel;
            _model.ChangeEnable -= ChangeEnableView;
        }
    }
}