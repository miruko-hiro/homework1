using System;
using UnityEngine;

namespace GameMechanics
{
    public class AttackEntity : MonoBehaviour
    {
        public int Attack { get; private set; }

        public event Action ChangeAttack;

        public void SetAttackOfEntity(int attack)
        {
            if (attack < 0) return;
            
            Attack = attack;
            ChangeAttack?.Invoke();
        }
    }
}