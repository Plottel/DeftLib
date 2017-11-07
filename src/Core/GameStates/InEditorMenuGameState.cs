using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public class InEditorMenuGameState : GameState
    {
        public InEditorMenuGameState()
        { }

        public override void Enter()
            => SceneManager.LoadWorld();

        public override void Exit() { }
        public override void HandleInput() { }
        public override void Update(GameTime gameTime) { }
        public override void Render(SpriteBatch spriteBatch) { }
        public override void RenderGUI(SpriteBatch spriteBatch) { }
    }
}
