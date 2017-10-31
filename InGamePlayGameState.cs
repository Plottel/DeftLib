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

            // TODO: FIX UP - SILLY TESTING
            if (Input.KeyTyped(Keys.F))
            {
                var fireball = Prototypes.Create("Fireball", new Vector2(50, 50));
                World.entities.Add(fireball);
                fireball.GetComponent<PhysicsComponent>().velocity = Vector2.Normalize(Input.MousePos - fireball.Spatial.pos) * 50;
                fireball.Spatial.size += new Vector2(10, 10);
                ECSCore.PlaceEntityInSystems(fireball);
            }

            //if (Input.KeyTyped(Keys.P))
            //{
            //    foreach (var e in World.entities)
            //    {
            //        var physics = e.GetComponent<PhysicsComponent>();

            //        if (physics != null)
            //            physics.AddForce(new Vector2(10, 10));
            //    }
            //}
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
