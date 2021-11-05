using System;
using System.Collections;
using GameMechanics;
using GameMechanics.Helpers;
using UnityEngine;

namespace UI.Panels.LoserPanel
{
    public class LoserMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject loserPanelPrefab;
        private LoserMenu _loserMenu;
        private bool _isFirstLoserPanel = true;
        private LoserMenuPresenter _loserMenuPresenter;
        public event Action IncludedLoserMenu;
        public event Action ReStart;
        
        private IEnumerator InitLoserPanel()
        {
            _loserMenu = Instantiate(loserPanelPrefab, transform).GetComponent<LoserMenu>();
            _loserMenu.gameObject.SetActive(true);
            while (!_loserMenu.ContinueButton || !_loserMenu.ExitButton)
                yield return null;
            _loserMenuPresenter = new LoserMenuPresenter(_loserMenu);
            _loserMenuPresenter.OnOpen(ReStart, ExitHelper.Exit);
            IncludedLoserMenu?.Invoke();
        }
        
        public void ShowLoserPanel()
        {
            if (_isFirstLoserPanel)
            {
                StartCoroutine(InitLoserPanel());
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

        private void OnDestroy()
        {
            if (!_isFirstLoserPanel)
            {
                _loserMenuPresenter.OnClose(ReStart, ExitHelper.Exit);
            }
        }
    }
}