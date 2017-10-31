using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class CollisionComponent : Component
    {
        public override void Deserialize(BinaryReader reader)
        {
        }

        public override void Serialize(BinaryWriter writer)
        {
        }

        public virtual void OnCollision(Entity collidedWith)
        {
        }
    }
}
