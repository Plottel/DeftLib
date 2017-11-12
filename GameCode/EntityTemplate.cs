using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class EntityTemplate : Entity
    {
        public override void OnCreate()
        {
            base.OnCreate();

            // Define your custom startup behaviour here.
        }


        public override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            // Define your custom update behaviour here.
        }


        public override void OnCollision(Entity collidedWith)
        {
            base.OnCollision(collidedWith);

            // Define your custom collision behaviour here.
        }        
    }
}
