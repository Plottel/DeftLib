using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class FloatBox : InputBox<float>
    {
        // Default constructor for reflection instantiation
        public FloatBox() :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instantiation
        public FloatBox(int layer) :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public FloatBox(string label, Vector2 pos, Vector2 size, int layer) :
            base(label, pos, size, layer)
        { }

        public override void SyncGadget(object toAttach)
        {
            float f = (float)toAttach;
            Text = f.ToString();
        }

        public override float Value
        {
            get
            {
                float result;

                if (float.TryParse(_text, out result))
                    return result;
                return 0f;
            }
        }
    }
}
