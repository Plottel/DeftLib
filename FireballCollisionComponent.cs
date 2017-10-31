using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class FireballCollisionComponent : CollisionComponent
    {
        public override void OnCollision(Entity collidedWith)
        {
            var thisPhysics = owner.GetComponent<PhysicsComponent>();
            var colliderPhysics = collidedWith.GetComponent<PhysicsComponent>();
           
            if (colliderPhysics != null)
            {
                var forceToApply = Vector2.Normalize(thisPhysics.velocity) * 5;
                colliderPhysics.AddForce(forceToApply);
            }
        }
    }
}
