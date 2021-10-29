using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PlayerUI.PlayerCooldown
{
    public class SkillCooldown : MonoBehaviour
    {
        [SerializeField] private Text countdown;
        [SerializeField] private Animation countdownAnimation;
        private int _numericCountdown;

        public void EnableAnimation(int numericCountdown)
        {
            _numericCountdown = numericCountdown;
            countdownAnimation.Stop();
            StartCoroutine(StartCooldown());
        }

        private IEnumerator StartCooldown()
        {
            countdownAnimation["CooldownAnimation"].speed = 1f / _numericCountdown;
            countdownAnimation.Play();
            while (_numericCountdown > 0)
            {
                countdown.text = _numericCountdown.ToString();
                yield return new WaitForSeconds(1f);
                _numericCountdown -= 1;
            }
            countdown.text = "";
        }
    }
}