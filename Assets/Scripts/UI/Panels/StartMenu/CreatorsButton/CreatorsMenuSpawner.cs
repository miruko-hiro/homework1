using UI.Interfaces;
using UnityEngine;

namespace UI.Panels.StartMenu.CreatorsButton
{
    public class CreatorsMenuSpawner : MonoBehaviour, ISpawner<CreatorsMenuView>
    {
        [SerializeField] private GameObject creatorsMenuPrefab;

        public CreatorsMenuView Spawn()
        {
            return Instantiate(creatorsMenuPrefab, transform).GetComponent<CreatorsMenuView>();
        }
    }
}