using Game.Signals;
using System.Collections.Generic;
using Zenject;

namespace Game.Systems
{
    public class StackSystem : IInitializable, IListener<BubbleComplete>, IListener<ConnectToStack>
    {
        private Dictionary<int, int> _bubbleScoreByObjectID;

        public void Initialize()
        {
            _bubbleScoreByObjectID = new Dictionary<int, int>();

            TriggerListenerSystem.AddListener<BubbleComplete>(this);
            TriggerListenerSystem.AddListener<ConnectToStack>(this);
        }

        void IListener<BubbleComplete>.Trigger(BubbleComplete signal)
        {
            _bubbleScoreByObjectID.Add(signal.ObjectID, signal.BubbleScore);
        }

        void IListener<ConnectToStack>.Trigger(ConnectToStack signal)
        {
            TriggerListenerSystem.Trigger(new UpdatePadlockScore()
            {
                AddedScore = _bubbleScoreByObjectID[signal.ObjectID]
            });
        }
    }
}
