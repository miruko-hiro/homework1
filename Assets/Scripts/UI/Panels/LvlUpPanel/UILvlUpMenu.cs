using System;
using System.Collections;
using GameMechanics;
using GameMechanics.Helpers;
using GameMechanics.Player.Planet;
using UnityEngine;

namespace UI.Panels.LvlUpPanel
{
    public class UILvlUpMenu : MonoBehaviour
    {
        [SerializeField] private GameObject lvlUpPanelPrefab;
        private LvlUpMenu _lvlUpMenu;
        private LvlUpMenuPresenter _lvlUpMenuPresenter;
        private PlayerModel _model;
        private bool _isFirstLvlUpPanel = true;
        private bool _isAddRocket;

        public event Action<int> SelectSpaceship;
        public event Action AddRocket;
        public void Init(PlayerModel model)
        {
            _model = model;
        }
        private IEnumerator InitLvlUpPanel()
        {
            _lvlUpMenu = Instantiate(lvlUpPanelPrefab, transform).GetComponent<LvlUpMenu>();
            _lvlUpMenu.gameObject.SetActive(true);
            while (!_lvlUpMenu.IsInit)
                yield return null;
            _lvlUpMenuPresenter = new LvlUpMenuPresenter(_lvlUpMenu);
            _lvlUpMenuPresenter.OnOpen(ChangeDamage, ChangeCooldownRocket, HideLvlUpPanel);
            _lvlUpMenu.SetMoney(_model.Money.Amount);
        }

        public void ShowLvlUpPanel()
        {
            GameStateHelper.Pause();
            if (_isFirstLvlUpPanel)
            {
                StartCoroutine(InitLvlUpPanel());
                _isFirstLvlUpPanel = false;
            }
            else
            {
                _lvlUpMenu.gameObject.SetActive(true);
                _lvlUpMenu.SetMoney(_model.Money.Amount);
            }
        }
        
        private void ChangeDamage(int damage, int index, int money)
        {
            _model.Attack.Increase(damage);
            _model.Money.Decrease(money);
            if (_lvlUpMenu.enabled) _lvlUpMenu.SetMoney(_model.Money.Amount);
            SelectSpaceship?.Invoke(index);
        }
        
        private void ChangeCooldownRocket(int time, int money)
        {
            if (!_isAddRocket)
            {
                AddRocket?.Invoke();
                _isAddRocket = true;
            }
            _model.Cooldown.SetAmount(time);
            _model.Money.Decrease(money);
            if (_lvlUpMenu.enabled) _lvlUpMenu.SetMoney(_model.Money.Amount);
        }

        private void HideLvlUpPanel()
        {
            _lvlUpMenu.gameObject.SetActive(false);
            GameStateHelper.Play();
        }

        private void OnDestroy()
        {
            if (!_isFirstLvlUpPanel)
            {
                _lvlUpMenuPresenter.OnClose(ChangeDamage, ChangeCooldownRocket, HideLvlUpPanel);
            }
        }
    }
}