using UnityEngine;

namespace GameMechanics.Behaviors
{
    public class MovementBehavior : MonoBehaviour
    {
        private float _speed;
        private bool _isMove;
        private Vector2 _endPosition;
        private Transform _transform;

        private void Start()
        {
            _transform = GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            if (_isMove)
            {
                if (Vector2.Distance(_transform.position, _endPosition) < 0.001f)
                {
                    _isMove = false;
                    return;
                }
                
                _transform.Translate(_endPosition * (_speed * Time.fixedDeltaTime));
            }
        }

        public void Move(Vector3 endPosition, float speed)
        {
            _endPosition = endPosition;
            _speed = speed;
            _isMove = true;
        }

        public void StopMove()
        {
            _isMove = false;
        }
    }
}
