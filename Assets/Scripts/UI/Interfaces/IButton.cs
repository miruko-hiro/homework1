using System;

namespace UI.Interfaces
{
    public interface IButton
    {
        public event Action Click;

        public void OnClick();
    }
}