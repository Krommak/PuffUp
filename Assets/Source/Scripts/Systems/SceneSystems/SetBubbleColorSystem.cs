using Game.Data;
using Game.Signals;
using Zenject;
using Zlodey;

namespace Game.Systems
{
    public class SetBubbleColorSystem : 
        IListener<NewBubble>, IListener<BubbleComplete>,
        IInitializable, ILateDisposable
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

        public void LateDispose()
        {
            TriggerListenerSystem.RemoveListener<NewBubble>(this);
            TriggerListenerSystem.RemoveListener<BubbleComplete>(this);
        }

        void IListener<BubbleComplete>.Trigger(BubbleComplete bubbleComplete)
        {
            var text = bubbleComplete.CompletedBubble.ScoreText;
            var renderer = bubbleComplete.CompletedBubble.Renderer;
            var block = _runtimeData.Block;

            renderer.GetPropertyBlock(block);
            
            bubbleComplete.CompletedBubble.Color.BaseColor.a = 1f;

            block.SetColor(ShaderConst.Color, bubbleComplete.CompletedBubble.Color.BaseColor);
            block.SetColor(ShaderConst.ShadowColor, bubbleComplete.CompletedBubble.Color.ShadowColor);

            renderer.SetPropertyBlock(block);

            text.alpha = 1f;
        }

        void IListener<NewBubble>.Trigger(NewBubble newBubble)
        {
            var renderer = newBubble.CreatedBubble.Renderer;
            
            var color 
                = newBubble.CreatedBubble.Color 
                = _runtimeData.LevelData.ColorScheme.GetColor(renderer.gameObject.layer);
            
            var block = _runtimeData.Block;
            renderer.material = _staticData.TransparentMaterial;

            renderer.GetPropertyBlock(block);

            var a = color.BaseColor;
            a.a = 0.3f;

            block.SetColor(ShaderConst.Color, a);
            block.SetColor(ShaderConst.ShadowColor, color.ShadowColor);

            renderer.SetPropertyBlock(block);

            foreach (var system in newBubble.CreatedBubble.Particle)
            {
                var main = system.main;
                main.startColor = color.BaseColor;
            }
        }
    }
}
