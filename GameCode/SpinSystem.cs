using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeftLib
{
    public class SpinSystem : EntitySystem
    {
        // Call the base constructor, passing in the component types
        // an Entity must have for this system to operate on it.
        public SpinSystem()
            : base(typeof(SpinComponent), typeof(SpatialComponent))
        {

        }

        public override void Process()
        {
            SpinComponent spin;
            SpatialComponent spatial;


            // Fill in this loop with what should happen to each Entity each update.
            foreach (Entity entity in Entities)
            {
                spin = entity.GetComponent<SpinComponent>();
                spatial = entity.Spatial;

                spatial.rotation += spin.rotationAmount;
            }
        }
    }
}
