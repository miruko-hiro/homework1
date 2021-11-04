using System;
using System.Collections;
using GameMechanics.Behaviors;
using GameMechanics.Enemy.DamageDisplay;
using GameMechanics.Player.Weapon.Rocket;
using UnityEngine;

namespace GameMechanics.Player.Weapon
{
    public class SpaceshipWithRockets : MonoBehaviour
    {
        [SerializeField] private GameObject rocketManagerPrefab;
        private RocketManager _rocketManager;
        [SerializeField] private SpriteRenderer shotEffect;
        [SerializeField] private GameObject damageTextPrefab;
        private DamageText _damageText;
        private Turn _turn;
        private bool _isEndCooldown = true;
        public event Action<int> RocketCooldown;
        
        public void Init(Vector2 pos)
        {
            transform.position = pos;
            _turn = new Turn();
            _damageText = Instantiate(damageTextPrefab).GetComponent<DamageText>();
            InitRocketManager();
            shotEffect.enabled = false;
        }

        private void InitRocketManager()
        {
            _rocketManager = Instantiate(rocketManagerPrefab, transform).GetComponent<RocketManager>();
            _rocketManager.Exploded += ShowDamage;
            _rocketManager.Init(transform.position);
            _rocketManager.Model.Attack.SetAmount(0);
            _rocketManager.Model.Cooldown.SetAmount(5);
        }

        private void ShowDamage(int damage, Vector2 pos)
        {
            if(damage > 0)
                _damageText.EnableAnimation(damage.ToString(), pos);
        }

        public void ShotRacket(Vector2 posClick)
        {
            if (!_isEndCooldown) return;

            RocketCooldown?.Invoke(_rocketManager.Model.Cooldown.Amount);
            
            StartCoroutine(StartCooldown());
            StartCoroutine(ShotEffect());

            Quaternion rotationOfSpaceship = _turn.GetRotationRelativeToAnotherObject(
                transform.position,
                posClick);
            
            _rocketManager.Shot(posClick, rotationOfSpaceship);

            transform.rotation = rotationOfSpaceship;
        }

        private IEnumerator StartCooldown()
        {
            _isEndCooldown = false;
            yield return new WaitForSeconds(_rocketManager.Model.Cooldown.Amount);
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

        public RocketManager GetRocketManager()
        {
            return _rocketManager;
        }

        private void OnDestroy()
        {
            _rocketManager.Exploded -= ShowDamage;
        }
    }
}