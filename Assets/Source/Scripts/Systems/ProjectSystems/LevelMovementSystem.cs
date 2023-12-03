using DG.Tweening;
using Game.Signals;
using Zenject;

namespace Game.Systems
{
    public class LevelMovementSystem : IInitializable,
        IListener<LevelMoveToCamera>, ILateDisposable
    {
        public void Initialize()
        {
            TriggerListenerSystem.AddListener<LevelMoveToCamera>(this);
        }

        public void LateDispose()
        {
            TriggerListenerSystem.RemoveListener<LevelMoveToCamera>(this);
        }

        void IListener<LevelMoveToCamera>.Trigger(LevelMoveToCamera signal)
        {
            var transform = signal.LevelMono.transform;
            var step = signal.LevelMono.LevelPartStepY;

            var target = -step * signal.LevelMono.CurrentPart;

            var duration = target == 0 ? 0 : signal.LevelMono.Duration;

            TriggerListenerSystem.Trigger(new SetGameState()
            {
                GameState = GameState.LevelPartTransitionStart
            });

            transform.DOMoveY(target, duration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    signal.OnEndAction?.Invoke();
                    TriggerListenerSystem.Trigger(new SetGameState()
                    {
                        GameState = GameState.LevelPartTransitionEnd,
                    });
                });
        }
    }
}
