using System;

namespace UI.Panels.LvlUpPanel.Improvement
{
    public class ImprovementPresenter
    {
        public ImprovementModel Model { get; private set; }
        public ImprovementView View { get; private set; }

        public ImprovementPresenter(ImprovementModel model, ImprovementView view)
        {
            Model = model;
            View = view;
            View.Init();
        }

        public void OnOpen(Action actionChangeWhenLevelUp)
        {
            Model.ChangeEnable += ChangeEnable;
            Model.ChangeIcon += ChangeIcon;
            Model.ChangeText += ChangeText;
            Model.ChangeUpText += ChangeUpText;
            Model.ChangeMoney += ChangeMoney;
            View.LvlButton.Click += actionChangeWhenLevelUp;
        }

        private void ChangeEnable()
        {
            View.SetButtonInteractable(Model.Enable);
        }

        private void ChangeIcon()
        {
            View.SetIcon(Model.Icon);
        }

        private void ChangeText()
        {
            View.SetText(Model.Text);
        }

        private void ChangeUpText()
        {
            View.SetUpText(Model.UpText);
        }

        private void ChangeMoney()
        {
            View.SetMoney(Model.Money.ToString());
        }

        public void OnClose(Action actionChangeWhenLevelUp)
        {
            Model.ChangeEnable -= ChangeEnable;
            Model.ChangeIcon -= ChangeIcon;
            Model.ChangeText -= ChangeText;
            Model.ChangeUpText -= ChangeUpText;
            Model.ChangeMoney -= ChangeMoney;
            View.LvlButton.Click -= actionChangeWhenLevelUp;
        }
    }
}