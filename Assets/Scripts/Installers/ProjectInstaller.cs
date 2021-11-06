using GameMechanics.Helpers;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ExitHelper>().AsSingle();
            Container.Bind<GameStateHelper>().AsSingle();
        }
    }
}