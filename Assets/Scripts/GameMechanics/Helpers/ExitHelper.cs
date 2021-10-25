#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GameMechanics.Helpers
{
    public static class ExitHelper
    {
        public static void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}