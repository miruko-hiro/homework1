using System;
using UnityEngine;

namespace UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private GameObject[] hpIcons = new GameObject[3];
        private Animation[] hpIconsAnimations = new Animation[3];

        private int _indexHpIcon = 2;

        private void Start()
        {
            for (int i = 0; i < hpIconsAnimations.Length; i++)
            {
                hpIconsAnimations[i] = hpIcons[i].GetComponent<Animation>();
            }
        }

        public void TakeOneLifeAway()
        {
            hpIconsAnimations[_indexHpIcon].Play();

            _indexHpIcon = _indexHpIcon == 0 ? 2 : _indexHpIcon -= 1;
        }

        public void RestoreOneLife()
        {
            if(_indexHpIcon == 0 || _indexHpIcon == 2) return;

            _indexHpIcon += 1;
            hpIconsAnimations[_indexHpIcon].Stop();
        }

        public void RestoreAllLife()
        {
            _indexHpIcon = 2;
            foreach (Animation hpIconsAnimation in hpIconsAnimations)
            {
                hpIconsAnimation.Stop();
            }
        }
    }
}
