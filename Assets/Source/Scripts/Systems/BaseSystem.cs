using System;

namespace Game.Systems
{
    public class BaseSystem : IDisposable, IUpdateSystem
    {
        public virtual void Dispose()
        {
        }

        public virtual void Update()
        {
        }
    }
}
