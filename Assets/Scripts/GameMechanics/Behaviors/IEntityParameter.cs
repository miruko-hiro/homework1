using System;

namespace GameMechanics.Behaviors
{
    public interface IEntityParameter
    {
        public event Action<int> ChangeAmount;
        public event Action<int> Increased;
        public event Action<int> Decreased;

        public void SetAmount(int amount);

        public void Increase(int increase);

        public void Decrease(int decrease);
    }
}