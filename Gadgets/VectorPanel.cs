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

        // Default constructor for Reflection instantiation
        public VectorPanel() : this("", Vector2.Zero, DEFAULT_SIZE)
        { }

        public VectorPanel(string label, Vector2 pos) : this(label, pos, DEFAULT_SIZE)
        { }

        public VectorPanel(string label, Vector2 pos, Vector2 size) : base(label, pos, size)
        {
            AddGadget<IntBox>("X");
            AddGadget<IntBox>("Y");
        }

        public override void SetGadgetVariable(object newValue)
        {
            Vector2 v = (Vector2)newValue;

            GetGadget<IntBox>("X").text = v.X.ToString();
            GetGadget<IntBox>("Y").text = v.Y.ToString();
        }   

        public Vector2 Value
        {
            get { return editing; }
        }

        public override void OnGUIEvent()
        {
            editing.X = (float)GetGadget<IntBox>("X").Value;
            editing.Y = (float)GetGadget<IntBox>("Y").Value;
        }
    }
}
