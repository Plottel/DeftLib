using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    class IntBox : InputBox
    {
        public static Vector2 DEFAULT_SIZE = new Vector2(200, 30);

        // Default constructor for Reflection instantiation
        public IntBox() : this("", Vector2.Zero, DEFAULT_SIZE)
        { }

        public IntBox(string label, Vector2 pos) : this(label, pos, DEFAULT_SIZE)
        { }

        public IntBox(string label, Vector2 pos, Vector2 size) : base(label, pos, size)
        { }

        public int Value
        {
            get
            {
                int result;                

                if (Int32.TryParse(text, out result))
                    return result;
                return 0;
            }
        }
    }
}