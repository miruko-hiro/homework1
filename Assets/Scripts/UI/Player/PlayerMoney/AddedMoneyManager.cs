using GameMechanics.Enemy;
using GameMechanics.Interfaces;
using UnityEngine;
using Zenject;

namespace UI.Player.PlayerMoney
{
    public class AddedMoneyManager : MonoBehaviour, IManager
    {
        [SerializeField] private GameObject moneyPrefab;
        [SerializeField] private Camera mainCamera;
        private RectTransform _rectTransform;
        private CommonAsteroidManager _commonAsteroidManager;
        private TranslatorWorldToScreen _translator;
        private AddedMoney[] _monetaryObjects = new AddedMoney[9];
        private int _index = 0;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _translator = new TranslatorWorldToScreen();

            for (var i = 0; i < _monetaryObjects.Length; i++)
            {
                _monetaryObjects[i] = Instantiate(moneyPrefab, transform).GetComponent<AddedMoney>();
                _monetaryObjects[i].Init();
            }

            OnOpen();
        }

        [Inject]
        private void Construct(CommonAsteroidManager commonAsteroidManager)
        {
            _commonAsteroidManager = commonAsteroidManager;
        }

        private void PourOutMoney(Vector2 pos)
        {
            pos =  _translator.WorldToScreenSpace(pos, mainCamera, _rectTransform);
            while (true)
            {
                _monetaryObjects[_index].StartAnimation(pos);
                _index = _index < _monetaryObjects.Length - 1 ? _index += 1 : 0;
                if(_index % 3 == 0) break;
            }
        }

        public void OnOpen()
        {
            _commonAsteroidManager.AsteroidExploded += PourOutMoney;
        }

        public void OnClose()
        {
            _commonAsteroidManager.AsteroidExploded -= PourOutMoney;
        }

        private void OnDestroy()
        {
            OnClose();
        }
    }
}