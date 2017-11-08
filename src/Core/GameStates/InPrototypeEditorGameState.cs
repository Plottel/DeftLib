using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public class InPrototypeEditorGameState : GameState
    {
        private PrototypeConstructionPanel _prototypePanel = new PrototypeConstructionPanel("Prototype Editor", new Vector2(10, 10), new Vector2(1000, 715), 1);

        public InPrototypeEditorGameState()
        {
            _prototypePanel.ResetPrototype();
        }

        public override void Enter()
        {
            GUIEventHub.Subscribe(_prototypePanel);
            ECSCore.SubscribeEntity(_prototypePanel.CurrentPrototype);
        }

        public override void Exit()
        {
            GUIEventHub.Unsubscribe(_prototypePanel);
            ECSCore.UnsubscribeEntity(_prototypePanel.CurrentPrototype);
        }

        public override void HandleInput()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Render(SpriteBatch spriteBatch)
        {
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
            _prototypePanel.Render(spriteBatch);
        }
    }
}
