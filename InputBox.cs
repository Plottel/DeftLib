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
            spriteBatch.DrawString(Deft.Font10, label, pos.Add(5), Color.Black);
            spriteBatch.DrawRectangle(_stringRect, Color.Black, 1);
            spriteBatch.DrawString(Deft.Font10, _text, _stringRect.Location.ToVector2().Add(3), Color.Black);
        }
    }
}
