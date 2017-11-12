using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class Spatial3DComponent : Component
    {
        // Declare component fields here. Prefer public access.
        public Vector3 pos;


        // Convert component to binary format and write to binary file.
        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(pos.X);
            writer.Write(pos.Y);
            writer.Write(pos.Z);
        }

        // Populate component with values read from a binary file.
        public override void Deserialize(BinaryReader reader)
        {
            pos.X = reader.ReadSingle();
            pos.Y = reader.ReadSingle();
            pos.Z = reader.ReadSingle();
        }
    }
}
