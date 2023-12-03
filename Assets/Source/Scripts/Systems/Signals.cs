using Game.MonoBehaviours;
using System;

namespace Game.Signals
{
    public interface ISignal
    {
    }

    public struct RegisterEnemy : ISignal
    {

    }

    public struct SetGameState : ISignal
    {
        public GameState GameState;
    }

    public struct ChangeGameState : ISignal
    {
        public GameState GameState;
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

    public struct LevelPartIsCancelled : ISignal
    {
        public LevelPart LevelPart;
    }

    public struct UpdatePadlockScore : ISignal
    {
        public int AddedScore;
    }
    
    public struct NextLevelPart : ISignal
    {
    }

    public struct LevelPartLoaded : ISignal
    {
        public LevelPart LevelPart;
    }

    public struct LevelMoveToCamera : ISignal
    {
        public LevelMono LevelMono;
        public Action OnEndAction;
    }

    public struct Lose : ISignal
    {
    }

    public struct ShowRewardPanel : ISignal
    {
        public int MovesCount;
    }
}
