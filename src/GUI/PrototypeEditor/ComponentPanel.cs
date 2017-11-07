using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder;

namespace DeftLib
{
    public class ComponentPanel<T> : Panel where T : Component
    {
        private List<FieldInfo> _componentFields;

        public const int DEFAULT_WIDTH = 250;
        public const int DEFAULT_HEIGHT = 200;

        // Default constructor for reflection instantiation
        public ComponentPanel() : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        {
        }

        // Layer constructor for reflection instantiation
        public ComponentPanel(int layer) : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public ComponentPanel(string label, Vector2 pos, Vector2 size, int layer) :
            base(label, pos, size, layer)
        {
            _componentFields = new List<FieldInfo>(typeof(T).GetFields());
            _componentFields.RemoveAll(field => field.Name == "owner");


            RepopulateGadgets();
            
        }     

        private void RepopulateGadgets()
        {
            _gadgets.Clear();

            foreach (var field in _componentFields)
            {
                Type gadgetType = GUIEventHub.GetGadgetType(field.FieldType);

                if (gadgetType == null)
                    throw new Exception("Invalid type in Component for GUI. " +
                        "Ensure you are using data types supported for GUI Gadgets. " +
                        "If not, ensure you add your own functionality");
                else
                {
                    AddGenericFieldGadget(gadgetType, field.Name);
                }
            }
        }

        /// <summary>
        /// Updates the values in this Panel to match the values of the passed in Component.
        /// Loops through each variable in the passed in Component
        /// and calls SyncGadget on the relevant gadget.
        /// </summary>
        public override void SyncGadget(object toAttach)
        {
            var component = (T)toAttach;

            if (component == null)
                throw new InvalidCastException("Object passed to ComponentPanel.Attach() " +
                    "could not be converted to proper component type");


            foreach (var field in _componentFields)
            {
                Type gadgetType = GUIEventHub.GetGadgetType(field.FieldType);

                if (gadgetType == null)
                    throw new Exception("Invalid type in Component for GUI. " +
                        "Ensure you are using data types supported for GUI Gadgets. " +
                        "If not, ensure you add your own functionality");
                else
                {

                    var gadgetForField = GetGenericFieldGadget(gadgetType, field.Name);
                    gadgetForField.SyncGadget(field.GetValue(component));

                }
            }
        }

        /// <summary>
        /// Updates the fields of the passed in component to match the values
        /// of the gadgets inside this Panel
        /// </summary>
        /// <param name="toSync">The component whose fields will be updated.</param>
        public void SyncComponent(Component toSync)
        {
            foreach (var field in _componentFields)
            {
                Type gadgetType = GUIEventHub.GetGadgetType(field.FieldType);

                if (gadgetType != null)
                {
                    // NOTE: Use of dynamic could cause casting errors.
                    dynamic gadgetForField = GetGenericFieldGadget(gadgetType, field.Name);
                    var value = gadgetForField.Value;

                    field.SetValue(toSync, value);
                }
            }
        }

        private void AddGenericFieldGadget(Type gadgetType, string fieldName)
        {
            var genericMethod = typeof(Panel).GetMethod("AddGadget");
            var concreteMethod = genericMethod.MakeGenericMethod(gadgetType);

            concreteMethod.Invoke(this, new object[] { fieldName });
        }

        private Gadget GetGenericFieldGadget(Type gadgetType, string fieldName)
        {
            var genericMethod = typeof(Panel).GetMethod("GetGadget");
            var concreteMethod = genericMethod.MakeGenericMethod(gadgetType);

            return concreteMethod.Invoke(this, new object[] { fieldName }) as Gadget;
        }
    }
}
