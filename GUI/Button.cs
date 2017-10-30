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
    public class Button : Gadget
    {
        public const int DEFAULT_HEIGHT = 30;
        public const int DEFAULT_WIDTH = 200;

        private enum ButtonState
        {
            Neutral,
            Pressed,
            Clicked
        }

        private ButtonState _state;

        public bool IsClicked
        {
            get { return _state == ButtonState.Clicked; }
        }

        // Default constructor for reflection instantiation
        public Button() : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instantiation
        public Button(int layer) : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public Button(string label, Vector2 pos, Vector2 size, int layer)
        {
            this.layer = layer;
            this.pos = pos;
            this.size = size;
            this.label = label;
            this._state = ButtonState.Neutral;
        }

        public override void SyncGadget(object toAttach) { }

        public override void OnGUIEvent()
        {
            if (Bounds.Contains(Input.MousePos))
            {
                if (_state == ButtonState.Neutral)
                {
                    if (Input.LeftMousePressed())
                        _state = ButtonState.Pressed;
                }
                else if (_state == ButtonState.Pressed)
                {
                    if (Input.LeftMouseClicked())
                        _state = ButtonState.Clicked;
                }
                else if (_state == ButtonState.Clicked)
                {
                    _state = ButtonState.Neutral;
                }
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (_state == ButtonState.Pressed)
                spriteBatch.FillRectangle(Bounds, Color.DimGray);
            else
                spriteBatch.FillRectangle(Bounds, Color.DarkGray);

            spriteBatch.DrawRectangle(Bounds, Color.Blue, 2);
            spriteBatch.DrawString(Deft.Font10, label, pos.Add(5), Color.Black);
        }
    }
}
