using GameMechanics.Behaviors;
using UnityEngine;

namespace GameMechanics
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Transform transformHealth;
        [SerializeField] private TextMesh textMeshHealth;
        private HealthBehavior health;

        private void Start()
        {
            health = GetComponentInParent<HealthBehavior>();
            SetTextInTextMeshHealth();
            health.HealthDecreased += RefreshHealth;
            health.HealthIncreased += RefreshHealth;
        }

        private void RefreshHealth()
        {
            SetTextInTextMeshHealth();
            
            float x = (float) health.Amount / health.MaxAmount;
            transformHealth.localScale = new Vector3(x, 1, 1);
        }

        private void SetTextInTextMeshHealth()
        {
            textMeshHealth.text = health.Amount + "/" + health.MaxAmount;
        }

        private void OnDestroy()
        {
            health.HealthDecreased -= RefreshHealth;
            health.HealthIncreased -= RefreshHealth;
        }
    }
}
