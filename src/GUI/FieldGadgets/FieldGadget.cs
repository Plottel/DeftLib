using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public abstract class FieldGadget<T> : Gadget
    {
        // Default constructor for reflection instantiation
        public FieldGadget() : 
            this("", Vector2.Zero, Vector2.Zero, 1)
        { }

        // Layer constructor for reflection instantiation
        public FieldGadget(int layer) : 
            this("", Vector2.Zero, Vector2.Zero, layer)
        { }

        public FieldGadget(string label, Vector2 pos, Vector2 size, int layer)
        {
            this.layer = layer;
            this.pos = pos;
            this.size = size;
            this.label = label;
        }

        /// <summary>
        /// All field gadgets must be able to return a T.
        /// </summary>
        public abstract T Value
        {
            get;
        }
    }
}
