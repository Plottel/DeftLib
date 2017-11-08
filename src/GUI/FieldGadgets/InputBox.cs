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
    public abstract class InputBox<T> : FieldGadget<T>
    {
        public const int DEFAULT_HEIGHT = 30;
        public const int DEFAULT_WIDTH = 170;

        private Rectangle _inputRect;
        protected string _text;

        // Default constructor for reflection instantiation
        public InputBox() : 
            this("", Vector2.Zero, Vector2.Zero, 1)
        { }

        // Layer constructor for reflection instantiation
        public InputBox(int layer) : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public InputBox(string label, Vector2 pos, Vector2 size, int layer) : 
            base(label, pos, size, layer)
        {
            _inputRect = Rectangle.Empty;
            _inputRect.Width = (int)size.X - 100;
            _inputRect.Height = (int)size.Y - 10;
            _inputRect.X = (int)(pos.X + 90);
            _inputRect.Y = (int)(pos.Y + 5);

            _text = "";
        }

        public string Text
        {
            get { return _text; }
            protected set { _text = value; }
        }
        
        public override void MoveTo(Vector2 newPos)
        {
            base.MoveTo(newPos);

            _inputRect.X = (int)(pos.X + 90);
            _inputRect.Y = (int)(newPos.Y + 5);
        }

        public override void MoveBy(Vector2 amt)
        {
            base.MoveBy(amt);
            _inputRect.Location += amt.ToPoint();
        }

        public override void OnGUIEvent()
        {
            if (Input.KeyTyped(Keys.Delete))
                _text = "";

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

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(_inputRect, Color.Black, 1);
            spriteBatch.DrawString(Deft.Font10, label, pos.Add(5), Color.Black);
            spriteBatch.DrawString(Deft.Font10, _text, _inputRect.Location.ToVector2().Add(3), Color.Black);
        }
    }
}
