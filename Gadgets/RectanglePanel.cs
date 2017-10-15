using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    class RectanglePanel : Panel
    {
        public static Vector2 DEFAULT_SIZE = new Vector2(200, 140);
        public Rectangle editing;

        // Default constructor for Reflection instantiation
        public RectanglePanel() : this("", Vector2.Zero, DEFAULT_SIZE, 1)
        { }

        public RectanglePanel(int layer) : this("", Vector2.Zero, DEFAULT_SIZE, layer)
        { }

        public RectanglePanel(string label, Vector2 pos) : this(label, pos, DEFAULT_SIZE, 1)
        { }

        public RectanglePanel(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        {
            AddGadget<IntBox>("X");
            AddGadget<IntBox>("Y");
            AddGadget<IntBox>("W");
            AddGadget<IntBox>("H");
        }

        public Rectangle Value
        {
            get { return editing; }
        }

        public override void OnGUIEvent()
        {
            editing.X = (byte)GetGadget<IntBox>("X").Value;
            editing.Y = (byte)GetGadget<IntBox>("Y").Value;
            editing.Width = (byte)GetGadget<IntBox>("W").Value;
            editing.Height = (byte)GetGadget<IntBox>("H").Value;
        }
    }
}
