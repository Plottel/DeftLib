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
    public class InEntityEditorGameState : GameState
    {
        private EntityPanel _entityPanel = new EntityPanel("Entity Editor", new Vector2(10, 10), new Vector2(300, 500));
        private PrototypeInstantiationPanel _prototypePanel = new PrototypeInstantiationPanel("Instantiate Prototypes with F1", new Vector2(620, 10), new Vector2(350, 200), 1);

        private Entity _selectedEntity;
        private SpatialComponentEditorTool _spatialEditor = new SpatialComponentEditorTool();

        private Dictionary<Type, ComponentEditorTool> _editorTools = new Dictionary<Type, ComponentEditorTool>();

        // TODO: Build a public API around Editor Tools
        // User should be able to create custom components and 
        // assign ComponentEditorTool objects to them.
        public InEntityEditorGameState()
        {
            _editorTools[typeof(SpatialComponent)] = new SpatialComponentEditorTool();
            _editorTools[typeof(MovementComponent)] = new MovementComponentEditorTool();
        }

        private ComponentEditorTool ActiveTool
        {
            get
            {
                Type toolType = _entityPanel.SelectedComponentType;
                if (_selectedEntity != null && toolType != null)
                {
                    if (_editorTools.ContainsKey(toolType))
                        return _editorTools[toolType];
                }
                return null;
            }
        }

        public override void Enter()
        {
            GUIEventHub.Subscribe(_entityPanel);
            GUIEventHub.Subscribe(_prototypePanel);
        }

        public override void Exit()
        {
            GUIEventHub.Unsubscribe(_entityPanel);
            GUIEventHub.Unsubscribe(_prototypePanel);
        }

        public override void HandleInput()
        {
            if (Input.KeyTyped(Keys.F1))
                InstantiateSelectedPrototypeAtMousePos();

            if (Input.KeyTyped(Keys.Delete))
            {
                if (_selectedEntity != null)
                {
                    World.entities.Remove(_selectedEntity);
                    ECSCore.UnsubscribeEntity(_selectedEntity);
                    _selectedEntity = null;
                }
            }

            // Update Component Panel when new Entity is selected.
            if (Input.LeftMousePressed())
            {
                foreach (var e in World.entities)
                {
                    var spatial = e.GetComponent<SpatialComponent>();

                    if (spatial.Bounds.Contains(Input.MousePos))
                    {
                        _selectedEntity = e;
                        _entityPanel.SetEntity(_selectedEntity);
                        break;
                    }

                }
            }

            // Check selected component and run active tool to edit components.
            if (ActiveTool != null)
                ActiveTool.Edit(_selectedEntity);

            // Check for requested state transitions
            if (World.programStatePanel.GetGadget<Button>("Play Scene").IsClicked)
                World.PushState(new InGamePlayGameState());
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Render(SpriteBatch spriteBatch)
        {
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
            if (ActiveTool != null)
                ActiveTool.RenderGUI(spriteBatch);

            if (_selectedEntity != null)
                spriteBatch.DrawRectangle(_selectedEntity.GetComponent<SpatialComponent>().Bounds, Color.LawnGreen, 2);

            _entityPanel.Render(spriteBatch);
            _prototypePanel.Render(spriteBatch);
        }

        private void InstantiateSelectedPrototypeAtMousePos()
        {
            var prototypeName = _prototypePanel.SelectedPrototypeName;

            if (prototypeName != "")
            {
                var newEntity = Prototypes.Create(prototypeName, Input.MousePos);
                World.entities.Add(newEntity);
            }
        }
    }
}
