using GameMechanics.Behaviors;
using UnityEngine;

namespace GameMechanics
{
    [RequireComponent(
        typeof(HealthBehavior), 
        typeof(AttackBehavior)
    )]
    public class Player : MonoBehaviour
    {
        private HealthBehavior _healthBehavior;
        private AttackBehavior _attackBehavior;

        public static string Tag = "Planet";
        
        void Start()
        {
            _healthBehavior = GetComponent<HealthBehavior>();
            _healthBehavior.SetHealthOfEntity(3);
            
            _attackBehavior = GetComponent<AttackBehavior>();
            _attackBehavior.SetAttackOfEntity(1);
        }

        public int GetAmountOfDamage()
        {
            return _attackBehavior.Attack;
        }
    }
}
