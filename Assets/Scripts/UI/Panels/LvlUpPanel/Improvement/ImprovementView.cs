using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.LvlUpPanel.Improvement
{
    public class ImprovementView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Text damageText;
        [SerializeField] private Text damageUpText;
        [SerializeField] private Text moneyText;
        [SerializeField] private Button button;
        public UIButton LvlButton { get; private set; }

        public void Init()
        {
            LvlButton = button.GetComponent<UIButton>();
        }
        public void SetButtonInteractable(bool value)
        {
            button.interactable = value;
        }

        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public void SetText(string text)
        {
            damageText.text = text;
        }

        public void SetUpText(string upText)
        {
            damageUpText.text = upText;
        }

        public void SetMoney(string money)
        {
            moneyText.text = money;
        }
    }
}
