using Game.Signals;
using Zenject;

namespace Game.Systems
{
    public class GameStateSystem : IInitializable, IListener<SetGameState>
    {
        private GameState CurrentState = GameState.LoadingStart;

        public void Initialize()
        {
            TriggerListenerSystem.AddListener(this);
            TriggerListenerSystem.Trigger(new ChangeGameState()
            {
                GameState = CurrentState
            });
        }

        void IListener<SetGameState>.Trigger(SetGameState signal)
        {
            CurrentState = signal.GameState;

            TriggerListenerSystem.Trigger(new ChangeGameState()
            {
                GameState = CurrentState
            });
        }
    }
}