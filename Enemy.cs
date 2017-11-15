using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class Enemy : Entity
    {
        public int lungeCooldown = 200;
        private int _updatesSinceLastLunge = 0;

        public override void OnCreate()
        {
            base.OnCreate();

            // Define your custom startup behaviour here.
        }


        public override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            // Lunge towards player every 200 ticks
            ++_updatesSinceLastLunge;

            if (lungeCooldown < _updatesSinceLastLunge)
            {
                _updatesSinceLastLunge = 0;

                var player = SceneManager.GetEntity<Player>();
                var toPlayer = Vector2.Normalize(player.Spatial.MidVector - Spatial.MidVector);

                GetComponent<PhysicsComponent>().AddForce(toPlayer * 30);
            }            
        }


        public override void OnCollision(Entity collidedWith)
        {
            base.OnCollision(collidedWith);

            // Define your custom collision behaviour here.
        }
    }
}
