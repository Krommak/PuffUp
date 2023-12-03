using Game.Data;
using Game.Signals;
using UnityEngine;
using Zenject;
using Zlodey;

namespace Game.Systems
{
    public class LevelInitializeSystem : IInitializable, IListener<LevelPartLoaded>
    {
        private RuntimeData _runtimeData;

        [Inject]
        public LevelInitializeSystem(RuntimeData runtimeData)
        {
            _runtimeData = runtimeData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener<LevelPartLoaded>(this);
        }

        void IListener<LevelPartLoaded>.Trigger(LevelPartLoaded loadLevel)
        {
            var level = _runtimeData.LevelData;

            foreach (var render in loadLevel.LevelPart.Obstacles)
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
