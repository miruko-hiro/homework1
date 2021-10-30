using System;
using System.Collections.Generic;
using GameMechanics.Behaviors;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameMechanics.Player.Weapon.Rocket
{
    public class RocketController
    {
        public RocketModel Model { get; private set; }
        public List<RocketView> Views { get; private set; }
        private List<Transform> _transformsOfRocketExplosion = new List<Transform>();
        private GameObject _explosionPrefab;
        private int _indexShot = 0;

        public void OnOpen(RocketModel model, RocketView view, GameObject explosionPrefab)
        {
            _explosionPrefab = explosionPrefab;
            Model = model;
            Views = new List<RocketView> {InitView(view)};
            AddExplosion();
            DefiningBehaviors();
        }

        private void DetonateRocket(bool arg1, Vector2 pos)
        {
            _transformsOfRocketExplosion[_indexShot].position = pos;
            _transformsOfRocketExplosion[_indexShot].gameObject.SetActive(false);
            _transformsOfRocketExplosion[_indexShot].gameObject.SetActive(true);
            _indexShot = _indexShot < _transformsOfRocketExplosion.Count - 1 ? _indexShot += 1 : 0;
        }

        private RocketView InitView(RocketView view)
        {
            view.Exploded += DetonateRocket;
            view.SetDamage(GetDamage);
            view.gameObject.SetActive(false);
            return view;
        }

        public void AddView(RocketView view)
        {
            Views.Add(InitView(view));
            AddExplosion();
        }
        
        private void AddExplosion()
        {
            Transform transformOfRocketExplosion = Object.Instantiate(_explosionPrefab).GetComponent<Transform>();
            transformOfRocketExplosion.position = new Vector2(-10f, 0f);
            transformOfRocketExplosion.gameObject.SetActive(false);
            _transformsOfRocketExplosion.Add(transformOfRocketExplosion);
        }
        
        private void DefiningBehaviors()
        {
            Model.Attack = new AttackBehavior();
            Model.Cooldown = new CooldownBehavior();
        }

        private int GetDamage()
        {
            return Model.Attack.Amount;
        }

        public void OnClose()
        {
            foreach (RocketView view in Views)
            {
                view.Exploded -= DetonateRocket;
            }
        }
    }
}