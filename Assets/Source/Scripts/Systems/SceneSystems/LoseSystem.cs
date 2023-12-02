using Game.Data;
using Game.Signals;
using Zenject;

namespace Game.Systems
{
    public class LoseSystem : IInitializable, IListener<BubbleComplete>, ILateDisposable
    {
        private int _scoreOfCreatedBubbles;
        private RuntimeData _runtimeData;

        [Inject]
        public LoseSystem(RuntimeData runtimeData)
        {
            _runtimeData = runtimeData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener<BubbleComplete>(this);
        }

        public void LateDispose()
        {
            TriggerListenerSystem.RemoveListener<BubbleComplete>(this);
        }

        void IListener<BubbleComplete>.Trigger(BubbleComplete signal)
        {
            _scoreOfCreatedBubbles += signal.CompletedBubble.Score;

            if (_runtimeData.Player.TurnCount == 0 && _scoreOfCreatedBubbles < _runtimeData.LoadedLevel.TargetValueForWin)
            {
                TriggerListenerSystem.Trigger(new Lose());
            }
        }
    }
}
