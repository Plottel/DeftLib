using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public abstract class Gadget : GUIEventListener
    {
        public Vector2 pos;
        public Vector2 size;
        public string label;

        public Gadget(string label, Vector2 pos, Vector2 size)
        {
            this.pos = pos;
            this.size = size;
            this.label = label;

            GUIEventHub.Subscribe(this);
        }

        public abstract void OnGUIEvent();
        public abstract void Render(SpriteBatch spriteBatch);
    }
}
