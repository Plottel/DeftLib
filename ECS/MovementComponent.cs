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
        public Vector2 direction;
        public float speed;

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteVector2(direction);
            writer.Write(speed);
        }

        public override void Deserialize(BinaryReader reader)
        {
            direction = reader.ReadVector2();
            speed = reader.ReadSingle();
        }
    }
}
