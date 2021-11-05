namespace UI.Player.PlayerHealth
{
    public interface IHealthUIView
    {
        public void Init(int length);
        public void TakeOneLifeAway(int index);
        public void RestoreOneLife(int index);
    }
}