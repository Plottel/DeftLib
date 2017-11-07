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

            // TODO: FIX UP - SILLY TESTING
            if (Input.KeyTyped(Keys.F))
            {
                var fireball = Prototypes.Create("Fireball", new Vector2(50, 50));
                fireball.GetComponent<PhysicsComponent>().velocity = Vector2.Normalize(Input.MousePos - fireball.Spatial.pos) * 50;
                fireball.Spatial.size += new Vector2(10, 10);
                ECSCore.SubscribeEntity(fireball);
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

            foreach (var entity in Scene.entities)
                entity.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
        }
    }
}
