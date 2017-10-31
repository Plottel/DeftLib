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
            SpatialComponent s1, s2;
            CollisionComponent c1, c2;

            // TODO: SLOW
            for (int i = 0; i < _entities.Count; ++i)
            {
                s1 = _entities[i].Spatial;

                for (int j = 0; j < _entities.Count; ++j)
                {
                    if (i == j)
                        continue;

                    s2 = _entities[j].Spatial;

                    if (s1.Bounds.Intersects(s2.Bounds))
                    {
                        if (_entities[i].HasComponent<FireballCollisionComponent>())
                            c1 = _entities[i].GetComponent<FireballCollisionComponent>();                        
                        else
                            c1 = _entities[i].GetComponent<CollisionComponent>();

                        if (_entities[j].HasComponent<FireballCollisionComponent>())
                            c2 = _entities[j].GetComponent<FireballCollisionComponent>();
                        else
                            c2 = _entities[j].GetComponent<CollisionComponent>();

                        c1.OnCollision(_entities[j]);
                        c2.OnCollision(_entities[i]);
                    }
                }
            }
        }
    }
}
