namespace UI.Panels.LvlUpPanel.Improvement
{
    public readonly struct ImprovementData
    {
        public readonly string Text;
        public readonly string UpText;
        public readonly int Money;
        public readonly int SpentMoney;
        public readonly int Damage;
        public readonly int Cooldown;

        public ImprovementData(string text, string upText, int money, int spentMoney, int damage, int cooldown)
        {
            Text = text;
            UpText = upText;
            Money = money;
            SpentMoney = spentMoney;
            Damage = damage;
            Cooldown = cooldown;
        }
    }
}