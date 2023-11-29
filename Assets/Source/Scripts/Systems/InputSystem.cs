using Game.Data;
using Game.Signals;
using UnityEngine;
using Zenject;

namespace Game.Systems
{
    public sealed class InputSystem : ITickable
    {
        private readonly RuntimeData _runtimeData;

        [Inject]
        public InputSystem(RuntimeData data)
        {
            _runtimeData = data;
        }

        public void Tick()
        {
            _runtimeData.MousePosition = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                TriggerListenerSystem.Trigger<MouseDown>();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                TriggerListenerSystem.Trigger<MouseUp>();
            }
        }
    }
}