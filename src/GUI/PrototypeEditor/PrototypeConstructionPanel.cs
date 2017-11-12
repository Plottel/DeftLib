using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace DeftLib
{
    public class PrototypeConstructionPanel : Panel
    {
        public const int DEFAULT_WIDTH = 1000;
        public const int DEFAULT_HEIGHT = 715;

        private EntityPanel _entityPanel;
        public EntityPanel EntityPanel { get => _entityPanel; }

        private Entity _currentPrototype;
        public Entity CurrentPrototype { get => _currentPrototype; }

        private Vector2 _prototypePos; 

        private ComponentEditorToolManager _toolManager = new ComponentEditorToolManager();

        private List<Type> _allEntityTypes = new List<Type>();


        // Default constructor for reflection instantiation
        public PrototypeConstructionPanel() : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instantiation
        public PrototypeConstructionPanel(int layer) : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public PrototypeConstructionPanel(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        {
            AddGadget<StringBox>("Open");
            AddGadget<StringBox>("Name");
            AddGadget<StringBox>("New");
            AddGadget<Button>("Save");
            AddGadget<ShieldButton>("Delete");
            AddGadget<EntityPanel>("Current Prototype");
            _entityPanel = GetGadget<EntityPanel>("Current Prototype");
            _prototypePos = new Vector2(700, 300);

            _entityPanel.SetEntity(_currentPrototype);

            _allEntityTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                 from assemblyType in domainAssembly.GetTypes()
                                 where typeof(Entity).IsAssignableFrom(assemblyType)
                                 select assemblyType).ToList();
            _allEntityTypes.Remove(typeof(EntityTemplate));
        }

        public override void OnGUIEvent()
        {
            // Sync gadget for _entityPanel.SelectedComponentType
            if (_entityPanel.SelectedComponentType != null)
                _entityPanel.SyncComponentPanel(_entityPanel.SelectedComponentType);

            base.OnGUIEvent();       

            // Update Editor Tool states
            if (_currentPrototype != null)
            {
                _toolManager.Edit(_currentPrototype, _entityPanel.SelectedComponentType);                
            }

            if (GetGadget<Button>("Save").IsClicked)
            {
                var nameSB = GetGadget<StringBox>("Name");

                Prototypes.AddPrototype(nameSB.Value, _currentPrototype);
                _entityPanel.SetEntity(_currentPrototype);
                //ResetPrototype();
                //nameSB.SyncGadget("");
            }

            if (GetGadget<ShieldButton>("Delete").IsClicked)
            {
                Prototypes.RemovePrototype(GetGadget<StringBox>("Name").Value);
                ResetPrototype();
            }

            if (Input.KeyTyped(Keys.Enter))
            {
                var openSB = GetGadget<StringBox>("Open");
                var newSB = GetGadget<StringBox>("New");

                if (ActiveGadget == openSB)
                {
                    if (Prototypes.AllPrototypeNames.Contains(openSB.Value.ToLower()))
                    {
                        _currentPrototype = Prototypes.Get(openSB.Value);
                        _currentPrototype.Spatial.pos = _prototypePos;
                        _entityPanel.SetEntity(_currentPrototype);
                        GetGadget<StringBox>("Name").SyncGadget(openSB.Value);
                        openSB.SyncGadget("");
                    }
                }
                else if (ActiveGadget == newSB)
                {
                    _currentPrototype = null;
                    //ResetPrototype();

                    // Create new Entity subclass with same name as "New" StringBox value.
                    string entityTypeName = newSB.Value.ToLower();

                    foreach (Type t in _allEntityTypes)
                    {
                        if (t.Name.ToLower() == entityTypeName)
                        {
                            _currentPrototype = (Entity)Activator.CreateInstance(t);
                            var spatial = new SpatialComponent() { pos = this.pos + _prototypePos, size = new Vector2(50, 50) };
                            _currentPrototype.AddComponent(spatial);
                            var textureRenderer = new TextureRendererComponent() { texture = Assets.GetTexture("DefaultTexture") };
                            _currentPrototype.AddComponent(textureRenderer);
                            ECSCore.UnsubscribeEntity(_currentPrototype);
                            _entityPanel.SetEntity(_currentPrototype);
                            GetGadget<StringBox>("Name").SyncGadget(entityTypeName.ToUpper());
                        }
                    }
                }                    
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);

            var prototypeBorderRect = new Rectangle((this.pos + _prototypePos - new Vector2(150, 150)).ToPoint(), new Point(350, 350));

            _toolManager.RenderGUI(spriteBatch);

            if (ActiveGadget == GetGadget<StringBox>("Open"))
                Panel.RenderSideText(spriteBatch, Prototypes.AllPrototypeNamesUpper, this);
            else if (ActiveGadget == GetGadget<StringBox>("New"))
                Panel.RenderSideText(spriteBatch, AllEntityTypeNames, this);

            // TODO: When program is larger, "Entity.Render()" is a consideration -> can't do it without the ECS.
            if (_currentPrototype != null)
            {
                var c = _currentPrototype;

                if (c.HasComponent<TextureRendererComponent>())
                    c.GetComponent<TextureRendererComponent>().Render(spriteBatch);
                else if (c.HasComponent<RectangleRendererComponent>())
                    c.GetComponent<RectangleRendererComponent>().Render(spriteBatch);
            }
                
        }

        private List<string> AllEntityTypeNames
        {
            get
            {
                var result = new List<string>();
                foreach (var t in _allEntityTypes)
                    result.Add(t.Name);
                return result;
            }
        }

        public void ResetPrototype()
        {
            GetGadget<StringBox>("Name").SyncGadget("");
            GetGadget<StringBox>("Open").SyncGadget("");
            GetGadget<StringBox>("New").SyncGadget("");
            _currentPrototype = null;
        }
    }
}
