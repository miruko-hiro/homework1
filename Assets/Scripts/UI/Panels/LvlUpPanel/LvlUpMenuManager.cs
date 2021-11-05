using System;
using System.Collections;
using GameMechanics.Helpers;
using GameMechanics.Player.Planet;
using GameMechanics.Player.Weapon.Rocket;
using UI.Panels.LvlUpPanel.Improvement;
using UnityEngine;
using Zenject;

namespace UI.Panels.LvlUpPanel
{
    public class LvlUpMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject lvlUpPanelPrefab;
        private LvlUpMenu _lvlUpMenu;
        private LvlUpMenuPresenter _lvlUpMenuPresenter;
        private PlayerManager _playerManager;
        private RocketModel _rocketModel;
        private bool _isFirstLvlUpPanel = true;
        private bool _isAddRocket;

        public event Action<ImprovementType> SelectSpaceship;
        public event Action AddRocket;
        public event Action ContinueGame;

        [Inject]
        private void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        private IEnumerator InitLvlUpPanel()
        {
            _lvlUpMenu = Instantiate(lvlUpPanelPrefab, transform).GetComponent<LvlUpMenu>();
            _lvlUpMenu.gameObject.SetActive(true);
            while (!_lvlUpMenu.IsInit)
                yield return null;
            _lvlUpMenuPresenter = new LvlUpMenuPresenter(_lvlUpMenu);
            _lvlUpMenuPresenter.OnOpen(ChangeDamage, ChangeCooldownRocket, HideLvlUpPanel);
            _lvlUpMenu.SetMoney(_playerManager.Model.Money.Amount);
        }

        public void RocketInit(RocketModel rocketModel)
        {
            _rocketModel = rocketModel;
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
                _lvlUpMenu.SetMoney(_playerManager.Model.Money.Amount);
            }
        }
        
        private void ChangeDamage(int damage, ImprovementType type, int money)
        {
            _playerManager.Model.LaserAttack.Increase(damage);
            _playerManager.Model.Money.Decrease(money);
            if (_lvlUpMenu.enabled) _lvlUpMenu.SetMoney(_playerManager.Model.Money.Amount);
            SelectSpaceship?.Invoke(type);
        }
        
        private void ChangeCooldownRocket(int damage, int time, int money)
        {
            if (!_isAddRocket)
            {
                AddRocket?.Invoke();
                _isAddRocket = true;
            }
            _rocketModel.Cooldown.Decrease(_rocketModel.Cooldown.Amount - time);
            _rocketModel.Attack.Increase(damage);
            _playerManager.Model.Money.Decrease(money);
            if (_lvlUpMenu.enabled) _lvlUpMenu.SetMoney(_playerManager.Model.Money.Amount);
        }

        private void HideLvlUpPanel()
        {
            _lvlUpMenu.gameObject.SetActive(false);
            ContinueGame?.Invoke();
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