using Game.MonoBehaviours;

namespace Game.Signals
{
    public interface ISignal
    {
    }

    public struct MouseDown : ISignal
    {
    }

    public struct MouseUp : ISignal
    {
    }

    public struct NewBubble : ISignal
    {
        public BubbleMono CreatedBubble;
    }

    public struct BubbleComplete: ISignal
    {
        public BubbleMono CompletedBubble;
    }

    public struct LevelLoaded : ISignal
    {
    }

    public struct LoadLevel : ISignal
    {
    }

    public struct UILoaded : ISignal
    {
        public UIMono UIMono;
    }
    
    public struct UpdateUI : ISignal
    {
    }

    public struct ContactWithObject : ISignal
    {
        public BubbleMono ContactedMono;
        public int OtherObjectID;
        public bool IsPadlock;
    }

    public struct UpdatePadlockScore : ISignal
    {
        public int AddedScore;
    }

    public struct Win : ISignal
    {
    }

    public struct Lose : ISignal
    {
    }
}
