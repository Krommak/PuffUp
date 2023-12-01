using Game.Data;
using Game.Signals;
using Zenject;
using Zlodey;
using Game.Extentions;

namespace Game.Systems
{
    public class SetBubbleColorSystem : IListener<NewBubble>, IListener<BubbleComplete>, IInitializable
    {
        private RuntimeData _runtimeData;
        private StaticData _staticData;

        [Inject]
        public SetBubbleColorSystem(RuntimeData runtimeData, StaticData staticData)
        {
            _runtimeData = runtimeData;
            _staticData = staticData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener<NewBubble>(this);
            TriggerListenerSystem.AddListener<BubbleComplete>(this);
        }

        void IListener<BubbleComplete>.Trigger(BubbleComplete bubbleComplete)
        {
            var text = _runtimeData.ScaledBubble.ScoreText;
            var renderer = _runtimeData.ScaledBubble.Renderer;
            var block = _runtimeData.Block;

            renderer.GetPropertyBlock(block);

            block.SetColor(ShaderConst.Color, _runtimeData.ScaledBubble.Color.BaseColor);
            block.SetColor(ShaderConst.ShadowColor, _runtimeData.ScaledBubble.Color.ShadowColor);

            renderer.SetPropertyBlock(block);

            text.alpha = 1f;
        }

        void IListener<NewBubble>.Trigger(NewBubble newBubble)
        {
            var renderer = _runtimeData.ScaledBubble.Renderer;
            var color = _runtimeData.ScaledBubble.Color = _staticData.Colors.RandomElement();
            var block = _runtimeData.Block;
            renderer.material = _staticData.TransparentMaterial;

            renderer.GetPropertyBlock(block);

            var a = color.BaseColor;
            a.a = 0.3f;

            block.SetColor(ShaderConst.Color, a);
            block.SetColor(ShaderConst.ShadowColor, color.ShadowColor);

            renderer.SetPropertyBlock(block);

            foreach (var system in _runtimeData.ScaledBubble.Particle)
            {
                var main = system.main;
                main.startColor = color.BaseColor;
            }
        }
    }
}
