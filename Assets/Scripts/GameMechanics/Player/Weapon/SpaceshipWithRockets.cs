using System.Collections;
using System.Collections.Generic;
using GameMechanics.Behaviors;
using UnityEngine;

namespace GameMechanics.Player.Weapon
{
    [RequireComponent(typeof(TurnBehavior))]
    public class SpaceshipWithRockets : MonoBehaviour
    {
        [SerializeField] private GameObject rocketPrefab;
        private List<Rocket> _arrRigidbodyOfRocket = new List<Rocket>();
        [SerializeField] private SpriteRenderer shotEffect;
        private CooldownBehavior _cooldown;
        private TurnBehavior _turn;
        private int _indexShot = 0;
        private bool _isEndCooldown = true;
        
        public void Init(Vector2 pos, CooldownBehavior cooldownBehavior)
        {
            transform.position = pos;
            _turn = GetComponent<TurnBehavior>();
            shotEffect.enabled = false;
            _cooldown = cooldownBehavior;
            AddRocket();
        }

        public void AddRocket()
        {
            Rocket rocket = Instantiate(rocketPrefab).GetComponent<Rocket>();
            rocket.SetBasicPosition(transform.position);
            rocket.Init();
            rocket.gameObject.SetActive(false);
            _arrRigidbodyOfRocket.Add(rocket);
        }
        
        public void ShotRacket(Vector2 posClick)
        {
            if (!_isEndCooldown) return;

            StartCoroutine(StartCooldown());
            StartCoroutine(ShotEffect());

            Quaternion rotationOfSpaceship = _turn.GetRotationRelativeToAnotherObject(
                transform.position,
                posClick);
            
            _arrRigidbodyOfRocket[_indexShot].gameObject.SetActive(true);
            _arrRigidbodyOfRocket[_indexShot].Shot(posClick, rotationOfSpaceship);

            _indexShot = _indexShot < _arrRigidbodyOfRocket.Count - 1 ? _indexShot += 1 : 0;
            transform.rotation = rotationOfSpaceship;
        }

        private IEnumerator StartCooldown()
        {
            _isEndCooldown = false;
            yield return new WaitForSeconds(_cooldown.Amount);
            _isEndCooldown = true;
        }
        
        private IEnumerator ShotEffect()
        {
            if (!shotEffect.enabled)
                shotEffect.enabled = true;
            yield return new WaitForSeconds(0.1f);
            if (shotEffect.enabled)
                shotEffect.enabled = false;
        }
    }
}