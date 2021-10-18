using System;

namespace UI.Buttons
{
    public interface IButton
    {
        public event Action Click;

        public void OnClick();
    }
}