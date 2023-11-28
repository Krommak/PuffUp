using System;
using System.Collections.Generic;

namespace Game.Systems
{
    public class SystemsContainer : IDisposable
    {
        private List<BaseSystem> _systems;

        public SystemsContainer()
        {
            _systems = new List<BaseSystem>();
        }

        public SystemsContainer Add(BaseSystem system)
        {
            _systems.Add(system);
            return this;
        }

        public void UpdateSystems()
        {
            foreach (var item in _systems)
            {
                item.Update();
            }
        }

        public void Dispose()
        {
            _systems.ForEach(x => x.Dispose());
        }
    }
}