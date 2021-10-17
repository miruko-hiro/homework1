using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField ]private Text amountOfMoney;

        public void SetTextAmountOFMoney(string amount)
        {
            amountOfMoney.text = amount;
        }
    }
}
