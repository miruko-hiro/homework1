using UnityEngine;
using UnityEngine.UI;

namespace UI.PlayerUI.PlayerMoney
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private Text amountOfMoney;
        [SerializeField] private Animation _animation;
        [SerializeField] private Text amountOfAddedMoney;

        public string AmountOfAddedMoney
        {
            get => amountOfAddedMoney.text;
            set
            {
                _animation.Stop();
                _animation.Play();
                amountOfAddedMoney.text = "+" + value;
            } 
        }

        public string AmountOfMoney
        {
            get => amountOfMoney.text;
            set => amountOfMoney.text = value;
        }
    }
}
