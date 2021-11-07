using System;

namespace UI.Interfaces.SwitchButton
{
    public interface ISwitchButtonModel
    {
        public event Action ChangeEnabled;
        public bool Enabled { get; set; }
    }
}