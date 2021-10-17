using System;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class DamageText : MonoBehaviour
    {
        private bool _isAnim;
        private Vector2 _randomVector;
        private Color32 _defaultColor;
        private Color32 _transparentColor;
        private TextMesh _textMesh;
        private float _speed;
        private float _percentColor;
        private Transform _transformText;

        private void Start()
        {
            _textMesh = GetComponentInChildren<TextMesh>();
            _transformText = GetComponentInChildren<Transform>();
            _defaultColor = _textMesh.color;
            _transparentColor = new Color32(_transparentColor.a, _transparentColor.b, _transparentColor.g, 0);
            _textMesh.color = _transparentColor;
            _percentColor = 0f;
            _speed = 0.01f;
        }

        private void FixedUpdate()
        {
            if (!_isAnim) return;
            _transformText.Translate(_randomVector * Time.fixedDeltaTime);
            _percentColor += _speed;
            _textMesh.color = Color.Lerp(_defaultColor, _transparentColor, _percentColor);
        }

        public void EnableAnimation(string damage, Vector2 pos)
        {
            _textMesh.text = "-" + damage;
            _transformText.position = pos;
            _textMesh.color = _defaultColor;
            _percentColor = 0f;
            _randomVector = new Vector2(Random.Range(-0.7f, 0.7f), Random.Range(-0.7f, 0.7f));
            _isAnim = true;
        }
    }
}
