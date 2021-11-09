using GameMechanics;
using GameMechanics.Enemy;
using GameMechanics.Helpers;
using GameMechanics.Player.Planet;
using GameMechanics.Player.Weapon;
using UI.Panels.LvlUpPanel.Improvement;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        public PlayerManager PlayerManager;
        public MainManager MainManager;
        public SpaceshipManager SpaceshipManager;
        public GoldenAsteroidManager GoldenAsteroidManager;
        public CommonAsteroidManager CommonAsteroidManager;
        public override void InstallBindings()
        {
            Container.Bind<PrefabFactory>().AsSingle();
            Container.Bind<InjectionObjectFactory>().AsSingle();
            Container.Bind<ImprovementLevel>().AsSingle();
            
            Container.Bind<PlayerManager>().FromInstance(PlayerManager).AsSingle();
            Container.Bind<MainManager>().FromInstance(MainManager).AsSingle();
            Container.Bind<SpaceshipManager>().FromInstance(SpaceshipManager).AsSingle();
            Container.Bind<GoldenAsteroidManager>().FromInstance(GoldenAsteroidManager).AsSingle();
            Container.Bind<CommonAsteroidManager>().FromInstance(CommonAsteroidManager).AsSingle();
        }
    }
}