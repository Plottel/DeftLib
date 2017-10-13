using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace DeftLib
{
    public class World
    {
        private List<Gadget> _gadgets = new List<Gadget>();

        private List<EntitySystem> _systems = new List<EntitySystem>();
        private List<Entity> _entities = new List<Entity>();

        private ulong _nextID = 1;

        public ulong NextEntityID
        {
            get { return _nextID++; }
        }

        EntityPanel entityPanel;
        Entity e;

        public World()
        {
            entityPanel = new EntityPanel("Entity Editor", new Vector2(10, 10), new Vector2(400, 200));

            _systems.Add(new RectangleRenderingSystem());

            e = new Entity();
            e.AddComponent<MovementComponent>(new MovementComponent());
            e.AddComponent<TestIntComponent>(new TestIntComponent());
            e.AddComponent<RectangleComponent>(new RectangleComponent());

            entityPanel.SetEntity(e);
            _gadgets.Add(entityPanel);
        }

        public void Update(GameTime gameTime)
        {
            foreach (EntitySystem s in _systems)
                s.Process();


            GUIEventHub.OnGUIEvent();
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var g in _gadgets)
                g.Render(spriteBatch);

            Gadget active = GUIEventHub.ActiveListener as Gadget;

            if (active != null)
                spriteBatch.DrawRectangle(active.pos, active.size, Color.Blue, 2);
        }
    }
}
