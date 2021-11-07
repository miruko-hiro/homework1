using System;
using System.Collections;
using GameMechanics;
using GameMechanics.Helpers;
using GameMechanics.Interfaces;
using GameMechanics.Player.Planet;
using UI.Interfaces;
using UnityEngine;
using Zenject;

namespace UI.Panels.LoserPanel
{
    public class LoserMenuManager : MonoBehaviour, IManager
    {
        [SerializeField] private GameObject loserPanelPrefab;
        private LoserMenu _loserMenu;
        private bool _isFirstLoserPanel = true;
        private ExitHelper _exitHelper;
        public event Action IncludedLoserMenu;
        public event Action ReStart;
        
        [Inject]
        private void Construct(ExitHelper exitHelper)
        {
            _exitHelper = exitHelper;
        }
        private void InitLoserPanel()
        {
            _loserMenu = Instantiate(loserPanelPrefab, transform).GetComponent<LoserMenu>();
            _loserMenu.gameObject.SetActive(true);
            _loserMenu.Init();
            OnOpen();
            IncludedLoserMenu?.Invoke();
        }
        
        public void ShowLoserPanel()
        {
            if (_isFirstLoserPanel)
            {
                InitLoserPanel();
                _isFirstLoserPanel = false;
            }
            else
            {
                _loserMenu.gameObject.SetActive(true);
            }
        }

        public void DisableMenu()
        {
            _loserMenu.gameObject.SetActive(false);
        }

        public void OnOpen()
        {
            _loserMenu.ContinueButton.Click += ReStart;
            _loserMenu.ExitButton.Click += _exitHelper.Exit;
        }

        public void OnClose()
        {
            _loserMenu.ContinueButton.Click -= ReStart;
            _loserMenu.ExitButton.Click -= _exitHelper.Exit;
        }

        private void OnDestroy()
        {
            if (!_isFirstLoserPanel)
            {
                OnClose();
            }
        }
    }
}