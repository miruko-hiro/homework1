using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Player.PlayerCooldown
{
    public class SkillCooldownView : MonoBehaviour, ISkillCooldownView
    {
        [SerializeField] private Text countdown;
        private CooldownAnimation _cooldownAnimation;

        private Coroutine _coroutine;
        private bool _coroutineIsPlaying;

        public void Init()
        {
            _cooldownAnimation = GetComponent<CooldownAnimation>();
        }
        public void EnableAnimation(int numericCountdown)
        {
            _cooldownAnimation.SpawnAnimation();
            _cooldownAnimation.EnableCooldownAnimation(numericCountdown);
            
            if(_coroutineIsPlaying) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(StartCooldown(numericCountdown));
        }

        private IEnumerator StartCooldown(int numericCountdown)
        {
            _coroutineIsPlaying = this;
            
            while (numericCountdown > 0)
            {
                countdown.text = numericCountdown.ToString();
                yield return new WaitForSeconds(1f);
                numericCountdown -= 1;
            }
            countdown.text = "";
            _coroutineIsPlaying = false;
            _cooldownAnimation.FadeAnimation();
        }
    }
}