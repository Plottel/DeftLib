using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class MovementComponent : IComponent
    {
        public Vector2 velocity;
        public int acceleration;
    }
}
