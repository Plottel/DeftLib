using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class CollisionComponent : Component
    {
        public Vector2 offset;
        public Vector2 size;

        public Rectangle HitBox
        {
            get
            {
                return new Rectangle((owner.Spatial.pos + offset).ToPoint(), size.ToPoint());
            }
        }

        public override void Deserialize(BinaryReader reader)
        {
            offset = reader.ReadVector2();
            size = reader.ReadVector2();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteVector2(offset);
            writer.WriteVector2(size);
        }
    }
}
