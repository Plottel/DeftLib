using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.CSharp.RuntimeBinder;

namespace DeftLib
{
    public class EntityPanel : Panel
    {
        public Entity editing;

        public EntityPanel(string label, Vector2 pos, Vector2 size) : base(label, pos, size)
        {           
        }

        public void SetEntity(Entity e)
        {
            editing = e;
            ClearGadgets();

            foreach (var component in editing.ComponentMap)
            {
                // TODO: I think i need an extra level of "Generic Method fetching"
                // We're always fetching "ComponentPanel"
                // Extra generic to fetch is ComponentPanel<T>

                //T newGadget = (T)Activator.CreateInstance(typeof(T), label, _nextGadgetAt);

                var cpType = typeof(ComponentPanel<>);
                Type[] typeArg = { component.Key };
                var concreteCPType = cpType.MakeGenericType(typeArg);

                var addGadgetMethod = typeof(Panel).GetMethod("AddGadget");
                var addGadgetCall = addGadgetMethod.MakeGenericMethod(concreteCPType);
                addGadgetCall.Invoke(this, new object[] { component.Key.Name});
            }
        }

        public override void OnGUIEvent()
        {
            base.OnGUIEvent();

            if (editing != null)
            {
                var componentsToUpdate = new List<IComponent>();

                foreach (var component in editing.ComponentMap)
                {
                    var cpType = typeof(ComponentPanel<>);
                    Type[] typeArg = { component.Key };
                    var concreteCPType = cpType.MakeGenericType(typeArg);

                    var getGadgetMethod = typeof(Panel).GetMethod("GetGadget");
                    var getGadgetCall = getGadgetMethod.MakeGenericMethod(concreteCPType);

                    dynamic g = getGadgetCall.Invoke(this, new object[] { component.Key.Name});

                    componentsToUpdate.Add(g.Value);                    
                }

                foreach (var newComponent in componentsToUpdate)
                    editing.ReplaceComponent(newComponent);  // TODO: This really needs to clone the component;
            }
        }
    }
}
