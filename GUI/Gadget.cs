using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public abstract class Gadget
    {
        public int layer;
        public Vector2 pos;
        public Vector2 size;
        public string label;

        public virtual void SetLayer(int newLayer)
            => layer = newLayer;

        public Rectangle Bounds
        {
            get { return new Rectangle(pos.ToPoint(), size.ToPoint()); }
        }

        // Default constructor for reflection instantiation
        public Gadget() : 
            this("", Vector2.Zero, Vector2.Zero, 1)
        { }

        // Layer constructor for reflection instantiation
        public Gadget(int layer) : 
            this("", Vector2.Zero, Vector2.Zero, layer)
        { }

        public Gadget(string label, Vector2 pos, Vector2 size, int layer)
        {
            this.layer = layer;
            this.pos = pos;
            this.size = size;
            this.label = label;
        }

        /// <summary>
        /// Updates the current properties of the gadget to match
        /// the passed in object.
        /// </summary>
        public abstract void SyncGadget(object toAttach);

        public virtual void MoveTo(Vector2 newPos)
            => pos = newPos;

        public virtual void MoveBy(Vector2 amt)
            => pos += amt;

        public abstract void OnGUIEvent();
        public abstract void Render(SpriteBatch spriteBatch);
    }
}
