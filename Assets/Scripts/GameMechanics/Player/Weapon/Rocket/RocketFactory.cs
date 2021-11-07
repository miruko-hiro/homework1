using GameMechanics.Sound;
using UnityEngine;

namespace GameMechanics.Player.Weapon.Rocket
{
    public class RocketFactory
    {
        public RocketController Load(GameObject rocketPrefab, Vector2 pos, GameObject explosionPrefab, SoundManager soundManager, AudioClip sound)
        {
            RocketController controller = new RocketController();
            
            RocketView view = Object.Instantiate(rocketPrefab).GetComponent<RocketView>();
            view.SetBasicPosition(pos);
            view.Init();
            
            RocketModel model = new RocketModel();
            model.Position = pos;
            
            controller.OnOpen(model, view, explosionPrefab, soundManager, sound);

            return controller;
        }

        public RocketView LoadView(GameObject rocketPrefab, Vector2 pos)
        {
            RocketView view = Object.Instantiate(rocketPrefab).GetComponent<RocketView>();
            view.SetBasicPosition(pos);
            view.Init();

            return view;
        }
    }
}