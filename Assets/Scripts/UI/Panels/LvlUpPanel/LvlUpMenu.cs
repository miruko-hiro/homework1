using System;
using System.Collections;
using UI.Buttons;
using UI.Panels.LvlUpPanel.Improvement;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.LvlUpPanel
{
    public class LvlUpMenu : MonoBehaviour
    {
        [SerializeField] private GameObject comeBackButton;
        public UIButton ComeBackButton { get; private set; }

        [SerializeField] private Text money;
        
        [Space(10)] 
        [SerializeField] private Sprite iconImprovement01;
        [SerializeField] private Sprite iconImprovement02;
        [SerializeField] private Sprite iconImprovement03;
        [SerializeField] private Sprite iconImprovement04;

        [Space(10)] 
        [SerializeField] private GameObject improvementPrefab;
        
        private ImprovementPresenter _improvement01;
        private ImprovementPresenter _improvement02;
        private ImprovementPresenter _improvement03;
        private ImprovementPresenter _improvement04;

        private readonly ImprovementLevel _improvementLevel = new ImprovementLevel();

        public event Action<int, ImprovementType, int> ChangeLvlTypeOne;
        public event Action<int, int, int> ChangeLvlTypeFour;

        public void Init()
        {
            ComeBackButton = comeBackButton.GetComponent<UIButton>();

            _improvement01 = new ImprovementPresenter(new ImprovementModel(), Instantiate(improvementPrefab, transform).GetComponent<ImprovementView>());
            _improvement01.OnOpen(LvlUpImprovement01);
            _improvement01.Model.Icon = iconImprovement01;
            _improvement01.Model.Type = ImprovementType.One;
            InitImprovement(_improvement01.Model);
            
            _improvement02 = new ImprovementPresenter(new ImprovementModel(), Instantiate(improvementPrefab, transform).GetComponent<ImprovementView>());
            _improvement02.OnOpen(LvlUpImprovement02);
            _improvement02.Model.Icon = iconImprovement02;
            _improvement02.Model.Type = ImprovementType.Two;
            _improvement02.View.transform.localPosition += new Vector3(0f, -200f, 0f);
            InitImprovement(_improvement02.Model);
            
            _improvement03 = new ImprovementPresenter(new ImprovementModel(), Instantiate(improvementPrefab, transform).GetComponent<ImprovementView>());
            _improvement03.OnOpen(LvlUpImprovement03);
            _improvement03.Model.Icon = iconImprovement03;
            _improvement03.Model.Type = ImprovementType.Three;
            _improvement03.View.transform.localPosition += new Vector3(0f, -400f, 0f);
            InitImprovement(_improvement03.Model);
            
            _improvement04 = new ImprovementPresenter(new ImprovementModel(), Instantiate(improvementPrefab, transform).GetComponent<ImprovementView>());
            _improvement04.OnOpen(LvlUpImprovement04);
            _improvement04.Model.Icon = iconImprovement04;
            _improvement04.Model.Type = ImprovementType.Four;
            _improvement04.View.transform.localPosition += new Vector3(0f, -600f, 0f);
            InitImprovement(_improvement04.Model);
        }

        private void InitImprovement(ImprovementModel model)
        {
            model.Level = 0;
            model.Enable = false;
            SaveData(model);
        }

        private void LvlUp(ImprovementModel model)
        {
            model.Level += 1;
            SaveData(model);
        }

        private void SaveData(ImprovementModel model)
        {
            ImprovementData improvementData =_improvementLevel.GetImprovementData(model.Type, model.Level);
            model.Text = improvementData.Text;
            model.UpText = improvementData.UpText;
            model.Money = improvementData.Money;
            model.SpentMoney = improvementData.SpentMoney;
            model.Damage = improvementData.Damage;
            model.Cooldown = improvementData.Cooldown;
        }

        private void LvlUpImprovement01()
        {
            LvlUp(_improvement01.Model);
            ChangeLvlTypeOne?.Invoke(_improvement01.Model.Damage, _improvement01.Model.Type, _improvement01.Model.SpentMoney);
        }


        private void LvlUpImprovement02()
        {
            LvlUp(_improvement02.Model);
            ChangeLvlTypeOne?.Invoke(_improvement02.Model.Damage, _improvement02.Model.Type, _improvement02.Model.SpentMoney);
        }


        private void LvlUpImprovement03()
        {
            LvlUp(_improvement03.Model);
            ChangeLvlTypeOne?.Invoke(_improvement03.Model.Damage, _improvement03.Model.Type, _improvement03.Model.SpentMoney);
        }


        private void LvlUpImprovement04()
        {
            LvlUp(_improvement04.Model);
            ChangeLvlTypeFour?.Invoke(_improvement04.Model.Damage, _improvement04.Model.Cooldown, _improvement04.Model.SpentMoney);
        }
        public void SetMoney(int amount)
        {
            _improvement01.Model.Enable = _improvement01.Model.Money <= amount;
            _improvement02.Model.Enable = _improvement02.Model.Money <= amount;
            _improvement03.Model.Enable = _improvement03.Model.Money <= amount;
            _improvement04.Model.Enable = _improvement04.Model.Money <= amount;
            money.text = amount.ToString();
        }

        private void OnDestroy()
        {
            _improvement01.OnClose(LvlUpImprovement01);
            _improvement02.OnClose(LvlUpImprovement02);
            _improvement03.OnClose(LvlUpImprovement03);
            _improvement04.OnClose(LvlUpImprovement04);
        }
    }
}