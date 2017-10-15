using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    class ColorPanel : Panel
    {
        public static Vector2 DEFAULT_SIZE = new Vector2(200, 140);
        public Color editing;

        // Default constructor for Reflection instantiation
        public ColorPanel() : this("", Vector2.Zero, DEFAULT_SIZE, 1)
        { }

        // Layer constructor for Reflection instantiation
        public ColorPanel(int layer) : this("", Vector2.Zero, DEFAULT_SIZE, layer)
        { }

        public ColorPanel(string label, Vector2 pos) : this(label, pos, DEFAULT_SIZE, 1)
        { }

        public ColorPanel(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        {
            AddGadget<IntBox>("R");
            AddGadget<IntBox>("G");
            AddGadget<IntBox>("B");
            AddGadget<IntBox>("A");
        }

        public Color Value
        {
            get { return editing; }
        }

        public override void OnGUIEvent()
        {
            editing.R = (byte)GetGadget<IntBox>("R").Value;
            editing.G = (byte)GetGadget<IntBox>("G").Value;
            editing.B = (byte)GetGadget<IntBox>("B").Value;
            editing.A = (byte)GetGadget<IntBox>("A").Value;
        }
    }
}
