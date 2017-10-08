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
            gadgetTypeMap[typeof(Color)] = typeof(ColorPanel);
            gadgetTypeMap[typeof(Rectangle)] = typeof(RectanglePanel);
            gadgetTypeMap[typeof(Vector2)] = typeof(VectorPanel);
        }

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

        public virtual void MoveBy(Vector2 amt)
            => pos += amt;

        public abstract void OnGUIEvent();
        public abstract void Render(SpriteBatch spriteBatch);
    }
}
