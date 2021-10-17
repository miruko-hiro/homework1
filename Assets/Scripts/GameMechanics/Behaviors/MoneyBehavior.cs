using System;
using UnityEngine;

namespace GameMechanics.Behaviors
{
    public class MoneyBehavior : MonoBehaviour
    {
        public int Amount { get; private set; }

        public event Action ChangeAmountOfMoney;

        public void SetAmount(int money)
        {
            Amount = money;
            ChangeAmountOfMoney?.Invoke();
        }

        public void IncreaseAmount(int increase)
        {
            Amount += increase;
            ChangeAmountOfMoney?.Invoke();
        }
    }
}