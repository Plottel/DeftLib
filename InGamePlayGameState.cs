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
            World.SaveWorld();
        }

        public override void Exit()
        {
        }

        public override void HandleInput()
        {
            if (World.programStatePanel.GetGadget<Button>("Stop Scene").IsClicked)
                World.PopState();

            if (Input.KeyTyped(Keys.F))
            {
                foreach (Entity e in World.entities)
                {
                    var physics = e.GetComponent<PhysicsComponent>();

                    if (physics != null)
                        physics.AddForce(new Vector2(0.1f, 0.1f));
                }
            }
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
