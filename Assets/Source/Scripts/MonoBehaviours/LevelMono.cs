using Game.Signals;
using Game.Systems;
using TMPro;
using UnityEngine;

namespace Game.MonoBehaviours
{
    public class LevelMono : MonoBehaviour, IListener<UpdatePadlockScore>
    {
        [SerializeField]
        private int _targetValueForWin;
        [SerializeField]
        private TMP_Text _padLockText;

        private int _currentValue;

        public void Init()
        {
            _currentValue = _targetValueForWin;
            
            UpdateText();
            
            TriggerListenerSystem.AddListener<UpdatePadlockScore>(this);
        }

        void IListener<UpdatePadlockScore>.Trigger(UpdatePadlockScore signal)
        {
            _currentValue -= signal.AddedScore;
            
            UpdateText();
            
            if(_currentValue <= 0)
            {

            }
        }

        private void UpdateText()
        {
            _padLockText.text = _currentValue.ToString();
        }

        private void OnDestroy()
        {
            TriggerListenerSystem.RemoveListener<UpdatePadlockScore>(this);
        }
    }
}