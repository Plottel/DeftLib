using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeftLib
{
    public class SpinComponent : Component
    {
        // Declare component fields here. Prefer public access.
        public float rotationAmount;


        // Convert component to binary format and write to binary file.
        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(rotationAmount);
        }

        // Populate component with values read from a binary file.
        public override void Deserialize(BinaryReader reader)
        {
            rotationAmount = reader.ReadSingle();
        }
    }
}
