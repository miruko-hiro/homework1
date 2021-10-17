using System;
using UnityEngine;

namespace GameMechanics.Behaviors
{
    public class HealthBehavior : MonoBehaviour
    {
        public int MaxAmount { get; private set; }
        public int Amount { get; private set; }

        public event Action ChangeHealth;

        public void SetAmount(int health)
        {
            MaxAmount = health;
            Amount = health;
            ChangeHealth?.Invoke();
        }

        public void TakeDamage(int hit)
        {
            if (Amount <= 0) return;
            
            Amount -= hit;
            ChangeHealth?.Invoke();
        }
    }
}
