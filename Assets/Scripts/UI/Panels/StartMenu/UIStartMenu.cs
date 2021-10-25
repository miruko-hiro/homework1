using System;
using GameMechanics;
using GameMechanics.Helpers;
using UI.Panels.Creators;
using UI.Panels.Settings;
using UnityEngine;

namespace UI.Panels.StartMenu
{
    [RequireComponent(typeof(UISettingsMenu),
        typeof(UICreatorsMenu))]
    public class UIStartMenu : MonoBehaviour
    {
        [SerializeField] private GameObject startMenuPrefab;
        private StartMenu _startMenu;
        private StartMenuPresenter _startMenuPresenter;

        private UISettingsMenu _uiSettingsMenu;
        private UICreatorsMenu _uiCreatorsMenu;

        public event Action StartGame;

        private void Start()
        {
            _startMenu = Instantiate(startMenuPrefab, transform).GetComponent<StartMenu>();
            _startMenuPresenter = new StartMenuPresenter(_startMenu);
            _startMenuPresenter.OnOpen(PlayGame, EnableSettingsMenu, EnableCreatorsMenu, ExitHelper.Exit);
            
            _uiSettingsMenu = GetComponent<UISettingsMenu>();
            _uiSettingsMenu.ClickReturn += EnableStartMenu;

            _uiCreatorsMenu = GetComponent<UICreatorsMenu>();
            _uiCreatorsMenu.ClickReturn += EnableStartMenu;
        }

        public void EnableStartMenu()
        {
            _startMenu.gameObject.SetActive(true);
        }

        private void PlayGame()
        {
            StartGame?.Invoke();
            _startMenu.gameObject.SetActive(false);
            GameStateHelper.Play();
        }
        
        private void EnableSettingsMenu()
        {
            _startMenu.gameObject.SetActive(false);
            _uiSettingsMenu.EnableSettingsMenu();
        }

        private void EnableCreatorsMenu()
        {
            _startMenu.gameObject.SetActive(false);
            _uiCreatorsMenu.EnableCreatorsMenu();
        }

        private void OnDestroy()
        {
            _startMenuPresenter.OnClose(PlayGame, EnableSettingsMenu, EnableCreatorsMenu, ExitHelper.Exit);
            _uiSettingsMenu.ClickReturn -= EnableStartMenu;
            _uiCreatorsMenu.ClickReturn -= EnableStartMenu;
        }
    }
}