using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMechanics
{
    public class MainMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject asteroidPrefab;
        [SerializeField] private Camera mainCamera;
        public event Action<int> AsteroidClick;

        private void Start()
        {
            Instantiate(asteroidPrefab, GetComponent<Transform>());
        }

        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                CheckTouchPosition(Input.GetTouch(0).position);
            }

            if (Input.GetMouseButtonDown(0))
            {
                CheckTouchPosition(Input.mousePosition);
            }
        }

        private void CheckTouchPosition(Vector3 touchPos)
        {
            Vector3 touchWorldPos =  mainCamera.ScreenToWorldPoint(touchPos);
            RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector2.zero);
            
            if (hit && hit.collider.CompareTag(Asteroid.Tag))
            {
                AsteroidClick?.Invoke(1);
            }
        }
    }
}
