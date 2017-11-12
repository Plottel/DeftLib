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
    public class InGamePlayGameState : GameState
    {
        public override void Enter()
        {
            SceneManager.SaveWorld();
        }

        public override void Exit()
        {
            //SceneManager.LoadWorld();
        }

        public override void HandleInput()
        {
            if (SceneManager.programStatePanel.GetGadget<Button>("Stop Scene").IsClicked)
                SceneManager.PopState();
        }

        public override void Update(GameTime gameTime)
        {
            ECSCore.Update(gameTime);

            foreach (var entity in Scene.entities)
                entity.OnUpdate(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
        }
    }
}
