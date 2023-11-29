using Game.Data;
using Game.Signals;
using UnityEngine;
using Zenject;

namespace Game.Systems
{
    public class CreateBubbleSystem : IListener<MouseDown>, IInitializable
    {
        private RuntimeData _runtimeData;

        [Inject]
        public CreateBubbleSystem(RuntimeData runtimeData)
        {
            _runtimeData = runtimeData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener<MouseDown>(this);
        }

        void IListener<MouseDown>.Trigger<MouseDown>()
        {
            //if (_runtimeData.GameState != GameState.Playing) return;

            var positionWorld = Camera.main.ScreenToWorldPoint(_runtimeData.MousePosition);

            var level = _runtimeData.LevelData.ClampSize;

            if (positionWorld.y > level.y)
            {
                return;
            }

            //LayerMask mask = LayerMask.GetMask("Obstacle");
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

            if (_runtimeData.TurnCount == 0)
            {
                return;
            }

            _runtimeData.TurnCount--;
            positionWorld.z = 5f;

            //var bubbleActor = Object.Instantiate(_staticData.PrefabBubble, positionWorld, Quaternion.identity);

            //bubbleActor.Init();
        }
    }
}
