using Game.Signals;
using System.Collections.Generic;
using Zenject;

namespace Game.Systems
{
    public class StackSystem : 
        IInitializable, IListener<BubbleComplete>,
        IListener<ContactWithObject>, ILateDisposable
    {
        private Dictionary<int, int> _bubbleScoreByObjectID;
        private List<int> _connectedBubbles;

        public void Initialize()
        {
            _bubbleScoreByObjectID = new Dictionary<int, int>();
            _connectedBubbles = new List<int>();

            TriggerListenerSystem.AddListener<BubbleComplete>(this);
            TriggerListenerSystem.AddListener<ContactWithObject>(this);
        }

        public void LateDispose()
        {
            TriggerListenerSystem.RemoveListener<BubbleComplete>(this);
            TriggerListenerSystem.RemoveListener<ContactWithObject>(this);
        }

        void IListener<BubbleComplete>.Trigger(BubbleComplete signal)
        {
            _bubbleScoreByObjectID.Add(signal.CompletedBubble.gameObject.GetInstanceID(), signal.CompletedBubble.Score);
        }

        void IListener<ContactWithObject>.Trigger(ContactWithObject signal)
        {
            var id = signal.ContactedMono.gameObject.GetInstanceID();

            if (_bubbleScoreByObjectID.ContainsKey(id) 
                && (_connectedBubbles.Contains(signal.OtherObjectID) || signal.IsPadlock))
            {
                TriggerListenerSystem.Trigger(new UpdatePadlockScore()
                {
                    AddedScore = _bubbleScoreByObjectID[id]
                });

                signal.ContactedMono.InStack = true;

                _connectedBubbles.Add(id);
                _bubbleScoreByObjectID.Remove(id);
            }
        }
    }
}
