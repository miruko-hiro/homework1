using UnityEngine;

namespace GameMechanics.Player.Planet
{
    public class PlayerFactory
    {
        public PlayerController Load(GameObject playerController, GameObject playerPrefab, Transform transform)
        {
            PlayerController controller = Object.Instantiate(playerController, transform).GetComponent<PlayerController>();
            
            PlayerView view = Object.Instantiate(playerPrefab, controller.transform).GetComponent<PlayerView>();
            
            PlayerModel model = new PlayerModel();
            
            controller.OnOpen(model, view);

            return controller;
        }
    }
}