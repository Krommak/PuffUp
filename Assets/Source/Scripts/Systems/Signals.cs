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

    public struct LevelLoaded : ISignal
    {
    }

    public struct NewBubble : ISignal
    {
    }

    public struct BubbleComplete: ISignal
    {
        public int ObjectID;
        public int BubbleScore;
    }

    public struct LoadLevel : ISignal
    {
    }

    public struct ConnectToStack : ISignal
    {
        public int ObjectID;
    }

    public struct UpdatePadlockScore : ISignal
    {
        public int AddedScore;
    }
}
