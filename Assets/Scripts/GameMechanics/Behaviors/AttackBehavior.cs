using System;
using UnityEngine;

namespace GameMechanics.Behaviors
{
    public class AttackBehavior : MonoBehaviour
    {
        private int _amount;
        public int Amount {
            get => _amount;
            set
            {
                if (value < 0) return;
                _amount = value;
                ChangeAttack?.Invoke();
            }
        }

        public event Action ChangeAttack;
    }
}
