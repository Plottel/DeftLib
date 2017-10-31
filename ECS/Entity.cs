using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DeftLib
{
    public class Entity
    {
        private Dictionary<Type, Component> _components = new Dictionary<Type, Component>();

        public Entity Copy()
        {
            var copy = new Entity();
            foreach (var component in ComponentList)
                copy.AddComponent(component.Copy());
            return copy;
        }

        /// <summary>
        /// Serializes all components into a BinaryWriter
        /// </summary>
        /// <param name="writer"></param>
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(_components.Count);

            foreach (var componentKVP in _components)
            {
                string typeString = componentKVP.Value.GetType().Name;
                writer.Write(typeString);
                componentKVP.Value.Serialize(writer);
            }
        }

        /// <summary>
        /// Deserializes all components from a BinaryReader
        /// </summary>
        /// <param name="reader"></param>
        public void Deserialize(BinaryReader reader)
        {
            int numComponents = reader.ReadInt32();

            for (int i = 0; i < numComponents; ++i)
            {
                string typeString = reader.ReadString();
                string fullTypeString = "DeftLib." + typeString;
                Type componentType = Type.GetType(fullTypeString);
                var newComponent = (Component)Activator.CreateInstance(componentType);
                newComponent.Deserialize(reader);
                AddComponent(newComponent);
            }
        }

        // TODO: Fix. This should not be a property with search logic, it's going to be used a lot.
        public SpatialComponent Spatial
        {
            get
            {
                if (HasComponent<SpatialComponent>())
                    return GetComponent<SpatialComponent>();
                return null;
            }
        }

        public Dictionary<Type, Component> ComponentMap
        {
            get { return _components; }
        }

        public List<Component> ComponentList
        {
            get { return _components.Values.ToList(); }
        }

        public T GetComponent<T>() where T : Component
        {
            var type = typeof(T);

            if (_components.ContainsKey(type))
                return (T)_components[type];
            return default(T);
        }

        public bool HasComponent<T>()
        {
            return _components.ContainsKey(typeof(T));
        }

        public bool HasComponent(Type t)
        {
            if (_components.ContainsKey(t))
                return true;

            // If dont have exact one, check if subclass.
            // TODO: Rethink the whole system here.
            foreach (var c in _components)
            {
                if (t.IsAssignableFrom(c.Key))
                    return true;
            }

            return false;
        }

        public void ReplaceComponent(Component component)
        {
            if (_components.ContainsKey(component.GetType()))
            {
                _components[component.GetType()] = component;
                component.owner = this;
            }
                
        }

        public void AddComponent<T>(Component component) where T : Component
        {
            _components.Add(typeof(T), component);
            component.owner = this;
            UpdateEntitySystemPlacementAfterComponentChange();
        }

        public void AddComponent(Component component)
        {
            _components[component.GetType()] = component;
            component.owner = this;
            UpdateEntitySystemPlacementAfterComponentChange();
        }

        public void AddComponent<T>() where T : Component
        {
            _components[typeof(T)] = (T)Activator.CreateInstance(typeof(T));
            _components[typeof(T)].owner = this;
        }

        public void RemoveComponent<T>(Component component)
        {
            var componentType = typeof(T);

            if (_components.ContainsKey(componentType))
            {
                _components.Remove(componentType);
                UpdateEntitySystemPlacementAfterComponentChange();
            }
        }

        public void RemoveComponent(Component component)
        {
            var componentType = component.GetType();

            if (_components.ContainsKey(componentType))
            {
                _components.Remove(componentType);
                UpdateEntitySystemPlacementAfterComponentChange();
            }
        }

        private void UpdateEntitySystemPlacementAfterComponentChange()
        {
            ECSCore.PlaceEntityInSystems(this);
        }
    }
}
