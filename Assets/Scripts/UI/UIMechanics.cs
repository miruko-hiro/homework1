using System.Collections;
using GameMechanics;
using GameMechanics.Helpers;
using UI.Buttons;
using UI.Panels;
using UI.Panels.GoldenMode;
using UI.Panels.LoserPanel;
using UI.Panels.LvlUpPanel;
using UI.Panels.LvlUpPanel.Improvement;
using UI.Panels.StartMenu;
using UI.PlayerUI;
using UI.PlayerUI.PlayerCooldown;
using UI.PlayerUI.PlayerHealth;
using UI.PlayerUI.PlayerMoney;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(UIStartMenu),
        typeof(UILoserMenu),
        typeof(UILvlUpMenu))]
    public class UIMechanics : MonoBehaviour
    {
        private UIStartMenu _uiStartMenu;
        private UILoserMenu _uiLoserMenu;
        private UILvlUpMenu _uiLvlUpMenu;

        [SerializeField] private GameObject cooldownPanelPrefab;
        private SkillCooldown _skillCooldown;
        
        [SerializeField] private GameObject goldenModeUIPrefab;
        private UIGoldenMode _uiGoldenMode;
        private Animator _animatorGoldenMode;
        
        [SerializeField] private GameObject moneyUIPrefab;
        private MoneyUI _moneyUI;

        [SerializeField] private GameObject hpPlayerUIPrefab;
        private PlayerHealthUI _playerHealthUI;

        [SerializeField] private GameObject controller;
        private MainMechanics _mainMechanics;
        private PlayerPresenter _playerPresenter;
        
        [SerializeField] private GameObject lvlUpButtonPrefab;
        private LvlUpButton _lvlUpButton;
        
        [SerializeField] private GameObject _startMenuButton;

        private static readonly int Disable = Animator.StringToHash("Disable");

        private void Start()
        {
            _mainMechanics = controller.GetComponent<MainMechanics>();
            _uiLoserMenu = GetComponent<UILoserMenu>();
            _uiLvlUpMenu = GetComponent<UILvlUpMenu>();
            _uiStartMenu = GetComponent<UIStartMenu>();
            
            StartCoroutine(StartGame());

            _uiStartMenu.StartGame += EnableMainUIElements;

            _uiLoserMenu.IncludedLoserMenu += DisableMainUIElements;
            _uiLoserMenu.ReStart += ReStart;

            _uiLvlUpMenu.Init(_mainMechanics.PlayerManager.Model);
            _mainMechanics.SpaceshipMechanics.AddSpaceshipRocket += _uiLvlUpMenu.RocketInit;
            _uiLvlUpMenu.SelectSpaceship += SelectSpaceship;
            _uiLvlUpMenu.AddRocket += AddRocket;
            
            GameStateHelper.Pause();
        }

        private IEnumerator StartGame()
        {
            InitMainUIElements();
            
            while (!_mainMechanics.PlayerManager.Enable)
                yield return null;
           
            _playerPresenter = new PlayerPresenter(_mainMechanics.PlayerManager.Model);
            _playerPresenter.OnOpen(SetAmountOfMoney, 
                SetAmountOfAddedMoney, 
                _playerHealthUI.TakeOneLifeAway, 
                _playerHealthUI.RestoreOneLife);
            
            _mainMechanics.PlayerMechanics.GameOver += _uiLoserMenu.ShowLoserPanel;
            _mainMechanics.SpaceshipMechanics.RocketCooldown += StartRocketCooldown;
            _mainMechanics.ChangeScoreGoldenMode += ChangeScoreGoldenMode;
            _mainMechanics.ChangeTimeGoldenMode += ChangeTimeGoldenMode;
        }

        public void OnClickStartMenu()
        {
            GameStateHelper.Pause();
            DisableMainUIElements();
            _uiStartMenu.EnableStartMenu();
        }

        private void ChangeScoreGoldenMode(string score)
        {
            _uiGoldenMode.Score = score;
        }

        private void SelectSpaceship(ImprovementType type)
        {
            switch (type)
            {
                case ImprovementType.Two:
                    _mainMechanics.SpaceshipMechanics.SpaceshipLvlUp();
                    break;
                case ImprovementType.Three:
                    _mainMechanics.SpaceshipMechanics.AddSpaceship();
                    break;
            }
        }

        private void AddRocket()
        {
            _mainMechanics.SpaceshipMechanics.AddSpaceshipWithRocket();
        }

        private void ChangeTimeGoldenMode(string time, bool isEnable)
        {
            if (!isEnable)
            {
                if (_uiGoldenMode)
                {
                    _uiGoldenMode.Time = "0";
                    _animatorGoldenMode.SetTrigger(Disable);
                    Destroy(_uiGoldenMode.gameObject, 1f);
                }
                   
                return;
            }

            if (!_uiGoldenMode)
            {
                _uiGoldenMode = Instantiate(goldenModeUIPrefab, transform).GetComponent<UIGoldenMode>();
                _animatorGoldenMode = _uiGoldenMode.gameObject.GetComponent<Animator>();
            }
            _uiGoldenMode.Time = time;
        }

        private void StartRocketCooldown(int numericCountdown)
        {
            if(!_skillCooldown)
                _skillCooldown = Instantiate(cooldownPanelPrefab, transform).GetComponent<SkillCooldown>();
            _skillCooldown.EnableAnimation(numericCountdown);
        }

        private void InitMainUIElements()
        {
            _moneyUI = Instantiate(moneyUIPrefab, transform).GetComponent<MoneyUI>();
            _playerHealthUI = Instantiate(hpPlayerUIPrefab, transform).GetComponent<PlayerHealthUI>();
            _lvlUpButton = Instantiate(lvlUpButtonPrefab, transform).GetComponent<LvlUpButton>();
            _lvlUpButton.Click += _uiLvlUpMenu.ShowLvlUpPanel;
            DisableMainUIElements();
        }

        private void DisableMainUIElements()
        {
            _moneyUI.gameObject.SetActive(false);
            _playerHealthUI.gameObject.SetActive(false);
            _lvlUpButton.gameObject.SetActive(false);
            _startMenuButton.gameObject.SetActive(false);
        }

        private void EnableMainUIElements()
        {
            _moneyUI.gameObject.SetActive(true);
            _playerHealthUI.gameObject.SetActive(true);
            _lvlUpButton.gameObject.SetActive(true);
            _startMenuButton.gameObject.SetActive(true);
        }

        private void SetAmountOfMoney(int money)
        {
            _moneyUI.AmountOfMoney = money.ToString();
        }

        private void SetAmountOfAddedMoney(int addedMoney)
        {
            _moneyUI.AmountOfAddedMoney = addedMoney.ToString();
        }

        private void ReStart()
        {
            _uiLoserMenu.DisableMenu();
            _mainMechanics.ReStart();
        }

        private void OnDestroy()
        {
            
            _lvlUpButton.Click -=  _uiLvlUpMenu.ShowLvlUpPanel;

            _playerPresenter.OnClose(SetAmountOfMoney, 
                SetAmountOfAddedMoney, 
                _playerHealthUI.TakeOneLifeAway, 
                _playerHealthUI.RestoreOneLife);
            

            _uiStartMenu.StartGame -= EnableMainUIElements;
            _uiLoserMenu.IncludedLoserMenu -= DisableMainUIElements;
            _uiLoserMenu.ReStart -= ReStart;
            _uiLvlUpMenu.SelectSpaceship -= SelectSpaceship;
            
            _mainMechanics.SpaceshipMechanics.AddSpaceshipRocket -= _uiLvlUpMenu.RocketInit;
            _mainMechanics.PlayerMechanics.GameOver -= _uiLoserMenu.ShowLoserPanel;
            _mainMechanics.SpaceshipMechanics.RocketCooldown -= StartRocketCooldown;
            _mainMechanics.ChangeScoreGoldenMode -= ChangeScoreGoldenMode;
            _mainMechanics.ChangeTimeGoldenMode -= ChangeTimeGoldenMode;
        }
    }
}
