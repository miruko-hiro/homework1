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

        [SerializeField] private GameObject loserPanelPrefab;
        private LoserPanel _loserPanel;

        [SerializeField] private GameObject controller;
        private MainMechanics _mainMechanics;

        private IEnumerator Start()
        {
            _moneyUI = Instantiate(moneyUIPrefab, transform).GetComponent<MoneyUI>();
            _playerHealthUI = Instantiate(hpPlayerUIPrefab, transform).GetComponent<PlayerHealthUI>();
            
            _loserPanel = Instantiate(loserPanelPrefab, transform).GetComponent<LoserPanel>();
            
            while (!_loserPanel.ContinueButton || !_loserPanel.ExitButton)
                yield return null;
            
            _loserPanel.ContinueButton.ClickContinue += ReStart;
            _loserPanel.ExitButton.ClickExit += Exit;
            _loserPanel.gameObject.SetActive(false);
                
            _mainMechanics = controller.GetComponent<MainMechanics>();

            
            while (!_mainMechanics.Player || !_mainMechanics.Player.Money)
                yield return null;
            
            _mainMechanics.Player.Money.ChangeAmountOfMoney += SetAmountOfMoney;
            _mainMechanics.Player.Health.HealthDecreased += TakeAwayOneLife;
            _mainMechanics.Player.Health.HealthIncreased += RestoreOneLife;
            
            _mainMechanics.GameOver += ShowLoserPanel;
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

        private void ShowLoserPanel()
        {
            _loserPanel.gameObject.SetActive(true);
        }

        private void ReStart()
        {
            _loserPanel.gameObject.SetActive(false);
            _mainMechanics.ReStart();
        }

        private void Exit()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            _mainMechanics.Player.Money.ChangeAmountOfMoney -= SetAmountOfMoney;
            _mainMechanics.Player.Health.HealthDecreased -= TakeAwayOneLife;
            _mainMechanics.Player.Health.HealthIncreased -= RestoreOneLife;

            _loserPanel.ContinueButton.ClickContinue -= ReStart;
            _loserPanel.ExitButton.ClickExit -= Exit;
            
            _mainMechanics.GameOver -= ShowLoserPanel;
        }
    }
}
