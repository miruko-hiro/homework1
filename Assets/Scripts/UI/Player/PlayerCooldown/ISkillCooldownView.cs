namespace UI.Player.PlayerCooldown
{
    public interface ISkillCooldownView
    {
        public void Init();
        public void EnableAnimation(int numericCountdown);
    }
}