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
    public class Panel : Gadget
    {
        private Rectangle _dragRect;
        private bool _isBeingDragged;
        private Point _dragPoint;
        private List<Gadget> _gadgets;
        private Vector2 _nextGadgetAt;

        private const int PADDING_BETWEEN_GADGETS = 5;

        public Panel(string label, Vector2 pos, Vector2 size) : base(label, pos, size)
        {
            _nextGadgetAt = pos + new Vector2(5, 35);


            Vector2 labelSize = Deft.Font14.MeasureString(label);
            labelSize.X = size.X;
            labelSize.Y += 10;

            _dragRect = new Rectangle(pos.ToPoint(), labelSize.ToPoint());
            _isBeingDragged = false;
            _gadgets = new List<Gadget>();
        }

        public void AddGadget<T>(string label) where T : Gadget
        {
            T newGadget = (T)Activator.CreateInstance(typeof(T), label, _nextGadgetAt);
            _gadgets.Add(newGadget);

            _nextGadgetAt = _nextGadgetAt.AddY((int)newGadget.size.Y + PADDING_BETWEEN_GADGETS);
        }

        public T GetGadget<T>(string label) where T : Gadget
        {
            return _gadgets.Find(g => g.label == label) as T;
        }

        public void MoveBy(Vector2 amt)
        {
            pos += amt;

            _dragRect.X += (int)amt.X;
            _dragRect.Y += (int)amt.Y;
        }

        public override void OnGUIEvent()
        {
            if (Input.LeftMouseClicked())
                _isBeingDragged = false;
            else if (Input.LeftMouseDown())
            {
                if (!_isBeingDragged)
                {
                    if (_dragRect.Contains(Input.MousePos))
                    {
                        _dragPoint = Input.MousePos.ToPoint();
                        _isBeingDragged = true;
                    }
                }
                else
                {
                    MoveBy(new Vector2(Input.MouseX - _dragPoint.X, Input.MouseY - _dragPoint.Y));
                    _dragPoint = Input.MousePos.ToPoint();
                }    
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(pos, size, Color.LightGray);
            spriteBatch.DrawString(Deft.Font14, label, pos.Add(5), Color.Black);

            spriteBatch.DrawRectangle(_dragRect, Color.Black, 3);

            foreach (var g in _gadgets)
                g.Render(spriteBatch);
        }
    }
}
