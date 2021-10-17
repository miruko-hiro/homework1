using System;
using UnityEngine;

namespace GameMechanics.Behaviors
{
    public class MoneyBehavior : MonoBehaviour
    {
        private int _amount;

        public int Amount
        {
            get => _amount;
            set
            {
                if (value < 0) return;
                _amount = value;
                ChangeAmountOfMoney?.Invoke();
            }
        }

        public event Action ChangeAmountOfMoney;
    }
}