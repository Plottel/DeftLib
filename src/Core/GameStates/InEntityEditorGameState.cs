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

        private ComponentEditorToolManager _toolManager = new ComponentEditorToolManager();
       
        // TODO: Build a public API around Editor Tools
        // User should be able to create custom components and 
        // assign ComponentEditorTool objects to them.
        public InEntityEditorGameState()
        {
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

            if (Input.KeyTyped(Keys.F2))
            {
                if (_selectedEntity != null)
                {
                    SceneManager.RemoveEntity(_selectedEntity);
                    ECSCore.UnsubscribeEntity(_selectedEntity);
                    _selectedEntity = null;
                }
            }

            // Update Component Panel when new Entity is selected.
            if (Input.LeftMousePressed())
            {
                foreach (var e in SceneManager.Scene.entities)
                {
                    var spatial = e.GetComponent<SpatialComponent>();

                    if (spatial.Bounds.Contains(Input.MousePos))
                    {
                        if (e != _selectedEntity)
                            _entityPanel.SetEntity(e);
                        _selectedEntity = e;
                        break;
                    }
                }
            }

            // Check selected component and run active tool to edit components.
            if (_selectedEntity != null)
                _toolManager.Edit(_selectedEntity, _entityPanel.SelectedComponentType);

            // Sync gadget for _entityPanel.SelectedComponentType
            if (_entityPanel.SelectedComponentType != null)
                _entityPanel.SyncComponentPanel(_entityPanel.SelectedComponentType);            
                      

            // Check for requested state transitions
            if (SceneManager.programStatePanel.GetGadget<Button>("Play Scene").IsClicked)
                SceneManager.PushState(new InGamePlayGameState());
        }

        public override void Update(GameTime gameTime)
        {            
        }

        public override void Render(SpriteBatch spriteBatch)
        {
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
            _toolManager.RenderGUI(spriteBatch);

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
                var e = Prototypes.Create(prototypeName, Input.MousePos);
                e.Spatial.pos = Input.MousePos - (e.Spatial.size / 2);
            }
        }
    }
}
