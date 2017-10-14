using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class MovementSystem : EntitySystem
    {
        public MovementSystem() : base(typeof(MovementComponent), typeof(SpatialComponent))
        { }

        public override void Process()
        {
            SpatialComponent spatial;
            MovementComponent movement;

            foreach (Entity e in _entities)
            {
                spatial = e.GetComponent<SpatialComponent>();
                movement = e.GetComponent<MovementComponent>();

                spatial.pos += movement.velocity;
            }
        }
    }
}
