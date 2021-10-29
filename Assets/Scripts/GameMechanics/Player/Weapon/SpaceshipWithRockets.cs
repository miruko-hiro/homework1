﻿using System;
using System.Collections;
using System.Collections.Generic;
using GameMechanics.Behaviors;
using GameMechanics.Enemy.DamageDisplay;
using UnityEngine;

namespace GameMechanics.Player.Weapon
{
    [RequireComponent(typeof(TurnBehavior))]
    public class SpaceshipWithRockets : MonoBehaviour
    {
        [SerializeField] private GameObject rocketPrefab;
        private List<Rocket> _arrRigidbodyOfRocket = new List<Rocket>();
        [SerializeField] private SpriteRenderer shotEffect;
        [SerializeField] private GameObject damageTextPrefab;
        [SerializeField] private GameObject explosionPrefab;
        private List<Transform> _arrTransformOfExplosion = new List<Transform>();
        private DamageText _damageText;
        private CooldownBehavior _cooldown;
        private TurnBehavior _turn;
        private int _indexShot = 0;
        private bool _isEndCooldown = true;
        public event Action<int> RocketCooldown;
        
        public void Init(Vector2 pos, CooldownBehavior cooldownBehavior)
        {
            transform.position = pos;
            _turn = GetComponent<TurnBehavior>();
            _damageText = Instantiate(damageTextPrefab).GetComponent<DamageText>();
            shotEffect.enabled = false;
            _cooldown = cooldownBehavior;
            AddRocket();
        }

        public void AddRocket()
        {
            Rocket rocket = Instantiate(rocketPrefab).GetComponent<Rocket>();
            rocket.SetBasicPosition(transform.position);
            rocket.Init();
            rocket.Exploded += ShowDamage;
            rocket.gameObject.SetActive(false);
            _arrRigidbodyOfRocket.Add(rocket);
            AddExplosion();
        }

        private void AddExplosion()
        {
            Transform transformExplosion = Instantiate(explosionPrefab).GetComponent<Transform>();
            transformExplosion.position = new Vector2(-10f, 0f);
            transformExplosion.gameObject.SetActive(false);
            _arrTransformOfExplosion.Add(transformExplosion);
        }

        private void ShowDamage(int damage, Vector2 pos)
        {
            if(damage > 0)
                _damageText.EnableAnimation(damage.ToString(), pos);
            _arrTransformOfExplosion[_indexShot].position = pos;
            _arrTransformOfExplosion[_indexShot].gameObject.SetActive(false);
            _arrTransformOfExplosion[_indexShot].gameObject.SetActive(true);
        }

        public void ShotRacket(Vector2 posClick)
        {
            if (!_isEndCooldown) return;

            RocketCooldown?.Invoke(_cooldown.Amount);
            
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

        private void OnDestroy()
        {
            foreach (Rocket rocket in _arrRigidbodyOfRocket)
            {
                rocket.Exploded -= ShowDamage;
            }
        }
    }
}