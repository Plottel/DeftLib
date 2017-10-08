using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace DeftLib
{
    public class World
    {
        private List<Gadget> _gadgets = new List<Gadget>();
        EntityPanel entityPanel;
        Entity e;

        public World()
        {
            entityPanel = new EntityPanel("Entity Editor", new Vector2(10, 10), new Vector2(400, 200));

            e = new Entity();
            e.AddComponent<MovementComponent>(new MovementComponent());
            e.AddComponent<TestIntComponent>(new TestIntComponent());
            e.AddComponent<RectangleComponent>(new RectangleComponent());

            entityPanel.SetEntity(e);
        }

        public void Update(GameTime gameTime)
        {
            GUIEventHub.OnGUIEvent();
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var g in _gadgets)
                g.Render(spriteBatch);


            entityPanel.Render(spriteBatch); // Didn't add it to gadget list.
        }
    }
}
