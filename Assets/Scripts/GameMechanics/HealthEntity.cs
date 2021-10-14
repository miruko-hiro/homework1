using System;
using UnityEngine;

namespace GameMechanics
{
    public class HealthEntity : MonoBehaviour
    {
        public int MaxHealth { get; private set; }
        public int Health { get; private set; }

        public event Action ChangeHealth;

        public void SetHealthOfEntity(int health)
        {
            MaxHealth = health;
            Health = health;
            ChangeHealth?.Invoke();
        }

        public void TakeDamage(int hit)
        {
            if (Health <= 0) return;
            
            Health -= hit;
            ChangeHealth?.Invoke();
        }
    }
}
