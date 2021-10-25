using System;
using UnityEngine;

namespace GameMechanics.Player.Planet
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerController;
        [SerializeField] private GameObject playerPrefab;
        private PlayerController _controller;
        public PlayerModel Model { get; private set; }
        public PlayerView View { get; private set; }
        public event Action<PlayerManager> Died;
        public bool Enable { get; private set; }

        public void Init()
        {
            _controller = new PlayerFactory().Load(playerController, playerPrefab, transform);
            Model = _controller.Model;
            View = _controller.View;
            _controller.Model.Died += NotifyAboutDeath;
            Enable = true;
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