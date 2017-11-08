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
        protected List<Entity> Entities { get => _entities; set => _entities = value; }

        public int EntityCount
        {
            get { return _entities.Count; }
        }

        public EntitySystem(params Type[] requiredComponents)
            => _requiredComponents = requiredComponents;

        public bool HasEntity(Entity e)
            => _entities.Contains(e);

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
            if (e == null) return;

            if (!_entities.Contains(e)) // Prevent aliasing
                _entities.Add(e);
        }

        public void UnsubscribeEntity(Entity e)
            => _entities.RemoveAll(entity => entity == e); // Prevent aliasing

        public abstract void Process();
    }
}
