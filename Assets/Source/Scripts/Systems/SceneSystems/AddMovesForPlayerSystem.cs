using Game.Data;
using Game.Signals;
using Zenject;

namespace Game.Systems
{
    public class AddMovesForPlayerSystem : IInitializable, IListener<LevelPartLoaded>, ILateDisposable
    {
        private RuntimeData _runtimeData;

        [Inject]
        public AddMovesForPlayerSystem(RuntimeData runtimeData)
        {
            _runtimeData = runtimeData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener<LevelPartLoaded>(this);
        }

        public void LateDispose()
        {
            TriggerListenerSystem.RemoveListener<LevelPartLoaded>(this);
        }

        void IListener<LevelPartLoaded>.Trigger(LevelPartLoaded signal)
        {
            _runtimeData.Player.TurnCount += signal.LevelPart.Turnes;

            TriggerListenerSystem.Trigger(new ShowRewardPanel()
            {
                MovesCount = signal.LevelPart.Turnes
            });
        }
    }
}
