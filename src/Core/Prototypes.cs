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

        private static List<Type> _allEntityTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                 from assemblyType in domainAssembly.GetTypes()
                                 where typeof(Entity).IsAssignableFrom(assemblyType)
                                 select assemblyType).ToList();

        public static Entity Create(string prototypeName, Vector2 pos)
        {
            if (_prototypes.ContainsKey(prototypeName.ToLower()))
            {
                Entity e = _prototypes[prototypeName.ToLower()].Copy();
                e.Spatial.pos = Vector2.Zero;
                e.Spatial.MoveBy(pos); // Move to proposed location, updating hit box.
                SceneManager.AddEntity(e);

                return e;
            }

            throw new Exception("Prototype with name : " + prototypeName + " : not found");
        }

        /// <summary>
        /// Same as Prototypes.Create, except doesn't add to World or ECS. Useful for editors.
        /// </summary>
        /// <param name="prototypeName"></param>
        /// <returns></returns>
        public static Entity Get(string prototypeName)
        {
            if (_prototypes.ContainsKey(prototypeName.ToLower()))
            {
                Entity e = _prototypes[prototypeName.ToLower()].Copy();
                ECSCore.UnsubscribeEntity(e);
                return e;
            }

            throw new Exception("Prototype with name : " + prototypeName + " : not found");
        }

        public static void Destroy(Entity toDestroy)
            => SceneManager.toBeDestroyed.Add(toDestroy);

        public static void AddPrototype(string prototypeName, Entity prototype)
        {
            _prototypes[prototypeName.ToLower()] = prototype.Copy();
            ECSCore.UnsubscribeEntity(_prototypes[prototypeName.ToLower()]);
        }
            

        public static void RemovePrototype(string prototypeName)
        {
            if (_prototypes.ContainsKey(prototypeName.ToLower()))
                _prototypes.Remove(prototypeName.ToLower());
        }

        public static List<string> AllPrototypeNamesUpper
        {
            get
            {
                var nonUpper = AllPrototypeNames;
                for (int i = nonUpper.Count - 1; i >= 0; --i)
                    nonUpper[i] = nonUpper[i].ToUpper();
                return nonUpper;
            }
        }

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
                    string name = reader.ReadString().ToLower();

                    string entityTypeName = reader.ReadString().ToLower();
                    Entity e = (Entity)Activator.CreateInstance(GetEntityType(entityTypeName));
                    e.Deserialize(reader);

                    // NOTE: Prototypes should not be part of the ECS, they are just templates to be instantiated.
                    ECSCore.UnsubscribeEntity(e);

                    _prototypes[name] = e;
                }
            }
        }

        private static Type GetEntityType(string name)
        {
            foreach (var t in _allEntityTypes)
            {
                if (t.Name.ToLower() == name)
                {
                    return t;
                }
            }
            return typeof(Entity);
        }
    }
}
