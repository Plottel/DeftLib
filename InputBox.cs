using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace DeftLib
{
    public abstract class InputBox : Gadget
    {
        private Rectangle _stringRect;
        private bool _isTyping;

        protected string _text;

        // Default constructor for Reflection instantiation
        public InputBox() : this("", Vector2.Zero, Vector2.Zero)
        { }

        public InputBox(string label, Vector2 pos, Vector2 size) : base(label, pos, size)
        {
            _stringRect = Rectangle.Empty;
            _stringRect.Width = (int)size.X / 2;
            _stringRect.Height = (int)size.Y - 10;
            _stringRect.X = (int)(pos.X + size.X - _stringRect.Width - 5);
            _stringRect.Y = (int)(pos.Y + 5);

            _isTyping = false;
            _text = "";
        }

        public override void MoveTo(Vector2 newPos)
        {
            base.MoveTo(newPos);

            _stringRect.X = (int)(newPos.X + size.X - _stringRect.Width - 5);
            _stringRect.Y = (int)(newPos.Y + 5);
        }

        public override void MoveBy(Vector2 amt)
        {
            base.MoveBy(amt);

            _stringRect.Location += amt.ToPoint();
        }

        public override void OnGUIEvent()
        {
            if (Input.LeftMousePressed())
                _isTyping = _stringRect.Contains(Input.MousePos.ToPoint());

            if (_isTyping)
            {

                if (Input.KeyTyped(Keys.Back) && _text.Length > 0)
                    _text = _text.Remove(_text.Length - 1);
                else
                {
                    foreach (var key in Input.TypedKeys)
                    {
                        _text += InputTextParser.ParseKeys(Input.TypedKeys);
                    }
                }
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(_stringRect, Color.Black, 1);

            if (_isTyping)
                spriteBatch.DrawRectangle(_stringRect, Color.Blue, 2);



            spriteBatch.DrawString(Deft.Font10, label, pos.Add(5), Color.Black);
            spriteBatch.DrawString(Deft.Font10, _text, _stringRect.Location.ToVector2().Add(3), Color.Black);
        }
    }
}
