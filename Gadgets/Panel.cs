﻿using System;
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
        // TODO: Make private
        private Rectangle _dragRect;
        private bool _isBeingDragged;
        private Point _dragPoint;
        protected List<Gadget> _gadgets;
        private Vector2 _nextGadgetAt;

        public List<Gadget> Gadgets
        {
            get { return _gadgets; }
        }

        private const int PADDING_BETWEEN_GADGETS = 0;

        // Default constructor for Reflection instantiation
        public Panel() : this("", Vector2.Zero, Vector2.Zero, 1)
        { }

        public Panel(int layer) : this ("", Vector2.Zero, Vector2.Zero, layer)
        {}

        public Panel(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        {
            _nextGadgetAt = new Vector2(20, 35);

            Vector2 labelSize = Deft.Font14.MeasureString(label);
            labelSize.X = size.X;
            labelSize.Y += 10;

            _dragRect = new Rectangle(pos.ToPoint(), labelSize.ToPoint());
            _isBeingDragged = false;
            _gadgets = new List<Gadget>();
        }

        public void ClearGadgets()
        {
            _gadgets.Clear();
            _nextGadgetAt = new Vector2(20, 35);
        }

        public void AddGadget<T>(string label) where T : Gadget
        {
            T newGadget = (T)Activator.CreateInstance(typeof(T), this.Layer + 1); // Panels are "behind" their gadgets.
            newGadget.label = label;
            newGadget.MoveTo(pos + _nextGadgetAt);

            _gadgets.Add(newGadget);

            _nextGadgetAt.Y += newGadget.size.Y + PADDING_BETWEEN_GADGETS;

            if (_nextGadgetAt.Y > pos.Y + size.Y)
                size.Y = _nextGadgetAt.Y - pos.Y + 10; 
        }

        public T GetGadget<T>(string label) where T : Gadget
        {
            return _gadgets.Find(g => g.label == label) as T;
        }

        public override void MoveTo(Vector2 newPos)
        {
            foreach (var g in _gadgets)
                g.MoveTo(g.pos + (newPos - pos));


            base.MoveTo(newPos);

            _dragRect.X = (int)newPos.X;
            _dragRect.Y = (int)newPos.Y;
        }

        public override void MoveBy(Vector2 amt)
        {
            base.MoveBy(amt);

            _dragRect.X += (int)amt.X;
            _dragRect.Y += (int)amt.Y;

            foreach (var g in _gadgets)
                g.MoveBy(amt);
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

            foreach (var g in _gadgets)
                g.Render(spriteBatch);
        }
    }
}
