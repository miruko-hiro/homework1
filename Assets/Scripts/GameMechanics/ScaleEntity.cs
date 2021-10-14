using System;
using UnityEngine;

namespace GameMechanics
{
    public class ScaleEntity : MonoBehaviour
    {
        private float _scalseX;
        private float _scalseY;
        private float _maxX;
        private float _maxY;
        private bool _isMaxScale;
        private bool _isScale;
        private Transform _transform;

        private void Start()
        {
            _transform = GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            if (_isScale)
            {
                Vector2 localScale = _transform.localScale;
                if (_isMaxScale && (localScale.x >= _maxX || localScale.y >= _maxY))
                {
                    _isScale = false;
                    return;
                }
                
                _transform.localScale = new Vector2(localScale.x + _scalseX, localScale.y + _scalseY);
            }
        }

        public void SetScale(float scaleX, float scaleY)
        {
            _scalseX = scaleX;
            _scalseY = scaleY;
        }

        public void SetMaxScale(float maxX, float maxY)
        {
            _maxX = maxX;
            _maxY = maxY;
            _isMaxScale = true;
        }

        public void ActiveScale(bool value)
        {
            _isScale = value;
        }
    }
}
