using System;
using UnityEngine;

namespace UI.Panels.Creators
{
    public class CreatorsMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject creatorsMenuPrefab;
        private CreatorsMenu _creatorsMenu;
        private CreatorsMenuPresenter _creatorsMenuPresenter;
        public event Action ClickReturn;
        private const string DeveloperURL = "https://play.google.com/store/apps/developer?id=MRKHR";

        public void EnableCreatorsMenu()
        {
            if (_creatorsMenu) return;
            _creatorsMenu = Instantiate(creatorsMenuPrefab, transform).GetComponent<CreatorsMenu>();
            _creatorsMenuPresenter = new CreatorsMenuPresenter(_creatorsMenu);
            _creatorsMenuPresenter.OnOpen(OnGooglePlay, OnReturn);
        }

        private void OnGooglePlay()
        {
            Application.OpenURL(DeveloperURL);
        }
        
        private void OnReturn()
        {
            Destroy(_creatorsMenu.gameObject);
            _creatorsMenuPresenter.OnClose(OnGooglePlay, OnReturn);
            ClickReturn?.Invoke();
        }

        private void OnDestroy()
        {
            if (_creatorsMenu) 
                _creatorsMenuPresenter.OnClose(OnGooglePlay, OnReturn);
        }
    }
}