using GameMechanics.Helpers;
using UI.Buttons;
using UI.Panels.StartMenu.CreatorsButton;
using UI.Panels.StartMenu.ExitButton;
using UI.Panels.StartMenu.LaunchButton;
using UI.Panels.StartMenu.SettingsButton;
using UnityEngine;
using Zenject;

namespace UI.Panels.StartMenu
{
    public class StartMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject startMenu;
        [Space(10)] 
        [SerializeField] private UIButton launchButton;
        [SerializeField] private UIButton exitButton;
        [Space(10)] 
        [SerializeField] private UIButton settingsButton;
        [SerializeField] private SettingsMenuSpawner settingsSpawner;
        [Space(10)] 
        [SerializeField] private UIButton creatorsButton;
        [SerializeField] private CreatorsMenuSpawner creatorsSpawner;

        private InjectionObjectFactory _factory;
        
        public LaunchManager LaunchManager { get; private set; }
        public SettingsMenuManager SettingsMenuManager { get; private set; }
        public CreatorsMenuManager CreatorsMenuManager { get; private set; }
        public ExitManager ExitManager { get; private set; }

        [Inject]
        private void Construct(InjectionObjectFactory factory)
        {
            _factory = factory;
        }
        public void Init()
        {
            InitLaunchButton();
            InitSettingsButton();
            InitCreatorsButton();
            InitExitButton();
        }

        private void InitLaunchButton()
        {
            LaunchManager = _factory.Create<LaunchManager>();
            LaunchManager.SetView(launchButton);
            LaunchManager.LaunchGame += DisableStartMenu;
            LaunchManager.OnOpen();
        }

        private void InitSettingsButton()
        {
            SettingsMenuManager = new SettingsMenuManager(settingsSpawner, settingsButton);
            SettingsMenuManager.ComeBack += EnableStartMenu;
            SettingsMenuManager.OnOpen();
        }

        private void InitCreatorsButton()
        {
            CreatorsMenuManager = new CreatorsMenuManager(creatorsSpawner, creatorsButton);
            CreatorsMenuManager.ComeBack += EnableStartMenu;
            CreatorsMenuManager.OnOpen();
        }

        private void InitExitButton()
        {
            ExitManager = _factory.Create<ExitManager>();
            ExitManager.SetView(exitButton);
            ExitManager.OnOpen();
        }

        private void DisableStartMenu()
        {
            startMenu.SetActive(false);
        }

        public void EnableStartMenu()
        {
            startMenu.SetActive(true);
        }

        private void OnDestroy()
        {
            ExitManager?.OnClose();
            CreatorsMenuManager?.OnClose();
            SettingsMenuManager?.OnClose();

            if (LaunchManager == null) return;
            LaunchManager.LaunchGame -= DisableStartMenu;
            LaunchManager.OnClose();
        }
    }
}