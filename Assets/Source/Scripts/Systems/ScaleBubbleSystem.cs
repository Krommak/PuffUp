using Game.Data;
using Game.Signals;
using UnityEngine;
using Zenject;

namespace Game.Systems
{
    public sealed class ScaleBubbleSystem : IInitializable, ITickable, IListener<NewBubble>, IListener<MouseUp>
    {
        private readonly StaticData _staticData;
        private readonly RuntimeData _runtimeData;

        private bool _bubbleCreated;

        [Inject]
        public ScaleBubbleSystem(RuntimeData runtimeData, StaticData data)
        {
            _staticData = data;
            _runtimeData = runtimeData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener<NewBubble>(this);
            TriggerListenerSystem.AddListener<MouseUp>(this);
        }

        public void Tick()
        {
            if (!_bubbleCreated || Time.time < _runtimeData.DelayTime)
            {
                return;
            }
        }

        void IListener<NewBubble>.Trigger<NewBubble>()
        {
            _bubbleCreated = true;
        }

        void IListener<MouseUp>.Trigger<MouseUp>()
        {
            _bubbleCreated = false;
        }
    }
}