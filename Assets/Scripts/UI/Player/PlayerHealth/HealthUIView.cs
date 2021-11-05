using UnityEngine;

namespace UI.Player.PlayerHealth
{
    public class HealthUIView : MonoBehaviour, IHealthUIView
    {
        [SerializeField] private GameObject[] hpIcons = new GameObject[3];
        private Animation[] _hpIconsAnimations;
        
        public void Init(int length)
        {
            _hpIconsAnimations = new Animation[length];
            for (int i = 0; i < length; i++)
            {
                _hpIconsAnimations[i] = hpIcons[i].GetComponent<Animation>();
            }
        }

        public void TakeOneLifeAway(int index)
        {
            _hpIconsAnimations[index].Play();
        }

        public void RestoreOneLife(int index)
        {
            _hpIconsAnimations[index].Stop();
        }
    }
}
