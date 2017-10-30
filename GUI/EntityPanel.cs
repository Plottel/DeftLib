using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DeftLib
{
    public class EntityPanel : Panel
    {
        private Entity _editing;
        private List<Type> allComponentTypes;
        private StringBox _addComponentStringBox;
        private StringBox _removeComponentStringBox;

        public EntityPanel(int layer) : this("", Vector2.Zero, Vector2.Zero, layer)
        { }

        public EntityPanel(string label, Vector2 pos, Vector2 size) : this(label, pos, size, 1)
        { }

        public EntityPanel(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        {
            AddTheAddComponentButton();
        }

        private List<String> AllComponentTypeNames
        {
            get
            {
                var result = new List<string>();

                foreach (var type in allComponentTypes)
                    result.Add(type.Name);

                return result;
            }
        }

        private void AddTheAddComponentButton()
        {
            AddGadget<StringBox>("Add");
            _addComponentStringBox = GetGadget<StringBox>("Add");

            AddGadget<StringBox>("Remove");
            _removeComponentStringBox = GetGadget<StringBox>("Remove");

            allComponentTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from assemblyType in domainAssembly.GetTypes()
                                     where typeof(Component).IsAssignableFrom(assemblyType)
                                     select assemblyType).ToList();
            allComponentTypes.Remove(typeof(Component));
        }

        public void SetEntity(Entity e)
        {
            _editing = e;
            ClearGadgets();
            AddTheAddComponentButton();

            foreach (var componentTypePair in _editing.ComponentMap)
                AddAndSyncComponentPanel(componentTypePair);
        }

        private void AddAndSyncComponentPanel(KeyValuePair<Type, Component> componentTypePair)
        {
            // Create Concrete Component Panel Type
            var genericComponentPanelType = typeof(ComponentPanel<>);
            Type[] concreteTypeArgs = { componentTypePair.Key };
            var concreteComponentPanelType = genericComponentPanelType.MakeGenericType(concreteTypeArgs);

            // Add Concrete Component Panel to Gadgets
            var genericAddGadgetMethod = typeof(Panel).GetMethod("AddGadget");
            var concreteAddGadgetMethod = genericAddGadgetMethod.MakeGenericMethod(concreteComponentPanelType);
            concreteAddGadgetMethod.Invoke(this, new object[] { componentTypePair.Key.Name });

            // Sync new Component Panel with passed in Component
            _gadgets.Last().SyncGadget(componentTypePair.Value);
        }

        private void SyncComponent(KeyValuePair<Type, Component> componentTypePair)
        {
            // Create Concrete Component Panel Type
            var genericComponentPanelType = typeof(ComponentPanel<>);
            Type[] concreteTypeArgs = { componentTypePair.Key };
            var concreteComponentPanelType = genericComponentPanelType.MakeGenericType(concreteTypeArgs);

            // Fetch Concrete Component Panel from Gadgets
            var genericGetGadgetMethod = typeof(Panel).GetMethod("GetGadget");
            var concreteGetGadgetMethod = genericGetGadgetMethod.MakeGenericMethod(concreteComponentPanelType);
            dynamic theComponentPanel = concreteGetGadgetMethod.Invoke(this, new object[] { componentTypePair.Key.Name });

            theComponentPanel.SyncComponent(componentTypePair.Value);

        }

        public Type SelectedComponentType
        {
            get
            {
                if (ActiveGadget != null)
                {
                    var genericClassArguments = ActiveGadget.GetType().GetGenericArguments();
                    return genericClassArguments.Length > 0 ? genericClassArguments[0] : null;
                }
                return null;
            }
        }

        public override void OnGUIEvent()
        {
            base.OnGUIEvent();            
        
            // Check if any new Components were requested to be added.
            if (_editing != null)
            {
                if (Input.KeyTyped(Keys.Enter))
                {
                    if (ActiveGadget == _addComponentStringBox)
                    {
                        var componentString = (_addComponentStringBox.Value + "component").ToLower();
                        var componentTypeToAdd = allComponentTypes.Find(t => t.Name.ToLower() == componentString);

                        if (componentTypeToAdd != null && !(_editing.HasComponent(componentTypeToAdd)))
                        {
                            _editing.AddComponent((Component)Activator.CreateInstance(componentTypeToAdd));
                            SetEntity(_editing);
                        }
                    }
                    else if (ActiveGadget == _removeComponentStringBox)
                    {
                        var componentString = _removeComponentStringBox.Value.ToLower();
                        var componentTypeToRemove = allComponentTypes.Find(t => t.Name.ToLower() == componentString);

                        if (componentTypeToRemove != null && _editing.HasComponent(componentTypeToRemove))
                        {
                            _editing.RemoveComponent((Component)Activator.CreateInstance(componentTypeToRemove));
                            SetEntity(_editing);
                        }
                    }
                }
            }
            

            // If update requested, sync the _editing Entity
            // with the values from all component panels
            if (_editing != null && Input.KeyTyped(Keys.Enter))
            {
                foreach (var componentTypePair in _editing.ComponentMap)
                    SyncComponent(componentTypePair);
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);

            // Render list of possible components if AddComponentStringBox is active
            if (ActiveGadget == _addComponentStringBox || ActiveGadget == _removeComponentStringBox)
                RenderSideText(spriteBatch, AllComponentTypeNames, this);
        }
    }
}
