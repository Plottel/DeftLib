using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.IO;

namespace DeftLib
{
    // TODO: Look up Prototype pattern
    public static class Prototypes
    {
        private static Dictionary<string, Entity> _prototypes = 
            new Dictionary<string, Entity>();

        public static Entity Create(string prototypeName, Vector2 pos)
        {
            if (_prototypes.ContainsKey(prototypeName))
            {
                Entity e = _prototypes[prototypeName].Copy();
                e.GetComponent<SpatialComponent>().pos = pos;

                return e;
            }

            throw new Exception("Prototype with name : " + prototypeName + " : not found");
        }

        public static void AddPrototype(string prototypeName, Entity prototype)
            => _prototypes[prototypeName] = prototype.Copy();

        public static List<string> AllPrototypeNames
        {
            get => _prototypes.Keys.ToList();
        }

        public static void SavePrototypes()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("prototypes.bin", FileMode.OpenOrCreate)))
            {
                writer.Write(_prototypes.Count);

                foreach (var prototype in _prototypes)
                {
                    string name = prototype.Key;
                    Entity e = prototype.Value;

                    writer.Write(name);
                    e.Serialize(writer);
                }
            }
        }

        public static void LoadPrototypes()
        {
            if (!File.Exists("prototypes.bin"))
                return;

            _prototypes.Clear();

            using (BinaryReader reader = new BinaryReader(File.Open("prototypes.bin", FileMode.Open)))
            {
                int numPrototypes = reader.ReadInt32();

                for (int i = 0; i < numPrototypes; ++i)
                {
                    string name = reader.ReadString();
                    Entity e = new Entity();
                    e.Deserialize(reader);

                    _prototypes[name] = e;
                }
            }
        }
    }
}
