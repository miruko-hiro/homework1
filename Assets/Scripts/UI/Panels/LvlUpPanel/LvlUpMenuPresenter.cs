using System;
using UI.Panels.LvlUpPanel.Improvement;

namespace UI.Panels.LvlUpPanel
{
    public class LvlUpMenuPresenter
    {
        private readonly LvlUpMenu _lvlUpMenu;

        public LvlUpMenuPresenter(LvlUpMenu lvlUpMenu)
        {
            _lvlUpMenu = lvlUpMenu;
        }

        public void OnOpen(Action<int, ImprovementType, int> actionChangeLvl, Action<int, int, int> actionChangeLvlRocket, Action actionClickComeBackButton)
        {
            _lvlUpMenu.ChangeLvlTypeOne += actionChangeLvl;
            _lvlUpMenu.ChangeLvlTypeFour += actionChangeLvlRocket;
            _lvlUpMenu.ComeBackButton.Click += actionClickComeBackButton;
        }

        public void OnClose(Action<int, ImprovementType, int> actionChangeLvl, Action<int, int, int> actionChangeLvlRocket, Action actionClickComeBackButton)
        {
            _lvlUpMenu.ChangeLvlTypeOne -= actionChangeLvl;
            _lvlUpMenu.ChangeLvlTypeFour -= actionChangeLvlRocket;
            _lvlUpMenu.ComeBackButton.Click -= actionClickComeBackButton;
        }
    }
}