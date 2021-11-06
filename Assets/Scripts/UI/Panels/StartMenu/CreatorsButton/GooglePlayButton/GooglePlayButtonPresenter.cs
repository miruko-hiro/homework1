using UI.Interfaces;
using UnityEngine;

namespace UI.Panels.StartMenu.CreatorsButton.GooglePlayButton
{
    public class GooglePlayButtonPresenter: IPresenter
    {
        private readonly GooglePlayButtonModel _model;
        private readonly IButton _view;

        public GooglePlayButtonPresenter(GooglePlayButtonModel model, IButton view)
        {
            _model = model;
            _view = view;
        }

        public void OnOpen()
        {
            _view.Click += OnClickButton;
        }

        private void OnClickButton()
        {
            Application.OpenURL(_model.DeveloperURL);
        }

        public void OnClose()
        {
            _view.Click -= OnClickButton;
        }
    }
}