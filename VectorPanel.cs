using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    class VectorPanel : Panel
    {
        public static Vector2 DEFAULT_SIZE = new Vector2(200, 70);
        public Vector2 editing;

        public VectorPanel(string label, Vector2 pos) : this(label, pos, DEFAULT_SIZE)
        { }

        public VectorPanel(string label, Vector2 pos, Vector2 size) : base(label, pos, size)
        {
            AddGadget<IntBox>("X");
            AddGadget<IntBox>("Y");
        }

        public Vector2 Value
        {
            get { return editing; }
        }

        public override void OnGUIEvent()
        {
            editing.X = (byte)GetGadget<IntBox>("X").Value;
            editing.Y = (byte)GetGadget<IntBox>("Y").Value;
        }
    }
}
