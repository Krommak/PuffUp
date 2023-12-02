using Game.Data;
using Game.Signals;
using UnityEngine;
using Zenject;

namespace Game.Systems
{
    public class CreateBubbleSystem : IListener<MouseDown>, IInitializable, ILateDisposable
    {
        private RuntimeData _runtimeData;
        private StaticData _staticData;

        [Inject]
        public CreateBubbleSystem(RuntimeData runtimeData, StaticData staticData)
        {
            _runtimeData = runtimeData;
            _staticData = staticData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener<MouseDown>(this);
        }

        public void LateDispose()
        {
            TriggerListenerSystem.RemoveListener<MouseDown>(this);
        }

        void IListener<MouseDown>.Trigger(MouseDown down)
        {
            //if (_runtimeData.GameState != GameState.Playing) return;

            if (_runtimeData.Player.TurnCount == 0)
            {
                return;
            }

            var positionWorld = _runtimeData.MousePosition;
            positionWorld.z = 10;
            positionWorld = Camera.main.ScreenToWorldPoint(positionWorld);

            var level = _runtimeData.LevelData.ClampSize;

            if (positionWorld.y > level.y)
            {
                return;
            }

            var hits = Physics2D.OverlapCircleAll(positionWorld, 0.1f);

            if (hits.Length > 0)
            {
                foreach (var collider2D in hits)
                {
                    var offset = collider2D.ClosestPoint(positionWorld) - (Vector2)positionWorld;

                    positionWorld = offset;
                }

                var hits2 = Physics2D.OverlapCircleAll(positionWorld, 0.2f);

                if (hits2.Length > 0)
                {
                    return;
                }
            }

            _runtimeData.Player.TurnCount--;

            var newBubble = Object.Instantiate(_staticData.PrefabBubble, positionWorld, Quaternion.identity);
            TriggerListenerSystem.Trigger(new NewBubble()
            {
                CreatedBubble = newBubble,
            });
        }
    }
}
