using GameMechanics.Behaviors;
using UnityEngine;

namespace GameMechanics.Player.Planet
{
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
            Model.Money = new Money();
            Model.LaserAttack = new Attack();
        }
        
        private void InitHealth()
        {
            Model.Health = new Health();
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