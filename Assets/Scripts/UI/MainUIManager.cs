using GameMechanics;
using GameMechanics.Enemy;
using GameMechanics.Helpers;
using GameMechanics.Player.Planet;
using GameMechanics.Player.Weapon;
using UI.Buttons;
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
using UnityEngine.Serialization;
using Zenject;

namespace UI
{
    public class MainUIManager : MonoBehaviour
    {
        [SerializeField] private StartMenuManager startMenuManager;
        [SerializeField] private LoserMenuManager loserMenuManager;
        [SerializeField] private LvlUpMenuManager lvlUpMenuManager;
        
        [Space(10)]

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
        private MainManager _mainManager;
        private PlayerPresenter _playerPresenter;
        
        [SerializeField] private GameObject lvlUpButtonPrefab;
        private LvlUpButton _lvlUpButton;
        
        [SerializeField] private GameObject startMenuButton;

        private static readonly int Disable = Animator.StringToHash("Disable");

        private PlayerManager _playerManager;
        private SpaceshipManager _spaceshipManager;
        private GoldenAsteroidManager _goldenAsteroidManager;

        private void Start()
        {
            _mainManager = controller.GetComponent<MainManager>();
            _mainManager.MainMechanicsCreate += StartGame;
        }

        [Inject]
        private void Construct(PlayerManager playerManager, 
            SpaceshipManager spaceshipManager, 
            GoldenAsteroidManager goldenAsteroidManager)
        {
            _playerManager = playerManager;
            _spaceshipManager = spaceshipManager;
            _goldenAsteroidManager = goldenAsteroidManager;
        }

        private void StartGame()
        {
            InitMainUIElements();
            
            _playerPresenter = new PlayerPresenter(_playerManager.Model, this);
            _playerPresenter.OnOpen(SetAmountOfMoney, 
                SetAmountOfAddedMoney, 
                _playerHealthUI.TakeOneLifeAway, 
                _playerHealthUI.RestoreOneLife);
            
            _playerManager.GameOver += loserMenuManager.ShowLoserPanel;
            _spaceshipManager.RocketCooldown += StartRocketCooldown;
            _spaceshipManager.AddSpaceshipRocket += lvlUpMenuManager.RocketInit;
            _goldenAsteroidManager.AsteroidDied += ChangeScoreGoldenMode;
            _mainManager.ChangeTimeGoldenMode += ChangeTimeGoldenMode;
            
            startMenuManager.StartGame += EnableMainElements;

            loserMenuManager.IncludedLoserMenu += DisableMainElements;
            loserMenuManager.ReStart += ReStart;

            lvlUpMenuManager.SelectSpaceship += SelectSpaceship;
            lvlUpMenuManager.AddRocket += AddRocket;
            lvlUpMenuManager.ContinueGame += EnableMainElements;
        }

        public void TakeOneLifeAway(int health)
        {
            _playerHealthUI.TakeOneLifeAway(health);
        }

        public void RestoreOneLife(int health)
        {
            _playerHealthUI.RestoreOneLife(health);
        }

        public void OnClickStartMenu()
        {
            GameStateHelper.Pause();
            DisableMainElements();
            startMenuManager.EnableStartMenuManagerManager();
        }

        private void ChangeScoreGoldenMode(int score)
        {
            _uiGoldenMode.Score = score.ToString();
        }

        private void SelectSpaceship(ImprovementType type)
        {
            switch (type)
            {
                case ImprovementType.Two:
                    _spaceshipManager.SpaceshipLvlUp();
                    break;
                case ImprovementType.Three:
                    _spaceshipManager.AddSpaceship();
                    break;
            }
        }

        private void AddRocket()
        {
            _spaceshipManager.AddSpaceshipWithRocket();
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
            _lvlUpButton.Click += LvlUpButtonClick;
            DisableMainElements();
        }

        private void LvlUpButtonClick()
        {
            DisableMainElements();
            lvlUpMenuManager.ShowLvlUpPanel();
        }

        private void DisableMainElements()
        {
            _moneyUI.gameObject.SetActive(false);
            _playerHealthUI.gameObject.SetActive(false);
            _lvlUpButton.gameObject.SetActive(false);
            startMenuButton.gameObject.SetActive(false);
        }

        private void EnableMainElements()
        {
            _moneyUI.gameObject.SetActive(true);
            _playerHealthUI.gameObject.SetActive(true);
            _lvlUpButton.gameObject.SetActive(true);
            startMenuButton.gameObject.SetActive(true);
        }

        public void SetAmountOfMoney(int money)
        {
            _moneyUI.AmountOfMoney = money.ToString();
        }

        public void SetAmountOfAddedMoney(int addedMoney)
        {
            _moneyUI.AmountOfAddedMoney = addedMoney.ToString();
        }

        private void ReStart()
        {
            loserMenuManager.DisableMenu();
            _mainManager.ReStart();
        }

        private void OnDestroy()
        {
            
            _lvlUpButton.Click -=  LvlUpButtonClick;

            _playerPresenter.OnClose(SetAmountOfMoney, 
                SetAmountOfAddedMoney, 
                _playerHealthUI.TakeOneLifeAway, 
                _playerHealthUI.RestoreOneLife);
            

            _mainManager.MainMechanicsCreate -= StartGame;
            startMenuManager.StartGame -= EnableMainElements;
            loserMenuManager.IncludedLoserMenu -= DisableMainElements;
            loserMenuManager.ReStart -= ReStart;
            lvlUpMenuManager.SelectSpaceship -= SelectSpaceship;
            
            _playerManager.GameOver -= loserMenuManager.ShowLoserPanel;
            _spaceshipManager.AddSpaceshipRocket -= lvlUpMenuManager.RocketInit;
            _spaceshipManager.RocketCooldown -= StartRocketCooldown;
            _goldenAsteroidManager.AsteroidDied -= ChangeScoreGoldenMode;
            _mainManager.ChangeTimeGoldenMode -= ChangeTimeGoldenMode;
        }
    }
}
