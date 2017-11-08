using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DeftLib
{
    public class ShieldButton : Button
    {
        private string _originalLabel;
        private bool _isConfirming;


        // Default constructor for reflection instantiation
        public ShieldButton() :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instantiation
        public ShieldButton(int layer) :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public ShieldButton(string label, Vector2 pos, Vector2 size, int layer) :
            base(label, pos, size, layer)
        {
            _isConfirming = false;
        }

        public override void OnGUIEvent()
        {
            base.OnGUIEvent();

            if (_isConfirming && Input.KeyTyped(Keys.N))
            {
                _isConfirming = false;
                State = BtnGadgetState.Neutral;
            }            

            if (State == BtnGadgetState.Clicked)
            {
                if (_isConfirming)
                {
                    _isConfirming = false;
                }
                else
                {
                    State = BtnGadgetState.Neutral;
                    _isConfirming = true;
                }
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            _originalLabel = label;
            if (_isConfirming)
                label = "CONFIRM ? (Click / N)";
            base.Render(spriteBatch);
            label = _originalLabel;
        }
    }
}
