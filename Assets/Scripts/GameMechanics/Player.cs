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
        public HealthBehavior HealthBehavior { get; private set; }
        public AttackBehavior AttackBehavior { get; private set; }
        public MoneyBehavior MoneyBehavior { get; private set; }

        public static string Tag = "Planet";
        
        void Start()
        {
            MoneyBehavior = GetComponent<MoneyBehavior>();
            MoneyBehavior.SetMoney(0);
            
            HealthBehavior = GetComponent<HealthBehavior>();
            HealthBehavior.SetHealthOfEntity(3);
            
            AttackBehavior = GetComponent<AttackBehavior>();
            AttackBehavior.SetAttackOfEntity(1);
        }
    }
}
