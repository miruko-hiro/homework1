using System;
using System.Collections;
using GameMechanics.Helpers;
using GameMechanics.Player.Planet;
using UnityEngine;

namespace GameMechanics.Player
{
    public class PlayerMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject playerManagerPrefab;
        [SerializeField] private GameObject explosionPlanetPrefab;
        private PlayerManager _playerManager;
        public event Action GameOver;
        public PlayerManager InitPlayer()
        {
            _playerManager = Instantiate(playerManagerPrefab).GetComponent<PlayerManager>();
            _playerManager.Init();
            _playerManager.transform.position = new Vector2(-1.5f, -3.7f);
            _playerManager.Model.Health.SetAmount(3);
            _playerManager.Model.Money.SetAmount(0);
            _playerManager.Model.LaserAttack.SetAmount(1);
            _playerManager.Died += StopGame;
            return _playerManager;
        }
        
        private void StopGame(PlayerManager playerManager)
        {
            StartCoroutine(ExplosionPlanet());
        }

        private IEnumerator ExplosionPlanet()
        {
            Instantiate(explosionPlanetPrefab).transform.position = new Vector2(-1.5f, -3.7f);
            _playerManager.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            GameOver?.Invoke();
            GameStateHelper.Pause();
        }

        private void OnDestroy()
        {
            _playerManager.Died -= StopGame;
        }
    }
}