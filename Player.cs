using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DeftLib
{
    public class Player : Entity
    {
        public float speed = 0.6f;
        public float fireballSpeed = 3f;

        public override void OnCreate()
        {
            base.OnCreate();

            // Define your custom startup behaviour here.
        }


        public override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            var physics = GetComponent<PhysicsComponent>();

            // Move with WASD Keys
            if (Input.KeyDown(Keys.W))
                physics.AddForce(new Vector2(0, -speed));
            if (Input.KeyDown(Keys.S))
                physics.AddForce(new Vector2(0, speed));
            if (Input.KeyDown(Keys.A))
                physics.AddForce(new Vector2(-speed, 0));
            if (Input.KeyDown(Keys.D))
                physics.AddForce(new Vector2(speed, 0));

            // Rotate to face the mouse
            var toMouse = Vector2.Normalize(Input.MousePos - Spatial.MidVector);
            Spatial.rotation = MathHelper.ToDegrees((float)Math.Atan2(toMouse.Y, toMouse.X));

            // Shoot fireball with Left click
            if (Input.LeftMouseClicked())
            {
                var fireball = Prototypes.Create("Fireball", Spatial.MidVector);
                fireball.Spatial.pos -= fireball.Spatial.size / 2;
                fireball.Spatial.pos += toMouse * Spatial.size;
                fireball.GetComponent<PhysicsComponent>().velocity = toMouse * fireballSpeed;
            }
        }


        public override void OnCollision(Entity collidedWith)
        {
            base.OnCollision(collidedWith);

            // Define your custom collision behaviour here.
        }
    }
}
