﻿using System;

namespace UI.Panels.LvlUpPanel
{
    public class LvlUpMenuPresenter
    {
        private readonly LvlUpMenu _lvlUpMenu;

        public LvlUpMenuPresenter(LvlUpMenu lvlUpMenu)
        {
            _lvlUpMenu = lvlUpMenu;
        }

        public void OnOpen(Action<int, int, int> actionChangeLvl, Action<int, int> actionChangeLvlRocket, Action actionClickComeBackButton)
        {
            _lvlUpMenu.ChangeLvl += actionChangeLvl;
            _lvlUpMenu.ChangeLvlRocket += actionChangeLvlRocket;
            _lvlUpMenu.ComeBackButton.Click += actionClickComeBackButton;
        }

        public void OnClose(Action<int, int, int> actionChangeLvl, Action<int, int> actionChangeLvlRocket, Action actionClickComeBackButton)
        {
            _lvlUpMenu.ChangeLvl -= actionChangeLvl;
            _lvlUpMenu.ChangeLvlRocket -= actionChangeLvlRocket;
            _lvlUpMenu.ComeBackButton.Click -= actionClickComeBackButton;
        }
    }
}