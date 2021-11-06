using UI.Interfaces;
using UnityEngine;

namespace UI.Panels.StartMenu.SettingsButton
{
    public class SettingsMenuSpawner : MonoBehaviour, ISpawner<SettingsMenuView>
    {
        [SerializeField] private GameObject settingsMenuPrefab;

        public SettingsMenuView Spawn()
        {
            return Instantiate(settingsMenuPrefab, transform).GetComponent<SettingsMenuView>();
        }
    }
}