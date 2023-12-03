using Game.Data;
using Game.MonoBehaviours;
using Game.Signals;
using UnityEngine;
using Zenject;

namespace Game.Systems
{
    public sealed class EnemyMovementSystem : IInitializable, ITickable,
        IListener<NewBubble>, IListener<MouseUp>, ILateDisposable
    {
        private readonly RuntimeData _runtimeData;

        private bool _needScaling;
        private BubbleMono _scaledBubble;

        [Inject]
        public EnemyMovementSystem(RuntimeData runtimeData)
        {
            _runtimeData = runtimeData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener<NewBubble>(this);
            TriggerListenerSystem.AddListener<MouseUp>(this);
        }

        public void LateDispose()
        {
            TriggerListenerSystem.RemoveListener<NewBubble>(this);
            TriggerListenerSystem.RemoveListener<MouseUp>(this);
        }

        public void Tick()
        {
            if (!_needScaling 
                || Time.time < _runtimeData.DelayTime
                || !_scaledBubble
                || _scaledBubble.OnPause)
            {
                return;
            }

            _scaledBubble.Score++;

            _scaledBubble.ScoreText.text = _scaledBubble.Score.ToString();
            var newValue = _scaledBubble.Score * _scaledBubble.Difference;
            var localScale = _scaledBubble.transform.localScale;
            localScale.Set(newValue, newValue, 0.23f);
            _scaledBubble.transform.localScale = localScale;
        }

        void IListener<NewBubble>.Trigger(NewBubble bubble)
        {
            _needScaling = true;
            _scaledBubble = bubble.CreatedBubble;
        }

        void IListener<MouseUp>.Trigger(MouseUp bubble)
        {
            _needScaling = false;

            if(_scaledBubble)
            {
                foreach (var item in _scaledBubble.ParticleComplete)
                {
                    item.Play();
                }

                TriggerListenerSystem.Trigger(new BubbleComplete()
                {
                    CompletedBubble = _scaledBubble
                });

                _scaledBubble.SetComplete();

                _scaledBubble = null;
            }
        }
    }
}