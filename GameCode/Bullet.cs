using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeftLib
{
    public class Bullet : Entity
    {
        public override void OnCreate()
        {
            base.OnCreate();

            SceneManager.RemoveEntity(this, 2f);
        }


        public override void OnCollision(Entity collidedWith)
        {
            base.OnCollision(collidedWith);

            if (collidedWith.Is<Wall>())
                SceneManager.RemoveEntity(this);
        }
    }
}
