﻿using UnityEngine;

namespace GameMechanics.Enemy.Asteroid
{
    public class AsteroidFactory
    {
        public AsteroidController Load(GameObject asteroidController, GameObject asteroidPrefab, GameObject healthBarPrefab, Transform transform)
        {
            AsteroidController controller = Object.Instantiate(asteroidController, transform).GetComponent<AsteroidController>();
            
            AsteroidView view = Object.Instantiate(asteroidPrefab, controller.transform).GetComponent<AsteroidView>();

            HealthBar hpBar  = Object.Instantiate(healthBarPrefab, view.transform).GetComponent<HealthBar>();
            
            AsteroidModel model = new AsteroidModel();
            
            controller.OnOpen(model, view, hpBar);

            return controller;
        }
        
        public AsteroidController Load(GameObject asteroidController, GameObject asteroidPrefab, Transform transform)
        {
            AsteroidController controller = Object.Instantiate(asteroidController, transform).GetComponent<AsteroidController>();
            
            AsteroidView view = Object.Instantiate(asteroidPrefab, controller.transform).GetComponent<AsteroidView>();
            
            AsteroidModel model = new AsteroidModel();
            
            controller.OnOpen(model, view);

            return controller;
        }
    }
}