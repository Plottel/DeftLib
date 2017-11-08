using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class ByteBox : InputBox<byte>
    {
        // Default constructor for reflection instantiation
        public ByteBox() :
            this("", Vector2.Zero, Vector2.Zero, 1)
        { }

        // Layer constructor for reflection instantiation
        public ByteBox(int layer) :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public ByteBox(string label, Vector2 pos, Vector2 size, int layer) :
            base(label, pos, size, layer)
        {
        }

        public override void SyncGadget(object toAttach)
        {
            byte b = (byte)toAttach;
            Text = b.ToString();
        }

        public override byte Value
        {
            get
            {
                byte result;

                if (byte.TryParse(_text, out result))
                    return result;
                return 0;
            }
        }
    }
}
