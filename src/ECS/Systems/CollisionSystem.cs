using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeftLib
{
    public class CollisionSystem : EntitySystem
    {
        public CollisionSystem() :
            base(typeof(CollisionComponent), typeof(SpatialComponent))
        { }

        public override void Process()
        {
            CollisionComponent e1Col, e2Col;
            Entity e1, e2;

            // TODO: SLOW
            for (int i = 0; i < _entities.Count; ++i)
            {
                e1 = _entities[i];

                for (int j = 0; j < _entities.Count; ++j)
                {
                    if (i == j)
                        continue;

                    e2 = _entities[j];

                    e1Col = _entities[i].GetComponent<CollisionComponent>();
                    e2Col = _entities[j].GetComponent<CollisionComponent>();

                    if (e1Col.HitBox.Intersects(e2Col.HitBox))
                    {
                        e1.OnCollision(e2);
                        e2.OnCollision(e1);
                    }
                }
            }
        }
    }
}
