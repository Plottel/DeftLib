using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class Fireball : Entity
    {
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            GetComponent<PhysicsComponent>().velocity = new Vector2(2.5f, 2.5f);

            Spatial.rotation += 25f;
        }
    }
}
