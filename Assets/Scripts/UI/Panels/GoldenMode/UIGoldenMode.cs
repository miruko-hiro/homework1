using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.GoldenMode
{
    public class UIGoldenMode : MonoBehaviour
    {
        [SerializeField] private Text score;
        [SerializeField] private Text time;

        public string Score {get => score.text; set => score.text = value;}
        public string Time {get => time.text; set => time.text = value;}
    }
}
