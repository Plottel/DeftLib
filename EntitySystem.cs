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

        public abstract void Process();
    }
}
