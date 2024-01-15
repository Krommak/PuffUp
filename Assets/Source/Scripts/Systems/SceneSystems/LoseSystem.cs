using Game.Data;
using Game.MonoBehaviours;
using Game.Signals;
using Zenject;

namespace Game.Systems
{
    public class LoseSystem : IInitializable, IListener<BubbleComplete>,
        IListener<LevelPartLoaded>, ILateDisposable
    {
        private int _scoreOfCreatedBubbles;
        private RuntimeData _runtimeData;

        private LevelPart _levelPart;

        [Inject]
        public LoseSystem(RuntimeData runtimeData)
        {
            _runtimeData = runtimeData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener<BubbleComplete>(this);
            TriggerListenerSystem.AddListener<LevelPartLoaded>(this);
        }

        public void LateDispose()
        {
            TriggerListenerSystem.RemoveListener<BubbleComplete>(this);
            TriggerListenerSystem.RemoveListener<LevelPartLoaded>(this);
        }

        void IListener<BubbleComplete>.Trigger(BubbleComplete signal)
        {
            _scoreOfCreatedBubbles += signal.CompletedBubble.Score;

            if (_runtimeData.Player.TurnCount == 0 && _scoreOfCreatedBubbles < _levelPart.TargetValueForWin)
            {
                TriggerListenerSystem.Trigger(new Lose());
            }
        }

        void IListener<LevelPartLoaded>.Trigger(LevelPartLoaded signal)
        {
            _scoreOfCreatedBubbles = 0;
            _levelPart = signal.LevelPart;
        }
    }
}
