using System;
using UnityEngine;

namespace GameMechanics.PlayerMechanics.Planet
{
    public class PlayerView : MonoBehaviour
    {
        public event Action<int> ReceivedDamage;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Asteroid"))
            {
                ReceivedDamage?.Invoke(1);
            }
        }
    }
}