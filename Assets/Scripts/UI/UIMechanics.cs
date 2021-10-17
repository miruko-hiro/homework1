using System;
using System.Collections;
using GameMechanics;
using UnityEngine;

namespace UI
{
    public class UIMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject moneyUIPrefab;
        private MoneyUI _moneyUI;

        [SerializeField] private GameObject hpPlayerUIPrefab;
        private PlayerHealthUI _playerHealthUI;

        [SerializeField] private GameObject controller;
        private MainMechanics _mainMechanics;

        private IEnumerator Start()
        {
            _moneyUI = Instantiate(moneyUIPrefab, transform).GetComponent<MoneyUI>();
            _playerHealthUI = Instantiate(hpPlayerUIPrefab, transform).GetComponent<PlayerHealthUI>();
            _mainMechanics = controller.GetComponent<MainMechanics>();

            
            while (!_mainMechanics.Player || !_mainMechanics.Player.Money)
                yield return null;
            
            _mainMechanics.Player.Money.ChangeAmountOfMoney += SetAmountOfMoney;
            _mainMechanics.Player.Health.HealthDecreased += TakeAwayOneLife;
            _mainMechanics.Player.Health.HealthIncreased += RestoreOneLife;
        }

        private void SetAmountOfMoney()
        {
            _moneyUI.SetTextAmountOFMoney(_mainMechanics.Player.Money.Amount.ToString());
        }

        private void TakeAwayOneLife()
        {
            _playerHealthUI.TakeOneLifeAway();
        }
        
        public void RestoreOneLife()
        {
            _playerHealthUI.RestoreOneLife();
        }

        public void RestoreAllLife()
        {
            _playerHealthUI.RestoreAllLife();
        }

        private void OnDestroy()
        {
            _mainMechanics.Player.Money.ChangeAmountOfMoney -= SetAmountOfMoney;
            _mainMechanics.Player.Health.HealthDecreased -= TakeAwayOneLife;
            _mainMechanics.Player.Health.HealthIncreased -= RestoreOneLife;
        }
    }
}
