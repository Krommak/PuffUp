using Game.Data;
using Game.Signals;
using UnityEngine;
using Zenject;

namespace Game.Systems
{
    public sealed class ScaleBubbleSystem : IInitializable, ITickable,
        IListener<NewBubble>, IListener<MouseUp>
    {
        private readonly RuntimeData _runtimeData;

        private bool _needScaling;

        [Inject]
        public ScaleBubbleSystem(RuntimeData runtimeData)
        {
            _runtimeData = runtimeData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener<NewBubble>(this);
            TriggerListenerSystem.AddListener<MouseUp>(this);
        }

        public void Tick()
        {
            if (!_needScaling 
                || Time.time < _runtimeData.DelayTime
                || !_runtimeData.ScaledBubble
                || _runtimeData.ScaledBubble.OnPause)
            {
                return;
            }

            var bubble = _runtimeData.ScaledBubble;

            bubble.Score++;

            bubble.ScoreText.text = bubble.Score.ToString();
            var newValue = bubble.Score * bubble.Difference;
            var localScale = bubble.transform.localScale;
            localScale.Set(newValue, newValue, 0.23f);
            bubble.transform.localScale = localScale;
        }

        void IListener<NewBubble>.Trigger(NewBubble bubble)
        {
            _needScaling = true;
        }

        void IListener<MouseUp>.Trigger(MouseUp bubble)
        {
            _needScaling = false;

            if(_runtimeData.ScaledBubble)
                _runtimeData.ScaledBubble.IsComplete = true;

            TriggerListenerSystem.Trigger(new BubbleComplete()
            {
                ObjectID = _runtimeData.ScaledBubble.gameObject.GetInstanceID(),
                BubbleScore = _runtimeData.ScaledBubble.Score
            });
        }
    }
}