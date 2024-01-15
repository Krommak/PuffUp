using Game.Data;
using Game.Signals;
using Zenject;

namespace Game.Systems
{
    public class MoneySystem : IInitializable, IListener<BubbleComplete>
    {
        private StaticData _staticData;
        private RuntimeData _runtimeData;

        [Inject]
        public MoneySystem(StaticData staticData, RuntimeData runtimeData)
        {
            _staticData = staticData;
            _runtimeData = runtimeData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener(this);
        }

        void IListener<BubbleComplete>.Trigger(BubbleComplete bubble)
        {
            _runtimeData.Player.Money += bubble.CompletedBubble.Score;

            TriggerListenerSystem.Trigger(new UpdateUI());
        }
    }
}