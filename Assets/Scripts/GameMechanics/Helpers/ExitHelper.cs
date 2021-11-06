#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GameMechanics.Helpers
{
    public class ExitHelper
    {
        public void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}