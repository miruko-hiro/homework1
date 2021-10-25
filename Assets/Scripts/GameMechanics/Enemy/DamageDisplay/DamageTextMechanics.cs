using UnityEngine;

namespace GameMechanics.Enemy.DamageDisplay
{
    public class DamageTextMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject damageTextPrefab;
        [SerializeField] private GameObject damageTextParent;
        private DamageText[] _damageTextArray = new DamageText[15];
        private int _damageTextIndex = 0;

        public void Init()
        {
            for (int i = 0; i < _damageTextArray.Length; i++)
            {
                _damageTextArray[i] =
                    Instantiate(damageTextPrefab, damageTextParent.transform).GetComponent<DamageText>();
            }
        }

        public void ShowDamageText(int damage, Vector2 pos)
        {
            _damageTextArray[_damageTextIndex].EnableAnimation(damage.ToString(), pos);

            if (_damageTextIndex < _damageTextArray.Length - 1)
            {
                _damageTextIndex += 1;
            }
            else
            {
                _damageTextIndex = 0;
            }
        }
    }
}