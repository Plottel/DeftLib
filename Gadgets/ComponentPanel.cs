using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.CSharp.RuntimeBinder;

namespace DeftLib
{
    public class ComponentPanel<T> : Panel
    {
        public T editing = (T)Activator.CreateInstance(typeof(T));

        public T Value
        {
            get { return editing; }
        }

        // Default constructor for Reflection instantiation
        public ComponentPanel() : this("", Vector2.Zero, Vector2.Zero, 1)
        { }

        public ComponentPanel(int layer) : this("", Vector2.Zero, Vector2.Zero, layer)
        { }

        public ComponentPanel(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        {
            foreach (var field in typeof(T).GetFields())
            {
                Type gadgetT = Gadget.gadgetTypeMap[field.FieldType];

                var mi = typeof(Panel).GetMethod("AddGadget");
                var methodRef = mi.MakeGenericMethod(gadgetT);

                methodRef.Invoke(this, new object[] { field.Name });
            }
        }

        public override void SetGadgetVariable(object newValue)
        {
            T component = (T)newValue;

            if (component != null)
            {
                foreach (var field in typeof(T).GetFields())
                {
                    Type gadgetT = Gadget.gadgetTypeMap[field.FieldType];

                    var mi = typeof(Panel).GetMethod("GetGadget");
                    var methodRef = mi.MakeGenericMethod(gadgetT);

                    dynamic g = methodRef.Invoke(this, new object[] { field.Name });
                    g.SetGadgetVariable(field.GetValue(component));
                }
            }
        }

        public override void OnGUIEvent()
        {
            base.OnGUIEvent();

            if (editing != null)
            {
                foreach (var field in typeof(T).GetFields())
                {
                    Type gadgetT = Gadget.gadgetTypeMap[field.FieldType];

                    var mi = typeof(Panel).GetMethod("GetGadget");
                    var methodRef = mi.MakeGenericMethod(gadgetT);

                    dynamic g = methodRef.Invoke(this, new object[] { field.Name });

                    field.SetValue(editing, g.Value);
                }
            }
        }
    }
}
