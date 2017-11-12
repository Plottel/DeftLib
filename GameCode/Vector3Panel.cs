using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class Vector3Panel : Panel
    {
        // Change default values
        public const int DEFAULT_WIDTH = 200;
        public const int DEFAULT_HEIGHT = 100;

        #region Required Constructors
        // Default constructor for reflection instantiation
        public Vector3Panel() :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instantiation
        public Vector3Panel(int layer) :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public Vector3Panel(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        {
            // Add Field Gadgets here.
            AddGadget<FloatBox>("X");
            AddGadget<FloatBox>("Y");
            AddGadget<FloatBox>("Z");
        }
        #endregion Required Constructors        

        public Vector3 Value
        {
            // Return a variable matching the data type this Field Panel corresponds to.
            get
            {
                return new Vector3
                    (
                        GetGadget<FloatBox>("X").Value,
                        GetGadget<FloatBox>("Y").Value,
                        GetGadget<FloatBox>("Z").Value
                    );
            }
        }

        public override void SyncGadget(object toAttach)
        {
            // Update the gadgets in this panel according to the passed in object.
            Vector3 v = (Vector3)toAttach;

            if (v == null)
                throw new Exception("Invalid data type");

            GetGadget<FloatBox>("X").SyncGadget(v.X);
            GetGadget<FloatBox>("Y").SyncGadget(v.Y);
            GetGadget<FloatBox>("Z").SyncGadget(v.Z);
        }

    }
}
