using GameMechanics.Behaviors;

namespace UI.Player.PlayerMoney
{
    public class MoneyUIPresenter
    {
        private readonly Money _model;
        private readonly IMoneyUIView _moneyUIView;

        public MoneyUIPresenter(Money model, IMoneyUIView moneyUIView)
        {
            _model = model;
            _moneyUIView = moneyUIView;
        }

        public void OnOpen()
        {
            _model.ChangeAmount += ChangeAmountOfMoney;
            _model.Increased += ChangeAmountOfAddedMoney;
        }

        private void ChangeAmountOfMoney(int money)
        {
            _moneyUIView.ChangeAmountOfMoney(money.ToString());
        }

        private void ChangeAmountOfAddedMoney(int money)
        {
            _moneyUIView.ChangeAmountOfAddedMoney(money.ToString());
        }

        public void OnClose()
        {
            _model.ChangeAmount -= ChangeAmountOfMoney;
            _model.Increased -= ChangeAmountOfAddedMoney;
        }
    }
}