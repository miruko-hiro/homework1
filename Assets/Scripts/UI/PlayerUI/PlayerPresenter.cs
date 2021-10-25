using System;
using GameMechanics.Player.Planet;

namespace UI.PlayerUI
{
    public class PlayerPresenter
    {
        private readonly PlayerModel _playerModel;

        public PlayerPresenter(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }

        public void OnOpen(Action<int> actionChangeAmountOfMoney, Action<int> actionIncreasedMoney, Action<int> actionHealthDecreased, Action<int> actionHealthIncreased)
        {
            _playerModel.Money.ChangeAmount += actionChangeAmountOfMoney;
            _playerModel.Money.Increased += actionIncreasedMoney;
            _playerModel.Health.Decreased += actionHealthDecreased;
            _playerModel.Health.Increased += actionHealthIncreased;
        }

        public void OnClose(Action<int> actionChangeAmountOfMoney, Action<int> actionIncreasedMoney, Action<int> actionHealthDecreased, Action<int> actionHealthIncreased)
        {
            _playerModel.Money.ChangeAmount -= actionChangeAmountOfMoney;
            _playerModel.Money.Increased -= actionIncreasedMoney;
            _playerModel.Health.Decreased += actionHealthDecreased;
            _playerModel.Health.Increased += actionHealthIncreased;
        }
        
    }
}