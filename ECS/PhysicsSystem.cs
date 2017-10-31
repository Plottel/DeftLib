using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class PhysicsSystem : EntitySystem
    {
        public PhysicsSystem() : 
            base(typeof(PhysicsComponent), typeof(SpatialComponent))
        { }

        public override void Process()
        {
            PhysicsComponent physics;
            SpatialComponent spatial;

            foreach (var entity in _entities)
            {
                physics = entity.GetComponent<PhysicsComponent>();

                // Apply and decay all accelerations
                for (int i = physics.ActiveForces.Count - 1; i >= 0; --i)
                {
                    physics.velocity += physics.ActiveForces[i]; // Take MASS and DELTATIME into account

                    var accelReductionVector = (physics.ActiveForces[i] * -1) * 0.9f;
                    physics.ActiveForces[i] += accelReductionVector;

                    // Kill acceleration if it has decayed below a threshold
                    if (physics.ActiveForces[i].Length() < 1) // 1 == threshold - pick a new one if needed.
                        physics.ActiveForces.RemoveAt(i);
                }

                if (physics.velocity != Vector2.Zero)
                {
                    // Reduce velocity according to drag
                    //physics.velocity *= 1 / physics.drag; // FIX FORMULA
                    var dragVector = physics.velocity * -1;
                    dragVector *= 0.05f;

                    physics.velocity += dragVector;

                    // Move the entity according to resultant velocity
                    spatial = entity.GetComponent<SpatialComponent>();
                    spatial.pos += physics.velocity;
                }             
            }
        }
    }
}
