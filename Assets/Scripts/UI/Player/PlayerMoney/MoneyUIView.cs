using UnityEngine;
using UnityEngine.UI;

namespace UI.Player.PlayerMoney
{
    public class MoneyUIView : MonoBehaviour, IMoneyUIView
    {
        [SerializeField] private Text amountOfMoney;
        [SerializeField] private Animation animationAddedMoney;
        [SerializeField] private Text amountOfAddedMoney;

        public void ChangeAmountOfAddedMoney(string money)
        {
            animationAddedMoney.Stop();
            animationAddedMoney.Play();
            amountOfAddedMoney.text = "+" + money;
        }
        
        public void ChangeAmountOfMoney(string money)
        {
            amountOfMoney.text = money;
        }
    }
}
