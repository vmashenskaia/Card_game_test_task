using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class StatWidget : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _amount;
        [SerializeField]
        private Image _iconImage;

        public void ApplyStat(Stat stat)
        {
            _amount.text = stat.count.ToString();
            _iconImage.sprite = stat.icon;
        }
    }
}