using UnityEngine;

namespace GameMechanics
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Transform transformHealth;
        [SerializeField] private TextMesh textMeshHealth;
        private HealthEntity _healthEntity;

        private void Start()
        {
            _healthEntity = GetComponentInParent<HealthEntity>();
            SetTextInTextMeshHealth();
            _healthEntity.ChangeHealth += RefreshHealth;
        }

        private void RefreshHealth()
        {
            SetTextInTextMeshHealth();
            
            float x = (float) _healthEntity.Health / _healthEntity.MaxHealth;
            transformHealth.localScale = new Vector3(x, 1, 1);
        }

        private void SetTextInTextMeshHealth()
        {
            textMeshHealth.text = _healthEntity.Health + "/" + _healthEntity.MaxHealth;
        }

        private void OnDestroy()
        {
            _healthEntity.ChangeHealth -= RefreshHealth;
        }
    }
}
