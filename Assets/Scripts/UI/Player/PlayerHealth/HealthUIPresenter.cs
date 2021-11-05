using GameMechanics.Behaviors;

namespace UI.Player.PlayerHealth
{
    public class HealthUIPresenter
    {
        private readonly HealthUIModel _model;
        private readonly IHealthUIView _view;

        public HealthUIPresenter(Health health, IHealthUIView view)
        {
            _model = new HealthUIModel(health);
            _view = view;
            _view.Init(3);
        }

        public void OnOpen()
        {
            _model.Health.Decreased += TakeOneLifeAway;
            _model.Health.Increased += RestoreOneLife;
        }

        private void TakeOneLifeAway(int health)
        {
            _view.TakeOneLifeAway(_model.IndexHpIcon);
            _model.IndexHpIcon = _model.IndexHpIcon == 0 ? 2 : _model.IndexHpIcon -= 1;
        }

        private void RestoreOneLife(int health)
        {
            if(_model.IndexHpIcon == 0 || _model.IndexHpIcon == 2) return;
            _model.IndexHpIcon += 1;
            _view.RestoreOneLife(_model.IndexHpIcon);
        }

        public void OnClose()
        {
            _model.Health.Decreased -= TakeOneLifeAway;
            _model.Health.Increased -= RestoreOneLife;
        }
    }
}