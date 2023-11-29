using TMPro;
using UnityEngine;

namespace Game.MonoBehaviours
{
    public class LevelMono : MonoBehaviour
    {
        [SerializeField]
        private int _targetValueForWin;
        [SerializeField]
        private TMP_Text _padLockText;

        public void Init()
        {
            _padLockText.text = _targetValueForWin.ToString();
        }
    }
}