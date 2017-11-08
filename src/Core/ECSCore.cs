using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public static class ECSCore
    {
        private static List<EntitySystem> _entitySystems;
        private static List<EntitySystem> _renderingEntitySystems;

        static ECSCore()
        {
            _entitySystems = new List<EntitySystem>();
            _renderingEntitySystems = new List<EntitySystem>();


            SubscribeSystem(new PhysicsSystem());
            SubscribeSystem(new CollisionSystem());
            SubscribeSystem(new TextureRenderingSystem());
            SubscribeSystem(new RectangleRenderingSystem());
        }

        public static void SubscribeEntity(Entity e)
        {
            if (e == null) return;

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

        public static void UpdateEntitySystemPlacement(Entity e)
        {
            if (e == null) return;

            foreach (var system in _entitySystems)
            {
                if (system.HasEntity(e))
                {
                    if (!system.CanOperateOnEntity(e))
                        system.UnsubscribeEntity(e);
                }
                else if (system.CanOperateOnEntity(e))
                    system.SubscribeEntity(e);
            }

            foreach (var system in _renderingEntitySystems)
            {
                if (system.HasEntity(e))
                {
                    if (!system.CanOperateOnEntity(e))
                        system.UnsubscribeEntity(e);
                }
                else if (system.CanOperateOnEntity(e))
                    system.SubscribeEntity(e);
            }
        }

        public static void UnsubscribeEntity(Entity subscriber)
        {
            if (subscriber == null) return;

            foreach (var system in _entitySystems)
                system.UnsubscribeEntity(subscriber);
            foreach (var system in _renderingEntitySystems)
                system.UnsubscribeEntity(subscriber);
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

        public static void Update(GameTime gameTime)
        {
            foreach (var s in _entitySystems)
                s.Process();
        }

        public static void Render(SpriteBatch spriteBatch)
        {
            foreach (var s in _renderingEntitySystems)
                s.Process();

            // Find collision system, print how may subscribed entities
            var colSys = _entitySystems.Find(s => s.GetType() == typeof(CollisionSystem));
        }
    }
}
