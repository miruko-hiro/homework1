using System;
using GameMechanics.Player.Planet;
using UI.Buttons;
using UI.Player.PlayerHealth;
using UI.Player.PlayerMoney;
using UnityEngine;
using Zenject;

namespace UI.Player
{
    public class PlayerUIManager : MonoBehaviour
    {
        [SerializeField] private MoneyUIView moneyUIView;
        private MoneyUIPresenter _moneyUIPresenter;

        [SerializeField] private HealthUIView healthUIView;
        private HealthUIPresenter _healthUIPresenter;
        
        [SerializeField] private LvlUpButton lvlUpButton;

        private PlayerManager _playerManager;

        public event Action LvlUpButtonClick;

        [Inject]
        private void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public void Init()
        {
            _moneyUIPresenter = new MoneyUIPresenter(_playerManager.Model.Money, moneyUIView);
            _moneyUIPresenter.OnOpen();
            
            _healthUIPresenter = new HealthUIPresenter(_playerManager.Model.Health, healthUIView);
            _healthUIPresenter.OnOpen();
            
            lvlUpButton.Click +=  LvlUpClick;
        }

        private void LvlUpClick()
        {
            LvlUpButtonClick?.Invoke();
        }
        
        public void DisableElements()
        {
            moneyUIView.gameObject.SetActive(false);
            healthUIView.gameObject.SetActive(false);
            lvlUpButton.gameObject.SetActive(false);
        }

        public void EnableElements()
        {
            moneyUIView.gameObject.SetActive(true);
            healthUIView.gameObject.SetActive(true);
            lvlUpButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _moneyUIPresenter?.OnClose();
            _healthUIPresenter?.OnClose();
            lvlUpButton.Click -= LvlUpClick;
        }
    }
}