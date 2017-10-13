using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeftLib
{
    public class Entity
    {
        private Dictionary<Type, IComponent> _components = new Dictionary<Type, IComponent>();

        public T GetComponent<T>() where T : IComponent
        {
            var type = typeof(T);

            if (_components.ContainsKey(type))
                return (T)_components[type];
            return default(T);
        }

        public void ReplaceComponent(IComponent component)
        {
            if (_components.ContainsKey(component.GetType()))
                _components[component.GetType()] = component;
        }

        public void AddComponent<T>(IComponent component)
        {
            _components.Add(typeof(T), component);
        }

        public Dictionary<Type, IComponent> ComponentMap
        {
            get { return _components; }
        }

        public List<IComponent> ComponentList
        {
            get { return _components.Values.ToList(); }
        }
    }
}
