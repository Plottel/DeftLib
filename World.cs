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
        ComponentPanel<TestIntComponent> testPanel;
        TestIntComponent testComp;

        public World()
        {
            testComp = new TestIntComponent();
            testPanel = new ComponentPanel<TestIntComponent>("RECTANGLE COMPONENT", new Vector2(50, 50), new Vector2(300, 50));
            testPanel.editing = testComp;


            //r = new RectComponent();
            //_gadgets.Add(new RectComponentPanel("Rectangle Editor", new Vector2(300, 200)));

            //RectComponentPanel rep = _gadgets[0] as RectComponentPanel;

            //rep.editing = r;
        }

        public void Update(GameTime gameTime)
        {
            GUIEventHub.OnGUIEvent();
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var g in _gadgets)
                g.Render(spriteBatch);


            testPanel.Render(spriteBatch);

            //spriteBatch.FillRectangle(new Rectangle(testComp.x, testComp.y, testComp.w, testComp.h), Color.Blue);
            spriteBatch.FillRectangle(testComp.rect, testComp.color);

            //spriteBatch.FillRectangle(r.rect, r.color);
        }
    }
}
