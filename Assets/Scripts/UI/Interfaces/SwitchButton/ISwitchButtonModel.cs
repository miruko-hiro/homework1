using System;

namespace UI.Interfaces.SwitchButton
{
    public interface ISwitchButtonModel
    {
        public event Action ChangeEnable;
        public bool Enable { get; set; }
    }
}