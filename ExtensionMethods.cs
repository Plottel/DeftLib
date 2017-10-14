﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public static class ExtensionMethods
    {
        public static Rectangle GetInflated(this Rectangle rectangle, int horizontalAmount, int verticalAmount)
        {
            Rectangle inflated = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            inflated.Inflate(horizontalAmount, verticalAmount);

            return inflated;
        }

        public static Rectangle GetInflated(this Rectangle rectangle, float horizontalAmount, float verticalAmount)
        {
            Rectangle inflated = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            inflated.Inflate(horizontalAmount, verticalAmount);

            return inflated;
        }

        public static Vector2 TopLeft(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Left, rectangle.Top);
        }

        public static Vector2 BottomRight(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Right, rectangle.Bottom);
        }

        public static Vector2 Add(this Vector2 vec, int other)
        {
            vec.X += other;
            vec.Y += other;
            return vec;
        }

        public static Vector2 AddX(this Vector2 vec, int other)
        {
            vec.X += other;
            return vec;
        }

        public static Vector2 AddY(this Vector2 vec, int other)
        {
            vec.Y += other;
            return vec;
        }

        public static int Col(this Point pt)
        {
            return pt.X;
        }

        public static int Row(this Point pt)
        {
            return pt.Y;
        }

        public static void WriteVector2(this BinaryWriter writer, Vector2 v)
        {
            writer.Write(v.X);
            writer.Write(v.Y);
        }

        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }
    }
}
