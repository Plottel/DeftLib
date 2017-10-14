using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Xna.Framework.Input;

namespace DeftLib
{
    public class EntityPanel : Panel
    {
        public Entity editing;

        public EntityPanel(string label, Vector2 pos, Vector2 size) : base(label, pos, size)
        {           
        }

        // TODO: Store generic method invocation stuff. It's being duplicated.
        public void SetEntity(Entity e)
        {
            editing = e;
            ClearGadgets();

            AddGadget<StringBox>("Add Component");

            foreach (var component in editing.ComponentMap)
            {
                var cpType = typeof(ComponentPanel<>);
                Type[] typeArg = { component.Key };
                var concreteCPType = cpType.MakeGenericType(typeArg);

                var addGadgetMethod = typeof(Panel).GetMethod("AddGadget");
                var addGadgetCall = addGadgetMethod.MakeGenericMethod(concreteCPType);
                addGadgetCall.Invoke(this, new object[] { component.Key.Name});

               _gadgets.Last().SetGadgetVariable(component.Value);
            }
        }

        public override void OnGUIEvent()
        {
            base.OnGUIEvent();

            if (!Input.KeyTyped(Keys.Enter))
                return;
            
            // TODO: Clean up MESSY logic.
            string componentToAdd = GetGadget<StringBox>("Add Component").Value;
            componentToAdd = "MovementComponent";
            Type typeToAdd = Type.GetType("DeftLib." + componentToAdd);

            if (typeToAdd != null && typeToAdd.IsSubclassOf(typeof(Component)))
            {
                if (editing != null)
                {
                    editing.AddComponent((Component)Activator.CreateInstance(typeToAdd));
                }                    
            }

            if (editing != null)
            {
                var componentsToUpdate = new List<Component>();

                foreach (var component in editing.ComponentMap)
                {
                    var cpType = typeof(ComponentPanel<>);
                    Type[] typeArg = { component.Key };
                    var concreteCPType = cpType.MakeGenericType(typeArg);

                    var getGadgetMethod = typeof(Panel).GetMethod("GetGadget");
                    var getGadgetCall = getGadgetMethod.MakeGenericMethod(concreteCPType);

                    dynamic g = getGadgetCall.Invoke(this, new object[] { component.Key.Name});

                    if (g != null) // TODO: This is a side effect of AddComponent button adding a gadget midway through Gadget list iteration.
                        componentsToUpdate.Add(g.Value);                    
                }

                foreach (var newComponent in componentsToUpdate)
                    editing.ReplaceComponent(newComponent.Copy());
            }
        }
    }
}
