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

    public struct LoadLevel : ISignal
    {
    }
}
