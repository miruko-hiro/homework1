﻿using UnityEngine;

namespace GameMechanics
{
    public static class GameStateHelper
    {
        public static void Pause()
        {
            Time.timeScale = 0;
        }

        public static void Play()
        {
            Time.timeScale = 1;
        }
    }
}