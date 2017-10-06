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
        private RectComponent r;

        public World()
        {
            r = new RectComponent();
            _gadgets.Add(new RectComponentPanel("Rectangle Editor", new Vector2(300, 200)));

            RectComponentPanel rep = _gadgets[0] as RectComponentPanel;

            rep.editing = r;
        }

        public void Update(GameTime gameTime)
        {
            GUIEventHub.OnGUIEvent();
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var g in _gadgets)
                g.Render(spriteBatch);

            spriteBatch.DrawString(Deft.Font12, "X " + r.rect.X.ToString(), new Vector2(700, 50), Color.Black);
            spriteBatch.DrawString(Deft.Font12, "Y " + r.rect.Y.ToString(), new Vector2(700, 80), Color.Black);
            spriteBatch.DrawString(Deft.Font12, "W " + r.rect.Width.ToString(), new Vector2(700, 110), Color.Black);
            spriteBatch.DrawString(Deft.Font12, "H " + r.rect.Height.ToString(), new Vector2(700, 140), Color.Black);

            spriteBatch.FillRectangle(r.rect, r.color);
        }
    }
}
