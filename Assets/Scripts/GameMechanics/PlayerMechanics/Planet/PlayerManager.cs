using System;
using UnityEngine;

namespace GameMechanics.PlayerMechanics.Planet
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerController;
        [SerializeField] private GameObject playerPrefab;
        private PlayerController _controller;
        public event Action<PlayerManager> Died;
        public bool Enable { get; private set; }

        public void Init()
        {
            _controller = new PlayerFactory().Load(playerController, playerPrefab, transform);
            _controller.Model.Died += NotifyAboutDeath;
            Enable = true;
        }

        public void SetMoney(int money)
        {
            _controller.Model.Money.SetAmount(money);
        }

        public void IncreaseMoney(int money)
        {
            _controller.Model.Money.Increase(money);
        }

        public void DecreaseMoney(int money)
        {
            _controller.Model.Money.Decrease(money);
        }
            
        public void SetHealth(int health)
        {
            _controller.Model.Health.SetAmount(health);
        }
            
        public void SetAttack(int damage)
        {
            _controller.Model.Attack.SetAmount(damage);
        }
            
        public void IncreaseAttack(int damage)
        {
            _controller.Model.Attack.Increase(damage);
        }
            
        public int GetAttack()
        {
            return _controller.Model.Attack.Amount;
        }

        public int GetMoney()
        {
            return _controller.Model.Money.Amount;
        }
            
        public PlayerModel GetPlayerModel()
        {
            return _controller.Model;
        }
        
        private void NotifyAboutDeath()
        {
            Died?.Invoke(this);
        }
        private void OnDestroy()
        {
            if(_controller)
                _controller.OnClose();
        }
    }
}