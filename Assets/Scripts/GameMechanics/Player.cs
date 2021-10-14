using UnityEngine;

namespace GameMechanics
{
    [RequireComponent(
        typeof(HealthEntity), 
        typeof(AttackEntity)
    )]
    public class Player : MonoBehaviour
    {
        private HealthEntity _healthEntity;
        private AttackEntity _attackEntity;
        
        void Start()
        {
            _healthEntity = GetComponent<HealthEntity>();
            _healthEntity.SetHealthOfEntity(3);
            
            _attackEntity = GetComponent<AttackEntity>();
            _attackEntity.SetAttackOfEntity(1);
        }
    }
}
