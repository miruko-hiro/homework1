using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Player.PlayerCooldown
{
    public class SkillCooldownView : MonoBehaviour, ISkillCooldownView
    {
        [SerializeField] private Text countdown;
        [SerializeField] private Animation countdownAnimation;
        private Animator _animator;
        private static readonly int Enable = Animator.StringToHash("Enable");

        private Coroutine _coroutine;
        private bool _corotineIsPlaying = false;

        public void Init()
        {
            _animator = GetComponent<Animator>();
            _animator.SetBool(Enable, false);
        }
        public void EnableAnimation(int numericCountdown)
        {
            countdownAnimation.Stop();
            _animator.SetBool(Enable, true);
            
            if(_corotineIsPlaying) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(StartCooldown(numericCountdown));
        }

        private IEnumerator StartCooldown(int numericCountdown)
        {
            _corotineIsPlaying = this;
            if (numericCountdown != 0)
                countdownAnimation["CooldownAnimation"].speed = 1f / numericCountdown;
            
            countdownAnimation.Play();
            while (numericCountdown > 0)
            {
                countdown.text = numericCountdown.ToString();
                yield return new WaitForSeconds(1f);
                numericCountdown -= 1;
            }
            countdown.text = "";
            _animator.SetBool(Enable, false);
            _corotineIsPlaying = false;
        }
    }
}