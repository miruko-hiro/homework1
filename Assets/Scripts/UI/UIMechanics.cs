using System;
using System.Collections;
using GameMechanics;
using UnityEngine;

namespace UI
{
    public class UIMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject moneyUIPrefab;
        private MoneyUI _moneyUI;

        [SerializeField] private GameObject controller;
        private MainMechanics _mainMechanics;

        private IEnumerator Start()
        {
            _moneyUI = Instantiate(moneyUIPrefab, transform).GetComponent<MoneyUI>();
            _mainMechanics = controller.GetComponent<MainMechanics>();
            
            while (!_mainMechanics.Player || !_mainMechanics.Player.MoneyBehavior)
                yield return null;
            
            _mainMechanics.Player.MoneyBehavior.ChangeAmountOfMoney += SetAmountOfMoney;
        }

        private void SetAmountOfMoney()
        {
            _moneyUI.SetTextAmountOFMoney(_mainMechanics.Player.MoneyBehavior.Money.ToString());
        }

        private void OnDestroy()
        {
            _mainMechanics.Player.MoneyBehavior.ChangeAmountOfMoney -= SetAmountOfMoney;
        }
    }
}
