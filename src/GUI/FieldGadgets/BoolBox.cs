using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace DeftLib
{
    public class BoolBox : FieldGadget<bool>
    {
        public const int DEFAULT_HEIGHT = 30;
        public const int DEFAULT_WIDTH = 150;

        private bool _isChecked;
        private Rectangle _checkBoxRect;

        // Default constructor for reflection instantiation
        public BoolBox() :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instantiation
        public BoolBox(int layer) :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public BoolBox(string label, Vector2 pos, Vector2 size, int layer) :
            base(label, pos, size, layer)
        {
            _isChecked = false;
            _checkBoxRect = Rectangle.Empty;
            _checkBoxRect.Width = 10;
            _checkBoxRect.Height = 10;
            _checkBoxRect.X = (int)(pos.X + size.X - 25);
            _checkBoxRect.Y = (int)pos.Y + 10;
        }

        public override void MoveBy(Vector2 amt)
        {
            base.MoveBy(amt);
            _checkBoxRect.Location += amt.ToPoint();
        }

        public override void MoveTo(Vector2 newPos)
        {
            base.MoveTo(newPos);
            _checkBoxRect.X = (int)(pos.X + size.X - 25);
            _checkBoxRect.Y = (int)pos.Y + 10;
        }

        public override bool Value
        {
            get
            {
                return _isChecked;
            }
        }

        public override void SyncGadget(object toAttach)
        {
            bool newValue = (bool)toAttach;
            _isChecked = newValue;
        }

        public override void OnGUIEvent()
        {
            if (Input.LeftMouseClicked())
            {
                if (_checkBoxRect.Contains(Input.MousePos))
                    _isChecked = !_isChecked;
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Deft.Font10, label, pos.Add(5), Color.Black);
            spriteBatch.DrawRectangle(_checkBoxRect, Color.Blue, 2);

            if (_isChecked)
                spriteBatch.FillRectangle(_checkBoxRect.GetInflated(-2, -2), Color.Blue);
        }
    }
}
