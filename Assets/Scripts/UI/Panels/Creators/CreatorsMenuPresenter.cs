using System;

namespace UI.Panels.Creators
{
    public class CreatorsMenuPresenter
    {
        private readonly ICreatorsMenu _creatorsMenu;

        public CreatorsMenuPresenter(ICreatorsMenu creatorsMenu)
        {
            _creatorsMenu = creatorsMenu;
        }

        public void OnOpen(Action actionClickGooglePlay, Action actionClickReturn)
        {
            _creatorsMenu.ClickGooglePlayButton += actionClickGooglePlay;
            _creatorsMenu.ClickReturnButton += actionClickReturn;
        }

        public void OnClose(Action actionClickGooglePlay, Action actionClickReturn)
        {
            _creatorsMenu.ClickGooglePlayButton -= actionClickGooglePlay;
            _creatorsMenu.ClickReturnButton -= actionClickReturn;
        }
    }
}