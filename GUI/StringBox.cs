using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace DeftLib
{
    public class StringBox : InputBox<string>

    {   // Default constructor for reflection instantiation
        public StringBox() :
            this("", Vector2.Zero, Vector2.Zero, 1)
        { }

        // Layer constructor for reflection instantiation
        public StringBox(int layer) :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH * 1.6f, DEFAULT_HEIGHT), layer)
        { }

        public StringBox(string label, Vector2 pos, Vector2 size, int layer) :
            base(label, pos, size, layer)
        {           
        }

        public override void SyncGadget(object toAttach)
        {
            var s = toAttach as string;
            if (s != null)
                _text = s;
        }

        public override string Value
        {
            get { return _text; }
        }        
    }
}
