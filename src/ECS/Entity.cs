using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class Entity
    {
        private Dictionary<Type, Component> _components = new Dictionary<Type, Component>();

        #region Entity User Definable Methods

        /// <summary>
        /// The standard collision function for an Entity.
        /// Applies basic physics according to respective physicsComponents
        /// </summary>
        /// <param name="collidedWith"></param>
        public virtual void OnCollision(Entity collidedWith)
        {
            if (this.HasComponent<PhysicsComponent>() && collidedWith.HasComponent<PhysicsComponent>())
            {
                var thisPhysics = GetComponent<PhysicsComponent>();
                var thatPhysics = collidedWith.GetComponent<PhysicsComponent>();

                // This applies force to that.
                // Get vector this.Mid -> that.Mid
                // Apply force to that based on this.Velocity and MASS // TODO: Add mass.
                var forceDirection = Vector2.Normalize(collidedWith.Spatial.MidVector - this.Spatial.MidVector);
                thatPhysics.AddForce(forceDirection * thisPhysics.velocity * thisPhysics.mass); // F = MV (mass * velocity)
            }
        }


        public virtual void Update(GameTime gameTime) { }


        #endregion Entity User Definable Methods
        public Entity Copy()
        {
            var copy = (Entity)Activator.CreateInstance(this.GetType());
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
            writer.Write(this.GetType().Name);
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

        public bool Is<T>() where T : Entity
            => this.GetType() == typeof(T);

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

        public Component GetComponent(Type componentType)
        {
            if (_components.ContainsKey(componentType))
                return _components[componentType];
            return null;
        }

        public bool HasComponent<T>()
        {
            return _components.ContainsKey(typeof(T));
        }

        public bool HasComponent(Type t)
        {
            return _components.ContainsKey(t);
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

        public void RemoveComponent<T>()
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
            ECSCore.UpdateEntitySystemPlacement(this);
        }
    }
}
