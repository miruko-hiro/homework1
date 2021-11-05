using System;
using System.Collections;
using GameMechanics.Helpers;
using UnityEngine;
using Zenject;

namespace GameMechanics.Player.Planet
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerController;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject explosionPlanetPrefab;
        private PlayerController _controller;
        public PlayerModel Model { get; private set; }
        public PlayerView View { get; private set; }
        public event Action GameOver;
        public void Init()
        {
            _controller = new PlayerFactory().Load(playerController, playerPrefab, transform);
            Model = _controller.Model;
            View = _controller.View;
            _controller.Model.Died += StopGame;
            
            transform.position = new Vector2(-1.5f, -3.7f);
            Model.Health.SetAmount(3);
            Model.Money.SetAmount(0);
            Model.LaserAttack.SetAmount(1);
        }
        
        private void StopGame()
        {
            StartCoroutine(ExplosionPlanet());
        }

        private IEnumerator ExplosionPlanet()
        {
            Instantiate(explosionPlanetPrefab).transform.position = new Vector2(-1.5f, -3.7f);
            View.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            GameOver?.Invoke();
            GameStateHelper.Pause();
        }

        private void OnDestroy()
        {
            if (!_controller) return;
            _controller.OnClose();
            _controller.Model.Died -= StopGame;
        }
    }
}