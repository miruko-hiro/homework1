using System.Collections;
using GameMechanics;
using UI.Buttons;
using UI.Panels;
using UI.Panels.GoldenMode;
using UI.Panels.StartMenu;
using UI.PlayerUI;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(UIStartMenu))]
    public class UIMechanics : MonoBehaviour
    {
        private UIStartMenu _uiStartMenu;

        [SerializeField] private GameObject goldenModeUIPrefab;
        private UIGoldenMode _uiGoldenMode;
        private Animator _animatorGoldenMode;
        
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
        private PlayerPresenter _playerPresenter;

        private bool _isFirstLoserPanel = true;
        private bool _isFirstLvlUpPanel = true;
        private static readonly int Disable = Animator.StringToHash("Disable");

        private void Start()
        {
            _mainMechanics = controller.GetComponent<MainMechanics>();
            _uiStartMenu = GetComponent<UIStartMenu>();
            GameStateHelper.Pause();
            
            StartCoroutine(StartGame());
        }

        private IEnumerator StartGame()
        {
            InitMainUIElements();
            
            while (!_mainMechanics.PlayerManager.Enable)
                yield return null;
           
            _playerPresenter = new PlayerPresenter(_mainMechanics.PlayerManager.Model);
            _playerPresenter.OnOpen(SetAmountOfMoney, SetAmountOfAddedMoney, TakeAwayOneLife, RestoreOneLife);
            
            _mainMechanics.GameOver += ShowLoserPanel;
            _mainMechanics.ChangeScoreGoldenMode += ChangeScoreGoldenMode;
            _mainMechanics.ChangeTimeGoldenMode += ChangeTimeGoldenMode;
        }

        private void ChangeScoreGoldenMode(string score)
        {
            _uiGoldenMode.Score = score;
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
            _loserPanel.ExitButton.Click += ExitHelper.Exit;
        }

        private IEnumerator InitLvlUpPanel()
        {
            _lvlUpPanel = Instantiate(lvlUpPanelPrefab, transform).GetComponent<LvlUpPanel>();
            _lvlUpPanel.gameObject.SetActive(true);
            while (!_lvlUpPanel.IsInit)
                yield return null;
            _lvlUpPanel.ChangeLvl += ChangeDamage;
            _lvlUpPanel.ComeBackButton.Click += HideLvlUpPanel;
            _lvlUpPanel.SetMoney(_mainMechanics.PlayerManager.Model.Money.Amount);
        }

        private void ChangeDamage(int damage, int index, int money)
        {
            _mainMechanics.PlayerManager.Model.Attack.Increase(damage);
            _mainMechanics.PlayerManager.Model.Money.Decrease(money);
            if (_lvlUpPanel.enabled) _lvlUpPanel.SetMoney(_mainMechanics.PlayerManager.Model.Money.Amount);
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

        private void SetAmountOfMoney(int money)
        {
            _moneyUI.AmountOfMoney = money.ToString();
        }

        private void SetAmountOfAddedMoney(int addedMoney)
        {
            _moneyUI.AmountOfAddedMoney = addedMoney.ToString();
        }

        private void TakeAwayOneLife(int health)
        {
            _playerHealthUI.TakeOneLifeAway();
        }

        private void RestoreOneLife(int health)
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
            GameStateHelper.Pause();
            if (_isFirstLvlUpPanel)
            {
                StartCoroutine(InitLvlUpPanel());
                _isFirstLvlUpPanel = false;
            }
            else
            {
                _lvlUpPanel.gameObject.SetActive(true);
                _lvlUpPanel.SetMoney(_mainMechanics.PlayerManager.Model.Money.Amount);
            }
        }

        private void HideLvlUpPanel()
        {
            _lvlUpPanel.gameObject.SetActive(false);
            GameStateHelper.Play();
        }

        private void ReStart()
        {
            _loserPanel.gameObject.SetActive(false);
            _mainMechanics.ReStart();
        }

        private void OnDestroy()
        {
            
            _lvlUpButton.Click -= ShowLvlUpPanel;

            if (!_isFirstLvlUpPanel)
            {
                _lvlUpPanel.ChangeLvl -= ChangeDamage;
                _lvlUpPanel.ComeBackButton.Click -= HideLvlUpPanel;
            }
            

            if (!_isFirstLoserPanel)
            {
                _loserPanel.ContinueButton.Click -= ReStart;
                _loserPanel.ExitButton.Click -= ExitHelper.Exit;
            }

            _playerPresenter.OnClose(SetAmountOfMoney, SetAmountOfAddedMoney, TakeAwayOneLife, RestoreOneLife);
            
            _mainMechanics.GameOver -= ShowLoserPanel;
            _mainMechanics.ChangeScoreGoldenMode -= ChangeScoreGoldenMode;
            _mainMechanics.ChangeTimeGoldenMode -= ChangeTimeGoldenMode;
        }
    }
}
