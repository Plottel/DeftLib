using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public class TextureInputBox : InputBox<Texture2D>
    {
        // Default constructor for reflection instantiation
        public TextureInputBox() :
            this("", Vector2.Zero, Vector2.Zero, 1)
        { }

        // Layer constructor for reflection instantiation
        public TextureInputBox(int layer) :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public TextureInputBox(string label, Vector2 pos, Vector2 size, int layer) :
            base(label, pos, size, layer)
        { }

        public override void SyncGadget(object toAttach)
        {
            var texture = toAttach as Texture2D;

            if (texture != null)
                _text = Assets.GetTextureName(texture);
        }

        public override Texture2D Value
        {
            get { return Assets.GetTexture(Text); }
        }

    }
}
