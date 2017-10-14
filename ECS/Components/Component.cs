using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace DeftLib
{
    public abstract class Component
    {
        public abstract void Serialize(BinaryWriter writer);
        public abstract void Deserialize(BinaryReader reader);

        public bool IsActive { get; set; }

        public virtual Component Copy()
        {
            var cType = this.GetType();
            var c = (Component)Activator.CreateInstance(cType);

            foreach (var field in cType.GetFields(System.Reflection.BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                field.SetValue(c, field.GetValue(this));

            return c;
        }

        public Component() { }
    }
}
