using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace DeftLib
{
    public class RectangleRenderingSystem : EntitySystem
    {
        private SpriteBatch _sb;

        public RectangleRenderingSystem() : base(typeof(SpatialComponent))
        {
            _sb = Deft.Get.spriteBatch;
        }

        public override void Process()
        {
            SpatialComponent spatial;

            foreach (Entity e in _entities)
            {
                spatial = e.GetComponent<SpatialComponent>();

                _sb.FillRectangle(spatial.pos, spatial.size, Color.Blue);
            }

        }
    }
}
