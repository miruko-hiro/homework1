using UnityEngine;

namespace GameMechanics.Behaviors
{
    public class ScaleBehavior : MonoBehaviour
    {
        public Vector2 Scale { get; set; }
        public Vector2 MaxScale { get; set; }

        private bool _isActive;

        private Transform _transform;

        private void FixedUpdate()
        {
            if (_isActive)
            {
                Vector2 localScale = _transform.localScale;
                if (localScale.x >= MaxScale.x || localScale.y >= MaxScale.y)
                {
                    _isActive = false;
                    return;
                }
                
                _transform.localScale = new Vector2(localScale.x + Scale.x, localScale.y + Scale.y);
            }
        }

        public void Active(bool isActive, Transform tf)
        {
            _transform = tf;
            _isActive = isActive;
        }
    }
}
