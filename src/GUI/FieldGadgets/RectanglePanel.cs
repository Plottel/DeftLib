using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class RectanglePanel : Panel
    {
        private IntBox _xBox, _yBox, _widthBox, _heightBox;

        // Default constructor for reflection instantiation
        public RectanglePanel() : 
            this("", Vector2.Zero, new Vector2(IntBox.DEFAULT_WIDTH + 30, IntBox.DEFAULT_HEIGHT * 2 + PADDING_BETWEEN_GADGETS), 1)
        { }

        // Layer constructor for reflection instantiation
        public RectanglePanel(int layer) : 
            this("", Vector2.Zero, new Vector2(IntBox.DEFAULT_WIDTH + 30, IntBox.DEFAULT_HEIGHT * 2 + PADDING_BETWEEN_GADGETS), layer)
        { }

        public RectanglePanel(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        {
            AddGadget<IntBox>("X");
            AddGadget<IntBox>("Y");
            AddGadget<IntBox>("Width");
            AddGadget<IntBox>("Height");

            _xBox = GetGadget<IntBox>("X");
            _yBox = GetGadget<IntBox>("Y");
            _widthBox = GetGadget<IntBox>("Width");
            _heightBox = GetGadget<IntBox>("Height");
        }

        public override void SyncGadget(object toAttach)
        {
            var rect = (Rectangle)toAttach;

            _xBox.SyncGadget(rect.X);
            _yBox.SyncGadget(rect.Y);
            _widthBox.SyncGadget(rect.Width);
            _heightBox.SyncGadget(rect.Height);
        }

        public Rectangle Value
        {
            get
            {
                return new Rectangle(
                    _xBox.Value, 
                    _yBox.Value, 
                    _widthBox.Value, 
                    _heightBox.Value
                );
            }
        }

    }
}
