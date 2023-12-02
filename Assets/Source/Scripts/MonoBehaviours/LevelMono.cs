using DG.Tweening;
using Game.Signals;
using Game.Systems;
using System;
using TMPro;
using UnityEngine;

namespace Game.MonoBehaviours
{
    public class LevelMono : MonoBehaviour, IListener<UpdatePadlockScore>
    {
        public int TargetValueForWin;
        public Renderer[] Obstacles;

        [SerializeField]
        private TMP_Text _padLockText;

        private int _currentValue;

        public void Init()
        {
            _currentValue = TargetValueForWin;
            
            UpdateScoreText();
            
            TriggerListenerSystem.AddListener<UpdatePadlockScore>(this);
        }

        void IListener<UpdatePadlockScore>.Trigger(UpdatePadlockScore signal)
        {
            _currentValue -= signal.AddedScore;
            
            UpdateScoreText();
            
            if(_currentValue <= 0)
            {
                TriggerListenerSystem.Trigger(new Win());
            }
        }

        private void UpdateScoreText()
        {
            var sequence = DOTween.Sequence(this);

            var startValue = int.Parse(_padLockText.text);
            var duration = MathF.Abs(_currentValue - startValue) / 5;
            var tweener = DOTween.To(() =>
                _padLockText.text,
                x => _padLockText.text =
                ((int)Mathf.MoveTowards(int.Parse(_padLockText.text), _currentValue, duration)).ToString(),
                _currentValue.ToString(), duration);

            sequence.Join(tweener);
            sequence.Play();
        }

        private void OnDestroy()
        {
            TriggerListenerSystem.RemoveListener<UpdatePadlockScore>(this);
        }
    }
}