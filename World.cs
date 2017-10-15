using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.Reflection;
using System.IO;

namespace DeftLib
{
    public static class World
    {
        private static List<Gadget> _gadgets = new List<Gadget>();
        private static List<EntitySystem> _entitySystems = new List<EntitySystem>();
        private static List<EntitySystem> _renderingEntitySystems = new List<EntitySystem>();
        private static List<Entity> _entities = new List<Entity>();
        private static Dictionary<string, List<Component>> _savedEntityTypes;

        private static EntityPanel _entityPanel;
        private static Entity _selectedEntity;

        private static ulong _nextID = 1;

        public static ulong NextEntityID
        {
            get { return _nextID++; }
        }

        public static void LoadWorld()
        {
            using (BinaryReader reader = new BinaryReader(File.Open("world.bin", FileMode.Open)))
            {
                int numEntities = reader.ReadInt32();

                for (int i = 0; i < numEntities; ++i)
                {
                    Entity newEntity = new Entity();
                    newEntity.Deserialize(reader);
                    _entities.Add(newEntity);
                }
            }
        }

        public static void SaveWorld()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("world.bin", FileMode.OpenOrCreate)))
            {
                writer.Write(_entities.Count);

                foreach (Entity e in _entities)
                    e.Serialize(writer);
            }
        }

        static World()
        {
            _entityPanel = new EntityPanel("Entity Viewer", new Vector2(10, 10), new Vector2(400, 200));
            _savedEntityTypes = new Dictionary<string, List<Component>>();
            _gadgets.Add(_entityPanel);

            SubscribeSystem(new RectangleRenderingSystem());
            SubscribeSystem(new MovementSystem());

            string identifier = "DefaultEntity";
            var spatial = new SpatialComponent { pos = new Vector2(300, 300), size = new Vector2(50, 50)};

            var components = new List<Component>
            {
                spatial,
            };

            _savedEntityTypes[identifier] = components;
        }

        public static Entity Create(string entityType, Vector2 pos)
        {
            Entity newEntity = new Entity();

            if (_savedEntityTypes.ContainsKey(entityType))
            {
                foreach (var component in _savedEntityTypes[entityType])
                {
                    var cType = component.GetType();
                    var newComponent = component.Copy();

                    newEntity.AddComponent(newComponent);
                }                    
            }

            newEntity.GetComponent<SpatialComponent>().pos = pos;

            _entities.Add(newEntity);
            PlaceEntityInSystems(newEntity);

            return newEntity;            
        }

        public static void PlaceEntityInSystems(Entity e)
        {
            foreach (var system in _entitySystems)
            {
                if (system.CanOperateOnEntity(e))
                    system.SubscribeEntity(e);
            }

            // Something here to EXCLUDE entities from the RenderingSystem check if they dont have any components which are renderable
            foreach (var system in _renderingEntitySystems)
            {
                if (system.CanOperateOnEntity(e))
                    system.SubscribeEntity(e);
            }
        }

        public static void SubscribeSystem(EntitySystem subscriber)
        {
            if (subscriber is IRenderingSystem)
                _renderingEntitySystems.Add(subscriber);
            else
                _entitySystems.Add(subscriber);
        }

        public static void UnsubscribeSystem(EntitySystem subscriber)
        {
            if (subscriber is IRenderingSystem)
                _renderingEntitySystems.Remove(subscriber);
            else
                _entitySystems.Remove(subscriber);
        }

        public static void HandleInput()
        {
            if (Input.KeyTyped(Keys.S))
                SaveWorld();
            if (Input.KeyTyped(Keys.L))
                LoadWorld();


            if (Input.KeyTyped(Keys.F1))
                Create("DefaultEntity", new Vector2(700 + (_entities.Count * 10), 300));

            // Select Entity and update EntityPanel on left mouse click.
            if (Input.LeftMouseClicked())
            {
                Vector2 mousePos = Input.MousePos;

                foreach (Entity e in _entities)
                {
                    if (e.Spatial.Bounds.Contains(mousePos))
                    {
                        _selectedEntity = e;
                        _entityPanel.SetEntity(e);
                        break;
                    }
                }
            }

            if (Input.KeyTyped(Keys.D))
            {
                Entity e = _entities[0];
                var spatial = e.GetComponent<SpatialComponent>();
                spatial.pos += new Vector2(20, 20);
            }
        }

        public static void Update(GameTime gameTime)
        {
            foreach (EntitySystem s in _entitySystems)
                s.Process();

            GUIEventHub.OnGUIEvent();
        }

        public static void Render(SpriteBatch spriteBatch)
        {
            foreach (EntitySystem s in _renderingEntitySystems)
                s.Process();

            if (_selectedEntity != null)
                spriteBatch.DrawRectangle(_selectedEntity.Spatial.Bounds.GetInflated(1, 1), Color.Green, 2);

            foreach (var g in _gadgets)
                g.Render(spriteBatch);

            if (GUIEventHub.ActiveListener != null)
                spriteBatch.DrawRectangle(GUIEventHub.ActiveListener.Bounds, Color.Blue, 2);
        }
    }
}
