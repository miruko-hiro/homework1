using System;
using GameMechanics.Behaviors;
using UnityEngine;

namespace GameMechanics
{
    [RequireComponent(
        typeof(HealthBehavior), 
        typeof(AttackBehavior),
        typeof(MoneyBehavior)
    )]
    public class Player : MonoBehaviour
    {
        public HealthBehavior Health { get; private set; }
        public AttackBehavior Attack { get; private set; }
        public MoneyBehavior Money { get; private set; }

        public static string Tag = "Planet";
        public event Action Died;
        
        void Start()
        {
            Money = GetComponent<MoneyBehavior>();
            Money.SetAmount(0);
            
            Health = GetComponent<HealthBehavior>();
            Health.SetAmount(3);
            
            Attack = GetComponent<AttackBehavior>();
            Attack.SetAmount(1);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Asteroid.Tag))
            {
                Health.TakeDamage(1);
                if (Health.Amount == 0)
                {
                    Died?.Invoke();
                }
            }
        }
    }
}
