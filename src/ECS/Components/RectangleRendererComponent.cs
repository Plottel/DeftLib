using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.IO;

namespace DeftLib
{
    public class RectangleRendererComponent : Component
    {
        public Color color = new Color(0, 0, 0, 0);

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(owner.Spatial.Bounds, color);
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteColor(color);
        }

        public override void Deserialize(BinaryReader reader)
        {
            color = reader.ReadColor();
        }
    }
}
