using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class PhysicsComponent : Component
    {
        private List<Vector2> _activeForces = new List<Vector2>();
        public Vector2 velocity = Vector2.Zero;
        public float mass;
        public float drag;
        public bool canReceiveForces;        

        public List<Vector2> ActiveForces
        {
            get { return _activeForces; }
        }

        public void AddForce(Vector2 force)
        {
            if (canReceiveForces)
                _activeForces.Add(force);
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteVector2(velocity);
            writer.Write(mass);
            writer.Write(drag);
            writer.Write(canReceiveForces);
        }

        public override void Deserialize(BinaryReader reader)
        {
            velocity = reader.ReadVector2();
            mass = reader.ReadSingle();
            drag = reader.ReadSingle();
            canReceiveForces = reader.ReadBoolean();
        }
    }
}
