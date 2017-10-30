using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class SpatialComponent : Component
    {
        public Vector2 pos;
        public Vector2 size;
        public float rotation;

        public Rectangle Bounds
        {
            get { return new Rectangle(pos.ToPoint(), size.ToPoint()); }
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteVector2(pos);
            writer.WriteVector2(size);
            writer.Write(rotation);
        }

        public override void Deserialize(BinaryReader reader)
        {
            pos = reader.ReadVector2();
            size = reader.ReadVector2();
            rotation = reader.ReadSingle();
        }
    }
}
