using GameMechanics.Behaviors;
using UnityEngine;

namespace GameMechanics
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Transform transformHealth;
        [SerializeField] private TextMesh textMeshHealth;
        private HealthBehavior _healthBehavior;

        private void Start()
        {
            _healthBehavior = GetComponentInParent<HealthBehavior>();
            SetTextInTextMeshHealth();
            _healthBehavior.ChangeHealth += RefreshHealth;
        }

        private void RefreshHealth()
        {
            SetTextInTextMeshHealth();
            
            float x = (float) _healthBehavior.Health / _healthBehavior.MaxHealth;
            transformHealth.localScale = new Vector3(x, 1, 1);
        }

        private void SetTextInTextMeshHealth()
        {
            textMeshHealth.text = _healthBehavior.Health + "/" + _healthBehavior.MaxHealth;
        }

        private void OnDestroy()
        {
            _healthBehavior.ChangeHealth -= RefreshHealth;
        }
    }
}
