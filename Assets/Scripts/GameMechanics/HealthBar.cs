using System;
using UnityEngine;

namespace GameMechanics
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Transform transformHealth;
        [SerializeField] private TextMesh textMeshHealth;
        private Asteroid _asteroid;

        private void Start()
        {
            _asteroid = GetComponentInParent<Asteroid>();
            SetTextInTextMeshHealth();
            _asteroid.ChangeHealth += RefreshHealth;
        }

        private void RefreshHealth()
        {
            SetTextInTextMeshHealth();
            
            float x = (float) _asteroid.Health / _asteroid.MaxHealth;
            transformHealth.localScale = new Vector3(x, 1, 1);
        }

        private void SetTextInTextMeshHealth()
        {
            textMeshHealth.text = _asteroid.Health + "/" + _asteroid.MaxHealth;
        }

        private void OnDestroy()
        {
            _asteroid.ChangeHealth -= RefreshHealth;
        }
    }
}
