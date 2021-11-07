using System;
using System.Collections.Generic;
using GameMechanics.Sound;
using UnityEngine;
using Zenject;

namespace GameMechanics.Player.Weapon.Rocket
{
    public class RocketManager : MonoBehaviour
    {
        [SerializeField] private GameObject rocketPrefab;
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private AudioClip flightSound;
        [SerializeField] private AudioClip explosionSound;
        private SoundManager _soundManager;
        
        public RocketModel Model { get; private set; }
        public List<RocketView> Views { get; private set; }

        private RocketController _controller;

        public event Action<int, Vector2> Exploded;

        [Inject]
        private void Construct(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }
        
        public void Init(Vector2 pos)
        {
            _controller = new RocketFactory().Load(rocketPrefab, pos, explosionPrefab, _soundManager, explosionSound);
            Model = _controller.Model;
            Views = _controller.Views;
            foreach (RocketView view in Views)
            {
                view.Exploded += NotifyAboutExplosion;
            }
        }

        private void NotifyAboutExplosion(bool isValue, Vector2 pos)
        {
            Exploded?.Invoke(isValue ? Model.Attack.Amount : 0, pos);
        }

        public void Shot(Vector2 posClick, Quaternion rotationOfSpaceship)
        {
            foreach (RocketView view in Views)
            {
                if (!view.gameObject.activeInHierarchy)
                {
                    view.gameObject.SetActive(true);
                    view.Shot(posClick, rotationOfSpaceship);
                } else if (view.IsShot)
                {
                    _controller.AddView(new RocketFactory().LoadView(rocketPrefab, Model.Position));
                }
                _soundManager.CreateSoundObjectDontDestroy()?.Play(flightSound);
            }
        }

        private void OnDestroy()
        {
            foreach (RocketView view in Views)
            {
                view.Exploded -= NotifyAboutExplosion;
            }

            _controller.OnClose();
        }
    }
}