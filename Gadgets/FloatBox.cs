using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    class FloatBox : InputBox
    {
        public static Vector2 DEFAULT_SIZE = new Vector2(200, 30);

        // Default constructor for Reflection instantiation
        public FloatBox() : this("", Vector2.Zero, DEFAULT_SIZE, 1)
        { }

        public FloatBox(int layer) : this("", Vector2.Zero, DEFAULT_SIZE, layer)
        { }
                 
        public FloatBox(string label, Vector2 pos) : this(label, pos, DEFAULT_SIZE, 1)
        { }    
                 
        public FloatBox(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        { }

        public float Value
        {
            get
            {
                float result;

                if (float.TryParse(text, out result))
                    return result;
                return 0f;
            }
        }
    }
}