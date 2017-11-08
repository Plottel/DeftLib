using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public class ColorPanel : Panel
    {
        // Default constructor for reflection instantiation
        public ColorPanel() :
            this("", Vector2.Zero, new Vector2(IntBox.DEFAULT_WIDTH + 30, IntBox.DEFAULT_HEIGHT * 2 + PADDING_BETWEEN_GADGETS), 1)
        { }

        // Layer constructor for reflection instantiation
        public ColorPanel(int layer) :
            this("", Vector2.Zero, new Vector2(IntBox.DEFAULT_WIDTH + 30, IntBox.DEFAULT_HEIGHT * 2 + PADDING_BETWEEN_GADGETS), layer)
        {
        }

        public ColorPanel(string label, Vector2 pos, Vector2 size, int layer) : 
            base(label, pos, size, layer)
        {
            AddGadget<ByteBox>("R");
            AddGadget<ByteBox>("G");
            AddGadget<ByteBox>("B");
            AddGadget<ByteBox>("A");
        }

        public override void SyncGadget(object toAttach)
        {
            var c = (Color)toAttach;

            GetGadget<ByteBox>("R").SyncGadget(c.R);
            GetGadget<ByteBox>("G").SyncGadget(c.G);
            GetGadget<ByteBox>("B").SyncGadget(c.B);
            GetGadget<ByteBox>("A").SyncGadget(c.A);
        }

        public Color Value
        {
            get
            {
                return new Color
                    (
                        GetGadget<ByteBox>("R").Value,
                        GetGadget<ByteBox>("G").Value,
                        GetGadget<ByteBox>("B").Value,
                        GetGadget<ByteBox>("A").Value
                    );
            }
        }


    }
}
