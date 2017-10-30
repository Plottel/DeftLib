﻿using System;
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
        private StringBox _viewComponentStringBox;

        private Vector2 _componentPanelPos;
        private Panel _activeComponentPanel;

        public const int DEFAULT_WIDTH = 300;
        public const int DEFAULT_HEIGHT = 600;

        private const int FRONT_COMPONENT_PANEL_LAYER = 100;

        public EntityPanel(int layer) : this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public EntityPanel(string label, Vector2 pos, Vector2 size) : this(label, pos, size, 1)
        { }

        public EntityPanel(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        {
            AddNonComponentPanelGadgets();
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

        private void AddNonComponentPanelGadgets()
        {
            AddGadget<StringBox>("Add");
            _addComponentStringBox = GetGadget<StringBox>("Add");

            AddGadget<StringBox>("Remove");
            _removeComponentStringBox = GetGadget<StringBox>("Remove");

            AddGadget<StringBox>("View");
            _viewComponentStringBox = GetGadget<StringBox>("View");

            allComponentTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from assemblyType in domainAssembly.GetTypes()
                                     where typeof(Component).IsAssignableFrom(assemblyType)
                                     select assemblyType).ToList();
            allComponentTypes.Remove(typeof(Component));

            _componentPanelPos = NextGadgetAt + new Vector2(0, 20);
        }

        private void RenderNonComponentPanelGadgets(SpriteBatch spriteBatch)
        {
            _addComponentStringBox.Render(spriteBatch);
            _removeComponentStringBox.Render(spriteBatch);
            _viewComponentStringBox.Render(spriteBatch);
        }

        public void SetEntity(Entity e)
        {
            ClearGadgets();
            AddNonComponentPanelGadgets();

            foreach (var componentTypePair in e.ComponentMap)
                AddAndSyncComponentPanel(componentTypePair);

            // Don't change active component panel if resetting to the same Entity
            if (_editing != e)
            {
                _activeComponentPanel = GetGadget<Panel>("SpatialComponent");
                _activeComponentPanel.SetLayer(FRONT_COMPONENT_PANEL_LAYER);
            }

            _editing = e;
        }

        private void AddAndSyncComponentPanel(KeyValuePair<Type, Component> componentTypePair)
        {
            // Cache old NextGadgetAtPos since component panels are technically "on top of each other"
            var oldNextGadgetPos = NextGadgetAt;


            // Create Concrete Component Panel Type
            var genericComponentPanelType = typeof(ComponentPanel<>);
            Type[] concreteTypeArgs = { componentTypePair.Key };
            var concreteComponentPanelType = genericComponentPanelType.MakeGenericType(concreteTypeArgs);

            // Add Concrete Component Panel to Gadgets
            var genericAddGadgetMethod = typeof(Panel).GetMethod("AddGadget");
            var concreteAddGadgetMethod = genericAddGadgetMethod.MakeGenericMethod(concreteComponentPanelType);
            concreteAddGadgetMethod.Invoke(this, new object[] { componentTypePair.Key.Name });

            // Reset NextGadgetAt so adding component panels doesn't move it down
            NextGadgetAt = oldNextGadgetPos;

            // Sync new Component Panel with passed in Component
            _gadgets.Last().MoveTo(this.pos + _componentPanelPos);
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
                        var componentString = _removeComponentStringBox.Value.ToLower() + "component";
                        var componentTypeToRemove = allComponentTypes.Find(t => t.Name.ToLower() == componentString);

                        if (componentTypeToRemove != null && _editing.HasComponent(componentTypeToRemove))
                        {
                            _editing.RemoveComponent((Component)Activator.CreateInstance(componentTypeToRemove));
                            SetEntity(_editing);
                        }
                    }
                    else if (ActiveGadget == _viewComponentStringBox)
                    {
                        var componentString = _viewComponentStringBox.Value.ToLower() + "component";
                        var componentTypeToView = allComponentTypes.Find(t => t.Name.ToLower() == componentString);

                        if (componentTypeToView != null && _editing.HasComponent(componentTypeToView))
                        {
                            // Send old active component panel to the back
                            if (_activeComponentPanel != null)
                                _activeComponentPanel.SetLayer(this.layer + 1);

                            // Bring new active component panel to the front
                            _activeComponentPanel = GetGadget<Panel>(componentTypeToView.Name);
                            _activeComponentPanel.SetLayer(FRONT_COMPONENT_PANEL_LAYER);
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
            base.RenderWithoutGadgets(spriteBatch);
            RenderNonComponentPanelGadgets(spriteBatch);

            if (ActiveGadget == _addComponentStringBox)
                RenderSideText(spriteBatch, AllComponentTypeNames, this);
            else if (ActiveGadget == _removeComponentStringBox || ActiveGadget == _viewComponentStringBox)
            {
                if (_editing != null)
                    RenderSideText(spriteBatch, EntityComponentTypeStrings(_editing), this);
            }

            if (_activeComponentPanel != null)
                _activeComponentPanel.Render(spriteBatch);
        }

        private List<String> EntityComponentTypeStrings(Entity e)
        {
            var result = new List<string>();
            foreach (var c in e.ComponentList)
                result.Add(c.GetType().Name);
            return result;
        } 
    }
}
