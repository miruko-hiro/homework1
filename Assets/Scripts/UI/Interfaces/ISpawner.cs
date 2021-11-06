namespace UI.Interfaces
{
    public interface ISpawner<out T>
    {
        public T Spawn();
    }
}