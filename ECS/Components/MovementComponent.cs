using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class MovementComponent : Component
    {
        public Vector2 velocity;

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteVector2(velocity);
        }

        public override void Deserialize(BinaryReader reader)
        {
            velocity = reader.ReadVector2();
        }
    }
}
