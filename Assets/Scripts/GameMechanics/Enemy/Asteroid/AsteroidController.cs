using System.Collections;
using GameMechanics.Behaviors;
using UnityEngine;

namespace GameMechanics.Enemy.Asteroid
{
    [RequireComponent(
        typeof(MovementBehavior),
        typeof(ScaleBehavior)
    )]
    public class AsteroidController: MonoBehaviour
    {
        public AsteroidModel Model { get; private set; }
        public AsteroidView View { get; private set; }

        public void OnOpen(AsteroidModel model, AsteroidView view, HealthBar hpBar)
        {
            Model = model;
            View = view;
            
            InitModel();
            InitView();
            DefiningBehaviors();
            InitHealth();
            
            if (hpBar)
            {
                Model.HpBar = hpBar;
                InitHpBar();
            }
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
            Model.Health = new HealthBehavior();
            Model.Health.Decreased += View.ReAnimation;
            Model.Health.ChangeAmount += ChangeHealth;
        }
        
        private void InitHpBar()
        {
            Model.Health.ChangeAmount += Model.HpBar.RefreshHealth;
        }

        private void TakeDamage(int hit)
        {
            Model.Health.Decrease(hit);
        }

        private void ReachedLineOfDestroy()
        {
            Model.IsEndCard = false;
        }
        
        private void DefiningBehaviors()
        {
            Model.Attack = new AttackBehavior();
            Model.Movement = GetComponent<MovementBehavior>();
            Model.Scale = GetComponent<ScaleBehavior>();
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
            Model.Movement.StopMove();
            Model.IsLive = false;
        }

        public void OnClose()
        {
            Model.Health.Decreased -= View.ReAnimation;
            Model.Health.ChangeAmount -= ChangeHealth;
            if (Model.HpBar)
            {
                Model.Health.ChangeAmount -= Model.HpBar.RefreshHealth;
            }
            
            Model.ChangePosition -= View.SetPosition;
            Model.ChangeLocalScale -= View.SetLocalScale;
            
            View.Died -= KillAsteroid;
            View.ReceivedDamage -= TakeDamage;
            View.ReachedLineOfDestroy -= ReachedLineOfDestroy;
        }
    }
}