using System;
using UnityEngine;

namespace GameMechanics.Behaviors
{
    public class CooldownBehavior : MonoBehaviour, IEntityParameter
    {
        public int Amount { get; private set; }
        
        public event Action<int> ChangeAmount;
        public event Action<int> Increased;
        public event Action<int> Decreased;
        
        public void SetAmount(int amount)
        {
            Amount = amount;
            ChangeAmount?.Invoke(Amount);
        }

        public void Increase(int increase)
        {
            if (increase <= 0) return;
            Amount += increase;
            Increased?.Invoke(increase);
            ChangeAmount?.Invoke(Amount);
        }

        public void Decrease(int decrease)
        {
            if (decrease <= 0) return;
            if (Amount - decrease < 0) Amount = 0;
            else Amount -= decrease;
            Decreased?.Invoke(decrease);
            ChangeAmount?.Invoke(Amount);
        }
    }
}