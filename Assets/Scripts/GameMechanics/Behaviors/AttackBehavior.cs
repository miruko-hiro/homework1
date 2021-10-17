using System;
using UnityEngine;

namespace GameMechanics.Behaviors
{
    public class AttackBehavior : MonoBehaviour
    {
        public int Amount { get; private set; }

        public event Action ChangeAttack;

        public void SetAmount(int attack)
        {
            if (attack < 0) return;
            
            Amount = attack;
            ChangeAttack?.Invoke();
        }
    }
}
