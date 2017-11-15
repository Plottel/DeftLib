using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class Fireball: Entity
    {
        public override void OnCreate()
        {
            base.OnCreate();

            // Limited lifetime
            SceneManager.RemoveEntity(this, 2f);
        }


        public override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            // Define your custom update behaviour here.
        }


        public override void OnCollision(Entity collidedWith)
        {
            base.OnCollision(collidedWith);

            // Destroy self on collision with Enemy or Player
            if (collidedWith.Is<Enemy>() || collidedWith.Is<Player>())
                SceneManager.RemoveEntity(this);
        }
    }
}
