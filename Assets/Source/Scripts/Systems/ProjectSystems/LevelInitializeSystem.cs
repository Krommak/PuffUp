using Game.Data;
using Game.Signals;
using UnityEngine;
using Zenject;
using Zlodey;

namespace Game.Systems
{
    public class LevelInitializeSystem : IInitializable, IListener<LevelLoaded>
    {
        private RuntimeData _runtimeData;

        [Inject]
        public LevelInitializeSystem(RuntimeData runtimeData)
        {
            _runtimeData = runtimeData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener<LevelLoaded>(this);
        }

        void IListener<LevelLoaded>.Trigger(LevelLoaded loadLevel)
        {
            var level = _runtimeData.LevelData;
            var levelMono = _runtimeData.LoadedLevel;

            foreach (var render in levelMono.Obstacles)
            {
                render.GetPropertyBlock(_runtimeData.Block);

                var color = level.ColorScheme.GetColor(render.gameObject.layer);

                _runtimeData.Block.SetColor(ShaderConst.Color, color.BaseColor);
                _runtimeData.Block.SetColor(ShaderConst.ShadowColor, color.ShadowColor);

                render.SetPropertyBlock(_runtimeData.Block);
            }

            Camera.main.backgroundColor = level.ColorScheme.GetColor(LayerMask.NameToLayer("Background")).BaseColor;
        }
    }
}
