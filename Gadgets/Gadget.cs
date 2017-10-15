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
        public static Dictionary<Type, Type> gadgetTypeMap = 
            new Dictionary<Type, Type>();

        static Gadget()
        {
            gadgetTypeMap[typeof(int)] = typeof(IntBox);
            gadgetTypeMap[typeof(float)] = typeof(FloatBox);
            gadgetTypeMap[typeof(Color)] = typeof(ColorPanel);
            gadgetTypeMap[typeof(Vector2)] = typeof(VectorPanel);
            gadgetTypeMap[typeof(Rectangle)] = typeof(RectanglePanel);
        }

        public int Layer { get; set; }
        public Vector2 pos;
        public Vector2 size;
        public string label;

        public Rectangle Bounds
        {
            get { return new Rectangle(pos.ToPoint(), size.ToPoint()); }
        }

        // Default constructor for Reflection instantiation
        public Gadget() : this("", Vector2.Zero, Vector2.Zero, 1)
        { }

        // Layer constructor for Reflection instantiation
        public Gadget(int layer) : this("", Vector2.Zero, Vector2.Zero, layer)
        { }

        public Gadget(string label, Vector2 pos, Vector2 size, int layer)
        {
            this.Layer = layer;
            this.pos = pos;
            this.size = size;
            this.label = label;

            GUIEventHub.Subscribe(this);
        }

        public virtual void SetGadgetVariable(object newValue) { }

        public virtual void MoveTo(Vector2 newPos)
            => pos = newPos;

        public virtual void MoveBy(Vector2 amt)
            => pos += amt;

        public abstract void OnGUIEvent();
        public abstract void Render(SpriteBatch spriteBatch);
    }
}
