using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeftLib
{
    public abstract class EntitySystem
    {
        private Type[] _requiredComponents;
        protected List<Entity> _entities = new List<Entity>();

        public EntitySystem(params Type[] requiredComponents)
            => _requiredComponents = requiredComponents;

        public bool CanOperateOnEntity(Entity e)
        {
            foreach (Type t in _requiredComponents)
            {
                if (!e.HasComponent(t))
                    return false;     
            }

            return true;
        }

        public void SubscribeEntity(Entity e)
        {
            if (!_entities.Contains(e))
                _entities.Add(e);
        }

        public void UnsubscribeEntity(Entity e)
            => _entities.Remove(e);        

        public abstract void Process();
    }
}
