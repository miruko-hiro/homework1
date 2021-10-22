using System;
using UnityEngine;

namespace UI.Panels.Settings
{
    public class UISettingsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject settingsMenuPrefab;
        private SettingsMenu _settingsMenu;
        private SettingsMenuPresenter _settingsMenuPresenter;
        public event Action ClickReturn;
        
        public void EnableSettingsMenu()
        {
            if (_settingsMenu) return;
            _settingsMenu = Instantiate(settingsMenuPrefab, transform).GetComponent<SettingsMenu>();
            _settingsMenuPresenter = new SettingsMenuPresenter(_settingsMenu);
            _settingsMenuPresenter.OnOpen(OnSound, OnMusic, OnReturn);
        }

        private void OnSound()
        {
            Debug.Log("Click Sound Button");
        }

        private void OnMusic()
        {
            Debug.Log("Click Music Button");
        }

        private void OnReturn()
        {
            Destroy(_settingsMenu.gameObject);
            _settingsMenuPresenter.OnClose(OnSound, OnMusic, OnReturn);
            ClickReturn?.Invoke();
        }

        private void OnDestroy()
        {
            if (_settingsMenu)
                _settingsMenuPresenter.OnClose(OnSound, OnMusic, OnReturn);
        }
    }
}