using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public class TextureRendererComponent : Component
    {
        public Texture2D texture;

        public void Render(SpriteBatch spriteBatch)
        {
            if (texture == null)
                return;

            var spatial = owner.Spatial;

            Vector2 origin = spatial.pos;
            float rotation = spatial.rotation;
            var scale = new Vector2(spatial.size.X / texture.Width, spatial.size.Y / texture.Height);

            spriteBatch.Draw(texture, origin, rotation, scale);
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(Assets.GetTextureName(texture));
        }

        public override void Deserialize(BinaryReader reader)
        {
            texture = Assets.GetTexture(reader.ReadString());
        }
    }
}
