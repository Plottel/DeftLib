using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class SpatialComponent : Component
    {
        public Vector2 pos;
        public Vector2 size;
        public float rotation;

        public void MoveBy(Vector2 amt)
        {
            pos += amt;
        }

        public void MoveByX(float amt)
            => MoveBy(new Vector2(amt, 0));

        public void MoveByY(float amt)
            => MoveBy(new Vector2(0, amt));

        public void ResizeBy(Vector2 amt)
        {
            size += amt;
        }

        public void ResizeByX(float amt)
            => ResizeBy(new Vector2(amt, 0));

        public void ResizeByY(float amt)
            => ResizeBy(new Vector2(0, amt));

        public Rectangle Bounds
        {
            get { return new Rectangle(pos.ToPoint(), size.ToPoint()); }
        }

        public Point MidPt
        {
            get { return new Point((int)pos.X + (int)size.X / 2, (int)pos.Y + (int)size.Y / 2); }
        }

        public Vector2 MidVector
        {
            get { return new Vector2(pos.X + size.X / 2, pos.Y + size.Y / 2); }
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteVector2(pos);
            writer.WriteVector2(size);
            writer.Write(rotation);
        }

        public override void Deserialize(BinaryReader reader)
        {
            pos = reader.ReadVector2();
            size = reader.ReadVector2();
            rotation = reader.ReadSingle();
        }
    }
}
