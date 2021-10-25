using System.Collections;
using GameMechanics.Behaviors;
using GameMechanics.PlayerMechanics;
using UnityEngine;

namespace GameMechanics.AsteroidMechanics.CommonAsteroid
{
    [RequireComponent(
        typeof(HealthBehavior), 
        typeof(AttackBehavior),
        typeof(MovementBehavior)
    )]
    [RequireComponent(typeof(ScaleBehavior))]
    public class AsteroidController: MonoBehaviour
    {
        public AsteroidModel Model { get; private set; }
        public AsteroidView View { get; private set; }
        public HealthBar HPBar { get; private set; }
        public HealthBehavior Health { get; private set; }
        public AttackBehavior Attack { get; private set; }
        public MovementBehavior Movement { get; private set; }
        public ScaleBehavior Scale  { get; private set; }

        public void OnOpen(AsteroidModel model, AsteroidView view, HealthBar hpBar)
        {
            Model = model;
            View = view;
            if (hpBar)
            {
                HPBar = hpBar;
                InitHpBar();
            }
            
            InitHealth();
            InitModel();
            InitView();
            DefiningBehaviors();
        }

        private void InitView()
        {
            View.Died += KillAsteroid;
            View.ReceivedDamage += TakeDamage;
            View.ReachedLineOfDestroy += ReachedLineOfDestroy;
        }

        private void InitModel()
        {
            Model.IsLive = true;
            Model.ChangePosition += View.SetPosition;
            Model.ChangeLocalScale += View.SetLocalScale;
        }
        
        private void InitHealth()
        {
            if (!Health)
                Health = GetComponent<HealthBehavior>();
            Health.Decreased += View.ReAnimation;
            Health.ChangeAmount += ChangeHealth;
        }
        
        private void InitHpBar()
        {
            if (!Health)
                Health = GetComponent<HealthBehavior>();
            Health.ChangeAmount += HPBar.RefreshHealth;
        }

        private void TakeDamage(int hit)
        {
            Health.Decrease(hit);
        }

        private void ReachedLineOfDestroy()
        {
            Model.IsEndCard = false;
        }
        
        private void DefiningBehaviors()
        {
            Attack = GetComponent<AttackBehavior>();
            Movement = GetComponent<MovementBehavior>();
            Scale = GetComponent<ScaleBehavior>();
        }

        private void ChangeHealth(int health)
        {
            if (health == 0)
                KillAsteroid();
        }

        private void KillAsteroid()
        {
            StartCoroutine(Death());
        }
        
        private IEnumerator Death()
        {
            yield return new WaitForSeconds(0.3f);
            Movement.StopMove();
            Model.IsLive = false;
        }

        public void OnClose()
        {
            if (Health)
            {
                Health.Decreased -= View.ReAnimation;
                Health.ChangeAmount -= ChangeHealth;
                if (HPBar)
                {
                    Health.ChangeAmount -= HPBar.RefreshHealth;
                }
            }
            
            Model.ChangePosition -= View.SetPosition;
            Model.ChangeLocalScale -= View.SetLocalScale;
            
            View.Died -= KillAsteroid;
            View.ReceivedDamage -= TakeDamage;
            View.ReachedLineOfDestroy -= ReachedLineOfDestroy;
        }
    }
}