using System;

namespace UI.Panels.Creators
{
    public interface ICreatorsMenu
    {
        public event Action ClickGooglePlayButton;
        public event Action ClickReturnButton;
        public void OnClickGooglePlay();
        public void OnClickReturn();
    }
}