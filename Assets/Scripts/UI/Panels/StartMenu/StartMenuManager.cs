using System;
using GameMechanics;
using GameMechanics.Helpers;
using UI.Panels.Creators;
using UI.Panels.Settings;
using UnityEngine;

namespace UI.Panels.StartMenu
{
    public class StartMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject startMenuPrefab;
        private StartMenu _startMenu;
        private StartMenuPresenter _startMenuPresenter;

        [SerializeField] private SettingsMenuManager settingsMenuManager;
        [SerializeField] private CreatorsMenuManager creatorsMenuManager;

        public event Action StartGame;

        private void Start()
        {
            _startMenu = Instantiate(startMenuPrefab, transform).GetComponent<StartMenu>();
            _startMenuPresenter = new StartMenuPresenter(_startMenu);
            _startMenuPresenter.OnOpen(PlayGame, EnableSettingsMenu, EnableCreatorsMenu, ExitHelper.Exit);
            
            settingsMenuManager.ClickReturn += EnableStartMenuManagerManager;
            creatorsMenuManager.ClickReturn += EnableStartMenuManagerManager;
        }
    
        public void EnableStartMenuManagerManager()
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
            settingsMenuManager.EnableSettingsMenu();
        }

        private void EnableCreatorsMenu()
        {
            _startMenu.gameObject.SetActive(false);
            creatorsMenuManager.EnableCreatorsMenu();
        }

        private void OnDestroy()
        {
            _startMenuPresenter.OnClose(PlayGame, EnableSettingsMenu, EnableCreatorsMenu, ExitHelper.Exit);
            settingsMenuManager.ClickReturn -= EnableStartMenuManagerManager;
            creatorsMenuManager.ClickReturn -= EnableStartMenuManagerManager;
        }
    }
}