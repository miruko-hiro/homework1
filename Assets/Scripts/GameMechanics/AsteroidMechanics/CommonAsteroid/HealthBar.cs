using UnityEngine;

namespace GameMechanics.AsteroidMechanics.CommonAsteroid
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Transform transformHealth;
        [SerializeField] private TextMesh textMeshHealth;
        private int _maxHealth = 0;

        public void RefreshHealth(int health)
        {
            if (_maxHealth <= health)
                _maxHealth = health;
            textMeshHealth.text = health + "/" + _maxHealth;
            float x = (float) health / _maxHealth;
            transformHealth.localScale = new Vector3(x, 1, 1);
        }
    }
}
