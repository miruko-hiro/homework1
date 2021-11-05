using GameMechanics.Player.Weapon;

namespace UI.Player.PlayerCooldown
{
    public class SkillCooldownPresenter
    {
        private readonly SpaceshipManager _spaceshipManager;
        private readonly ISkillCooldownView _view;

        public SkillCooldownPresenter(SpaceshipManager spaceshipManager, ISkillCooldownView view)
        {
            _spaceshipManager = spaceshipManager;
            _view = view;
            _view.Init();
        }

        public void OnOpen()
        {
            _spaceshipManager.RocketCooldown += StartRocketCooldown;
        }

        private void StartRocketCooldown(int numericCountdown)
        {
            _view.EnableAnimation(numericCountdown);
        }

        public void OnClose()
        {
            _spaceshipManager.RocketCooldown += StartRocketCooldown;
        }
    }
}