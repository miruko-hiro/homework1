using GameMechanics;
using GameMechanics.Enemy;
using GameMechanics.Player.Planet;
using GameMechanics.Player.Weapon;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        public PlayerManager PlayerManager;
        public SpaceshipManager SpaceshipManager;
        public GoldenAsteroidManager GoldenAsteroidManager;
        public override void InstallBindings()
        {
            Container.Bind<PrefabFactory>().AsSingle();
            
            Container.Bind<PlayerManager>().FromInstance(PlayerManager).AsSingle();
            Container.Bind<SpaceshipManager>().FromInstance(SpaceshipManager).AsSingle();
            Container.Bind<GoldenAsteroidManager>().FromInstance(GoldenAsteroidManager).AsSingle();
        }
    }
}