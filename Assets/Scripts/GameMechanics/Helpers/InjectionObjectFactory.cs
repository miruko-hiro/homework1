using Zenject;

namespace GameMechanics.Helpers
{
    public class InjectionObjectFactory
    {
        private DiContainer _container;

        [Inject]
        public InjectionObjectFactory(DiContainer container)
        {
            _container = container;
        }

        public T Create<T>()
        {
            return _container.Instantiate<T>();
        }
    }
}