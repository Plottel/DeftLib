using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DeftLib
{
    public class Tile
    {
        public Texture2D srcTexture;
        public Rectangle srcTextureRegion;

        public Vector2 pos;
        public Vector2 size;

        public Tile() { }

        public Tile(Texture2D srcTexture, Rectangle srcTextureRegion, Vector2 pos, Vector2 size)
        {
            this.srcTexture = srcTexture;
            this.srcTextureRegion = srcTextureRegion;
            this.pos = pos;
            this.size = size;
        }

        public Tile(Tile other)
        {
            this.srcTexture = other.srcTexture;
            this.srcTextureRegion = other.srcTextureRegion;
            this.pos = other.pos;
            this.size = other.size;
        }
        
        public Rectangle Bounds
        {
            get { return new Rectangle(pos.ToPoint(), size.ToPoint()); }
        }

        public void MoveBy(Vector2 amt)
            => pos += amt;

        public void Serialize(BinaryWriter writer)
        {
            var srcTextureName = Assets.GetTextureName(srcTexture);

            writer.Write(srcTextureName);
            writer.WriteRectangle(srcTextureRegion);
            writer.WriteVector2(pos);
            writer.WriteVector2(size);
        }

        public void Deserialize(BinaryReader reader)
        {
            var srcTextureName = reader.ReadString();

            srcTexture = Assets.GetTexture(srcTextureName);
            srcTextureRegion = reader.ReadRectangle();
            pos = reader.ReadVector2();
            size = reader.ReadVector2();
        }
    }
}
