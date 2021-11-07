using GameMechanics;
using GameMechanics.Enemy;
using GameMechanics.Helpers;
using GameMechanics.Player.Planet;
using GameMechanics.Player.Weapon;
using UI.Panels.GoldenMode;
using UI.Panels.LoserPanel;
using UI.Panels.LvlUpPanel;
using UI.Panels.LvlUpPanel.Improvement;
using UI.Panels.StartMenu;
using UI.Player;
using UI.Player.PlayerCooldown;
using UnityEngine;
using Zenject;

namespace UI
{
    public class MainUIManager : MonoBehaviour, IMainUIManager
    {
        [SerializeField] private StartMenuManager startMenuManager;
        [SerializeField] private LoserMenuManager loserMenuManager;
        [SerializeField] private LvlUpMenuManager lvlUpMenuManager;

        [Space(10)] 
        [SerializeField] private GameObject playerUIManagerPrefab;
        private PlayerUIManager _playerUIManager;

        [SerializeField] private GameObject cooldownPanelPrefab;
        private SkillCooldownView _skillCooldownView;
        private SkillCooldownPresenter _skillCooldownPresenter;
        
        [SerializeField] private GameObject goldenModeUIPrefab;
        private UIGoldenMode _uiGoldenMode;
        private Animator _animatorGoldenMode;

        private MainManager _mainManager;
        
        [SerializeField] private GameObject startMenuButton;

        private static readonly int Disable = Animator.StringToHash("Disable");

        private PlayerManager _playerManager;
        private SpaceshipManager _spaceshipManager;
        private GoldenAsteroidManager _goldenAsteroidManager;
        private PrefabFactory _prefabFactory;
        private GameStateHelper _gameStateHelper;

        [Inject]
        private void Construct(PlayerManager playerManager, 
            SpaceshipManager spaceshipManager, 
            GoldenAsteroidManager goldenAsteroidManager,
            PrefabFactory prefabFactory,
            GameStateHelper gameStateHelper,
            MainManager mainManager)
        {
            _playerManager = playerManager;
            _spaceshipManager = spaceshipManager;
            _goldenAsteroidManager = goldenAsteroidManager;
            _prefabFactory = prefabFactory;
            _gameStateHelper = gameStateHelper;
            _mainManager = mainManager;
            _mainManager.MainMechanicsCreate += StartGame;
        }

        private void StartGame()
        {
            InitMainUIElements();
            
            _playerManager.GameOver += loserMenuManager.ShowLoserPanel;
            _spaceshipManager.AddSpaceshipRocket += lvlUpMenuManager.RocketInit;
            _goldenAsteroidManager.AsteroidDied += ChangeScoreGoldenMode;
            _mainManager.ChangeTimeGoldenMode += ChangeTimeGoldenMode;

            startMenuManager.Init();
            startMenuManager.LaunchManager.LaunchGame += _mainManager.GameStart;
            startMenuManager.LaunchManager.LaunchGame += EnableMainElements;

            loserMenuManager.IncludedLoserMenu += DisableMainElements;
            loserMenuManager.ReStart += ReStart;

            lvlUpMenuManager.SelectSpaceship += SelectSpaceship;
            lvlUpMenuManager.AddRocket += AddRocket;
            lvlUpMenuManager.ContinueGame += EnableMainElements;
        }
        public void OnClickStartMenu()
        {
            _gameStateHelper.Pause();
            DisableMainElements();
            startMenuManager.EnableStartMenu();
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
            if (_skillCooldownPresenter != null) return;
            _skillCooldownView = Instantiate(cooldownPanelPrefab, transform).GetComponent<SkillCooldownView>();
            _skillCooldownPresenter = new SkillCooldownPresenter(_spaceshipManager, _skillCooldownView);
            _skillCooldownPresenter.OnOpen();
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
            _playerUIManager = _prefabFactory.Spawn(playerUIManagerPrefab, transform).GetComponent<PlayerUIManager>();
            _playerUIManager.Init();
            _playerUIManager.LvlUpButtonClick += LvlUpButtonClick;
            DisableMainElements();
        }

        private void LvlUpButtonClick()
        {
            DisableMainElements();
            lvlUpMenuManager.ShowLvlUpPanel();
        }

        private void DisableMainElements()
        {
            _playerUIManager.DisableElements();
            startMenuButton.gameObject.SetActive(false);
        }

        private void EnableMainElements()
        {
            _playerUIManager.EnableElements();
            startMenuButton.gameObject.SetActive(true);
        }

        private void ReStart()
        {
            loserMenuManager.DisableMenu();
            _mainManager.GameReStart();
        }

        private void OnDestroy()
        {

            _mainManager.MainMechanicsCreate -= StartGame;
            _playerUIManager.LvlUpButtonClick -= LvlUpButtonClick;
            startMenuManager.LaunchManager.LaunchGame -= _mainManager.GameStart;
            startMenuManager.LaunchManager.LaunchGame -= EnableMainElements;
            loserMenuManager.IncludedLoserMenu -= DisableMainElements;
            loserMenuManager.ReStart -= ReStart;
            lvlUpMenuManager.SelectSpaceship -= SelectSpaceship;
            
            _playerManager.GameOver -= loserMenuManager.ShowLoserPanel;
            _spaceshipManager.AddSpaceshipRocket -= lvlUpMenuManager.RocketInit;
            _goldenAsteroidManager.AsteroidDied -= ChangeScoreGoldenMode;
            _mainManager.ChangeTimeGoldenMode -= ChangeTimeGoldenMode;
            
            _skillCooldownPresenter?.OnClose();
        }
    }
}
