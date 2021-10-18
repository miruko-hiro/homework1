using System;
using System.Collections;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public class LvlUpPanel : MonoBehaviour
    {
        [SerializeField] private GameObject comeBackButton;
        public UIButton ComeBackButton { get; private set; }

        [SerializeField] private Text money;
        
        [Space(10)] 
        [SerializeField] private Sprite iconImprovement01;
        [SerializeField] private Sprite iconImprovement02;
        [SerializeField] private Sprite iconImprovement03;

        [Space(10)] 
        [SerializeField] private GameObject improvementPrefab01;
        [SerializeField] private GameObject improvementPrefab02;
        [SerializeField] private GameObject improvementPrefab03;
        
        private ImprovementUI _improvement01;
        private string _damageTextImprovement01 = "Lvl {0}: {1} damage";
        private string _damageUpTextImprovement01 = "Next {0}: {1} damage";
        private int _lvlImprovement01 = 1;
        private int _damageImprovement01 = 1;
        private int _moneyImprovement01 = 5;
        
        private ImprovementUI _improvement02;
        private string _damageTextImprovement02 = "Lvl {0}: x{1} damage";
        private string _damageUpTextImprovement02  = "Next {0}: x{1} damage";
        private int _lvlImprovement02 = 1;
        private int _damageImprovement02 = 1;
        private int _moneyImprovement02 = 10;
        
        private ImprovementUI _improvement03;
        private string _damageTextImprovement03 = "Lvl {0}: x{1} damage";
        private string _damageUpTextImprovement03  = "Next {0}: x{1} damage";
        private int _lvlImprovement03 = 1;
        private int _damageImprovement03 = 1;
        private int _moneyImprovement03 = 20;
        
        
        public bool IsInit { get; private set; }
        public event Action<int, int, int> ChangeLvl;

        private IEnumerator Start()
        {
            ComeBackButton = comeBackButton.GetComponent<UIButton>();
            
            _improvement01 = improvementPrefab01.GetComponent<ImprovementUI>();
            _improvement01.Icon = iconImprovement01;
            _improvement01.Index = 1;
            while (!_improvement01.LvlButton)
                yield return null;
            _improvement01.LvlButton.Click += LvlUpImprovement01;
            SetDamageTextImprovement01();
            SetDamageUpTextImprovement01();
            SetMoneyImprovement01();
            EnableImprovement01(false);
            
            _improvement02 = improvementPrefab02.GetComponent<ImprovementUI>();
            _improvement02.Icon = iconImprovement02;
            _improvement02.Index = 2;
            while (!_improvement02.LvlButton)
                yield return null;
            _improvement02.LvlButton.Click += LvlUpImprovement02;
            SetDamageTextImprovement02();
            SetDamageUpTextImprovement02();
            SetMoneyImprovement02();
            EnableImprovement02(false);
            
            _improvement03 = improvementPrefab03.GetComponent<ImprovementUI>();
            _improvement03.Icon = iconImprovement03;
            _improvement03.Index = 3;
            while (!_improvement03.LvlButton)
                yield return null;
            _improvement03.LvlButton.Click += LvlUpImprovement03;
            SetDamageTextImprovement03();
            SetDamageUpTextImprovement03();
            SetMoneyImprovement03();
            EnableImprovement03(false);

            IsInit = true;
        }

        public void LvlUpImprovement01()
        {
            _moneyImprovement01 += _moneyImprovement01;
            ChangeLvl?.Invoke(_damageImprovement01, _improvement01.Index, _moneyImprovement01 / 2);
            _lvlImprovement01 += 1;
            _damageImprovement01 += 1;
            SetDamageTextImprovement01();
            SetDamageUpTextImprovement01();
            SetMoneyImprovement01();
        }

        public void LvlUpImprovement02()
        {
            _moneyImprovement02 += _moneyImprovement02;
            ChangeLvl?.Invoke(_damageImprovement02, _improvement02.Index, _moneyImprovement02 / 2);
            _lvlImprovement02 += 1;
            _damageImprovement02 *= 2;
            SetDamageTextImprovement02();
            SetDamageUpTextImprovement02();
            SetMoneyImprovement02();
        }

        public void LvlUpImprovement03()
        {
            _moneyImprovement03 += _moneyImprovement03;
            ChangeLvl?.Invoke(_damageImprovement03, _improvement03.Index, _moneyImprovement03 / 2);
            _lvlImprovement03 += 1;
            _damageImprovement03 *= 2;
            SetDamageTextImprovement03();
            SetDamageUpTextImprovement03();
            SetMoneyImprovement03();
        }

        public void SetDamageTextImprovement01()
        {
            _improvement01.DamageText = string.Format(_damageTextImprovement01, _lvlImprovement01, _damageImprovement01);
        }

        public void SetDamageUpTextImprovement01()
        {
            _improvement01.DamageUpText = string.Format(_damageUpTextImprovement01, _lvlImprovement01 + 1, _damageImprovement01 + 1);
        }

        public void SetMoneyImprovement01()
        {
            _improvement01.Money = _moneyImprovement01.ToString();
        }

        public void EnableImprovement01(bool value)
        {
            _improvement01.Enable = value;
        }

        public void SetDamageTextImprovement02()
        {
            _improvement02.DamageText = string.Format(_damageTextImprovement02, _lvlImprovement02, _damageImprovement02);
        }

        public void SetDamageUpTextImprovement02()
        {
            _improvement02.DamageUpText = string.Format(_damageUpTextImprovement02, _lvlImprovement02 + 1, _damageImprovement02 * 2);
        }

        public void SetMoneyImprovement02()
        {
            _improvement02.Money = _moneyImprovement02.ToString();
        }

        public void EnableImprovement02(bool value)
        {
            _improvement02.Enable = value;
        }

        public void SetDamageTextImprovement03()
        {
            _improvement03.DamageText = string.Format(_damageTextImprovement03, _lvlImprovement03, _damageImprovement03);
        }

        public void SetDamageUpTextImprovement03()
        {
            _improvement03.DamageUpText = string.Format(_damageUpTextImprovement03, _lvlImprovement03 + 1, _damageImprovement03 * 2);
        }

        public void SetMoneyImprovement03()
        {
            _improvement03.Money = _moneyImprovement03.ToString();;
        }

        public void EnableImprovement03(bool value)
        {
            _improvement03.Enable = value;
        }

        public void SetMoney(int amount)
        {
            EnableImprovement01(_moneyImprovement01 <= amount);
            EnableImprovement02(_moneyImprovement02 <= amount);
            EnableImprovement03(_moneyImprovement03 <= amount);
            money.text = amount.ToString();
        }

        private void OnDestroy()
        {
            _improvement01.LvlButton.Click -= LvlUpImprovement01;
            _improvement02.LvlButton.Click -= LvlUpImprovement02;
            _improvement03.LvlButton.Click -= LvlUpImprovement03;
        }
    }
}
