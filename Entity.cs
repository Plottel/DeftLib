using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeftLib
{
    public class Entity
    {
        private Dictionary<Type, Component> _components = new Dictionary<Type, Component>();

        public T GetComponent<T>() where T : Component
        {
            var type = typeof(T);

            if (_components.ContainsKey(type))
                return (T)_components[type];
            return null;
        }

        public void ReplaceComponent(Component component)
        {
            if (_components.ContainsKey(component.GetType()))
                _components[component.GetType()] = component;
        }

        public void AddComponent<T>(Component component)
        {
            _components.Add(typeof(T), component);
        }

        public Dictionary<Type, Component> ComponentMap
        {
            get { return _components; }
        }

        public List<Component> ComponentList
        {
            get { return _components.Values.ToList(); }
        }
    }
}
