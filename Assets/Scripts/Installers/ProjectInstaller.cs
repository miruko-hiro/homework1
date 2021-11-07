using GameMechanics.Helpers;
using GameMechanics.Sound;
using UI.Sound;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public SoundManager SoundManager;
        public MusicManager MusicManager;
        public override void InstallBindings()
        {
            Container.Bind<ExitHelper>().AsSingle();
            Container.Bind<GameStateHelper>().AsSingle();
            Container.Bind<MusicClaspRepository>().AsSingle();
            
            Container.Bind<SoundManager>().FromInstance(SoundManager).AsSingle();
            Container.Bind<MusicManager>().FromInstance(MusicManager).AsSingle();
        }
    }
}