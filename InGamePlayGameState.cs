using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public class InGamePlayGameState : GameState
    {
        public override void Enter()
        {
            World.SaveWorld();
        }

        public override void Exit()
        {
        }

        public override void HandleInput()
        {
            if (World.programStatePanel.GetGadget<Button>("Stop Scene").IsClicked)
                World.PopState();
        }

        public override void Update(GameTime gameTime)
        {
            ECSCore.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
        }
    }
}
