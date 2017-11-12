using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class FieldPanelTemplate : Panel
    {
        // Change default values
        public const int DEFAULT_WIDTH = 0;
        public const int DEFAULT_HEIGHT = 0;

        #region Required Constructors
        // Default constructor for reflection instantiation
        public FieldPanelTemplate() :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instantiation
        public FieldPanelTemplate(int layer) :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public FieldPanelTemplate(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        {
        }
        #endregion Required Constructors        

        public object Value
        {
            // Return a variable matching the data type this Field Panel corresponds to.
            get { return new object(); }
        }

        public override void SyncGadget(object toAttach)
        {
            // Update the gadgets in this panel according to the passed in object.
        }

    }
}
