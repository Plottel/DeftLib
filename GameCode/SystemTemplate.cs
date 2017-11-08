using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeftLib
{
    public class SystemTemplate : EntitySystem
    {
        // Call the base constructor, passing in the component types
        // an Entity must have for this system to operate on it.
        public SystemTemplate()
            : base()
        {

        }

        public override void Process()
        {
            // Fill in this loop with what should happen to each Entity each update.
            foreach (Entity entity in Entities)
            {

            }
        }
    }
}
