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
            Money.Amount = 0;
            
            Health = GetComponent<HealthBehavior>();
            Health.SetAmount(3);
            
            Attack = GetComponent<AttackBehavior>();
            Attack.Amount = 1;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(MainMechanics.Asteroid01Tag) 
                || other.CompareTag(MainMechanics.Asteroid02Tag) 
                || other.CompareTag(MainMechanics.Asteroid03Tag))
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
