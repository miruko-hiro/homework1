using UI.Interfaces;
using UI.Interfaces.SwitchButton;

namespace UI.Panels.StartMenu.SettingsButton.SoundButton
{
    public class SoundButtonPresenter: IPresenter
    {
        private readonly ISwitchButtonModel _model;
        private readonly ISwitchButtonView _view;

        public SoundButtonPresenter(ISwitchButtonModel model, ISwitchButtonView view)
        {
            _model = model;
            _view = view;
        }

        public void OnOpen()
        {
            ChangeEnableView();
            _view.Click += ChangeEnableModel;
            _model.ChangeEnabled += ChangeEnableView;
        }

        private void ChangeEnableModel()
        {
            _model.Enabled = !_model.Enabled;
        }

        private void ChangeEnableView()
        {
            if(_model.Enabled) _view.Enable();
            else _view.Disable();
        }

        public void OnClose()
        {
            _view.Click -= ChangeEnableModel;
            _model.ChangeEnabled -= ChangeEnableView;
        }
    }
}