using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class IntBox : InputBox<int>
    {
        // Default constructor for reflection instantiation
        public IntBox() : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instantiation
        public IntBox(int layer) : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public IntBox(string label, Vector2 pos, Vector2 size, int layer) : 
            base(label, pos, size, layer)
        { }

        public override void SyncGadget(object toAttach)
        {
            int i = (int)toAttach;
            Text = i.ToString();
        }

        public override int Value
        {
            get
            {
                int result;

                if (Int32.TryParse(_text, out result))
                    return result;
                return 0;
            }
        }
    }
}
