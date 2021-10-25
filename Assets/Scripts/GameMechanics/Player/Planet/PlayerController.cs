using GameMechanics.Behaviors;
using UnityEngine;

namespace GameMechanics.Player.Planet
{
    [RequireComponent(
        typeof(HealthBehavior), 
        typeof(AttackBehavior),
        typeof(MoneyBehavior)
    )]
    public class PlayerController : MonoBehaviour
    {
        public PlayerModel Model { get; private set; }
        public PlayerView View { get; private set; }
        
        public void OnOpen(PlayerModel model, PlayerView view)
        {
            Model = model;
            View = view;
            InitView();
            InitHealth();
            DefiningBehaviors();
        }
        
        private void InitView()
        {
            View.ReceivedDamage += TakeDamage;
        }
            
        private void DefiningBehaviors()
        {
            Model.Money = GetComponent<MoneyBehavior>();
            Model.Attack = GetComponent<AttackBehavior>();
        }
        
        private void InitHealth()
        {
            Model.Health = GetComponent<HealthBehavior>();
            Model.Health.ChangeAmount += ChangeHealth;
        }

        private void ChangeHealth(int health)
        {
            if (health == 0)
                Model.IsLive = false;;
        }
        
        private void TakeDamage(int hit)
        {
            Model.Health.Decrease(hit);
        }

        public void OnClose()
        {
            View.ReceivedDamage -= TakeDamage;
            Model.Health.ChangeAmount -= ChangeHealth;
        }
    }
}