using System;
using System.Collections;
using GameMechanics;
using UI.Buttons;
using UI.Panels;
using UnityEngine;

namespace UI
{
    public class UIMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject moneyUIPrefab;
        private MoneyUI _moneyUI;

        [SerializeField] private GameObject hpPlayerUIPrefab;
        private PlayerHealthUI _playerHealthUI;

        [SerializeField] private GameObject lvlUpButtonPrefab;
        private LvlUpButton _lvlUpButton;

        [SerializeField] private GameObject loserPanelPrefab;
        private LoserPanel _loserPanel;

        [SerializeField] private GameObject lvlUpPanelPrefab;
        private LvlUpPanel _lvlUpPanel;

        [SerializeField] private GameObject controller;
        private MainMechanics _mainMechanics;

        private bool _isFirstLoserPanel = true;
        private bool _isFirstLvlUpPanel = true;

        private IEnumerator Start()
        {
            _mainMechanics = controller.GetComponent<MainMechanics>();
            InitMainUIElements();
            
            while (!_mainMechanics.Player || !_mainMechanics.Player.Money)
                yield return null;
            _mainMechanics.Player.Money.ChangeAmountOfMoney += SetAmountOfMoney;
            _mainMechanics.Player.Health.HealthDecreased += TakeAwayOneLife;
            _mainMechanics.Player.Health.HealthIncreased += RestoreOneLife;
            _mainMechanics.GameOver += ShowLoserPanel;
        }

        private void InitMainUIElements()
        {
            _moneyUI = Instantiate(moneyUIPrefab, transform).GetComponent<MoneyUI>();
            _playerHealthUI = Instantiate(hpPlayerUIPrefab, transform).GetComponent<PlayerHealthUI>();
            _lvlUpButton = Instantiate(lvlUpButtonPrefab, transform).GetComponent<LvlUpButton>();
            _lvlUpButton.Click += ShowLvlUpPanel;
        }

        private IEnumerator InitLoserPanel()
        {
            _loserPanel = Instantiate(loserPanelPrefab, transform).GetComponent<LoserPanel>();
            _loserPanel.gameObject.SetActive(true);
            while (!_loserPanel.ContinueButton || !_loserPanel.ExitButton)
                yield return null;
            _loserPanel.ContinueButton.Click += ReStart;
            _loserPanel.ExitButton.Click += Exit;
        }

        private IEnumerator InitLvlUpPanel()
        {
            _lvlUpPanel = Instantiate(lvlUpPanelPrefab, transform).GetComponent<LvlUpPanel>();
            _lvlUpPanel.gameObject.SetActive(true);
            while (!_lvlUpPanel.IsInit)
                yield return null;
            _lvlUpPanel.ChangeLvl += ChangeDamage;
            _lvlUpPanel.ComeBackButton.Click += HideLvlUpPanel;
            _lvlUpPanel.SetMoney(_mainMechanics.Player.Money.Amount);
        }

        private void ChangeDamage(int damage, int index, int money)
        {
            _mainMechanics.Player.Attack.Amount += damage;
            _mainMechanics.Player.Money.Amount -= money;
            if (_lvlUpPanel.enabled) _lvlUpPanel.SetMoney(_mainMechanics.Player.Money.Amount);
            switch (index)
            {
                case 2:
                    _mainMechanics.SpaceshipLvlUp();
                    break;
                case 3:
                    _mainMechanics.AddSpaceship();
                    break;
            }
        }

        private void SetAmountOfMoney()
        {
            _moneyUI.SetTextAmountOFMoney(_mainMechanics.Player.Money.Amount.ToString());
        }

        private void TakeAwayOneLife()
        {
            _playerHealthUI.TakeOneLifeAway();
        }

        private void RestoreOneLife()
        {
            _playerHealthUI.RestoreOneLife();
        }

        private void RestoreAllLife()
        {
            _playerHealthUI.RestoreAllLife();
        }

        private void ShowLoserPanel()
        {
            if (_isFirstLoserPanel)
            {
                StartCoroutine(InitLoserPanel());
                _isFirstLoserPanel = false;
            }
            else
            {
                _loserPanel.gameObject.SetActive(true);
            }
        }

        private void ShowLvlUpPanel()
        {
            _mainMechanics.Pause();
            if (_isFirstLvlUpPanel)
            {
                StartCoroutine(InitLvlUpPanel());
                _isFirstLvlUpPanel = false;
            }
            else
            {
                _lvlUpPanel.gameObject.SetActive(true);
                _lvlUpPanel.SetMoney(_mainMechanics.Player.Money.Amount);
            }
        }

        private void HideLvlUpPanel()
        {
            _lvlUpPanel.gameObject.SetActive(false);
            _mainMechanics.Play();
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
            _lvlUpButton.Click -= ShowLvlUpPanel;

            if (!_isFirstLvlUpPanel)
            {
                _lvlUpPanel.ChangeLvl -= ChangeDamage;
                _lvlUpPanel.ComeBackButton.Click -= HideLvlUpPanel;
            }
            
            _mainMechanics.Player.Money.ChangeAmountOfMoney -= SetAmountOfMoney;
            _mainMechanics.Player.Health.HealthDecreased -= TakeAwayOneLife;
            _mainMechanics.Player.Health.HealthIncreased -= RestoreOneLife;

            if (!_isFirstLoserPanel)
            {
                _loserPanel.ContinueButton.Click -= ReStart;
                _loserPanel.ExitButton.Click -= Exit;
            }

            _mainMechanics.GameOver -= ShowLoserPanel;
        }
    }
}
