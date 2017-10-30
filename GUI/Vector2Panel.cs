using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class Vector2Panel : Panel
    {
        private FloatBox _xBox;
        private FloatBox _yBox;

        // Default constructor for reflection instantiation
        public Vector2Panel() : 
            this("", Vector2.Zero, new Vector2(IntBox.DEFAULT_WIDTH + 30, IntBox.DEFAULT_HEIGHT * 2 + PADDING_BETWEEN_GADGETS), 1)
        { }

        // Layer constructor for reflection instantiation
        public Vector2Panel(int layer) : 
            this("", Vector2.Zero, new Vector2(IntBox.DEFAULT_WIDTH + 30, IntBox.DEFAULT_HEIGHT * 2 + PADDING_BETWEEN_GADGETS), layer)
        { }

        public Vector2Panel(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        {
            AddGadget<FloatBox>("X");
            AddGadget<FloatBox>("Y");

            _xBox = GetGadget<FloatBox>("X");
            _yBox = GetGadget<FloatBox>("Y");
        }

        public override void SyncGadget(object toAttach)
        {
            var v2 = (Vector2)toAttach;

            _xBox.SyncGadget(v2.X);
            _yBox.SyncGadget(v2.Y);
        }

        public Vector2 Value
        {
            get { return new Vector2(_xBox.Value, _yBox.Value); }
        }

    }
}
