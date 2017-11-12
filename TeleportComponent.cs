using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class TeleportComponent : Component
    {
        public Vector2 position;

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteVector2(position);
        }

        public override void Deserialize(BinaryReader reader)
        {
            position = reader.ReadVector2();
        }
    }
}
