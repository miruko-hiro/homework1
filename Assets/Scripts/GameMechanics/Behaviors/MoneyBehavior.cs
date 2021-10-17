using System;
using UnityEngine;

namespace GameMechanics.Behaviors
{
    public class MoneyBehavior : MonoBehaviour
    {
        public int Money { get; private set; }

        public event Action ChangeAmountOfMoney;

        public void SetMoney(int money)
        {
            Money = money;
            ChangeAmountOfMoney?.Invoke();
        }

        public void IncreaseMoney(int increase)
        {
            Money += increase;
            ChangeAmountOfMoney?.Invoke();
        }
    }
}