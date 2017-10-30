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
        // TODO: Make private
        private Rectangle _dragRect;
        private bool _isBeingDragged;
        private Point _dragPoint;
        protected List<Gadget> _gadgets;
        private Vector2 _nextGadgetAt;

        private Gadget _activeGadget;

        protected Gadget ActiveGadget
        {
            get { return _activeGadget; }
        }

        protected Vector2 NextGadgetAt
        {
            get { return _nextGadgetAt; }
        }

        public List<Gadget> Gadgets
        {
            get { return _gadgets; }
        }

        protected const int PADDING_BETWEEN_GADGETS = 0;

        // Default constructor for reflection instantiation
        public Panel() : 
            this("", Vector2.Zero, Vector2.Zero, 1)
        { }

        // Layer constructor for reflection instnatiation
        public Panel(int layer) : 
            this("", Vector2.Zero, Vector2.Zero, layer)
        { }

        public Panel(string label, Vector2 pos, Vector2 size, int layer) : 
            base(label, pos, size, layer)
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
            foreach (var g in _gadgets)
                GUIEventHub.Unsubscribe(g);

            _gadgets.Clear();
            _nextGadgetAt = new Vector2(20, 35);
        }

        public override void SyncGadget(object toAttach) { }

        public void AddGadget<T>(string label) where T : Gadget
        {
            T newGadget = (T)Activator.CreateInstance(typeof(T), this.layer + 1); // Panels are "behind" their gadgets.
            newGadget.label = label;
            newGadget.MoveTo(pos + _nextGadgetAt);

            _gadgets.Add(newGadget);

            _nextGadgetAt.Y += newGadget.size.Y + PADDING_BETWEEN_GADGETS;

            if (_nextGadgetAt.Y > pos.Y + size.Y - 35)
                size.Y = _nextGadgetAt.Y - pos.Y + 50;
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

        private bool MouseOnDragRect
        {
            get { return _dragRect.Contains(Input.MousePos); }
        }

        /// <summary>
        /// Core GUIEvent handling method
        /// </summary>
        public override void OnGUIEvent()
        {
            HandleOuterPanelEvent();
            HandleInnerGadgetEvent();
        }

        private void HandleOuterPanelEvent()
        {
            // Handle active gadget selection
            if (Input.LeftMouseClicked())
            {
                _isBeingDragged = false;                
            }

            // Handle panel dragging
            if (Input.LeftMouseDown())
            {
                _activeGadget = null;

                foreach (var g in _gadgets)
                {
                    if (g.Bounds.Contains(Input.MousePos))
                    {
                        _activeGadget = g;
                        break;
                    }
                }


                if (!_isBeingDragged)
                {
                    if (MouseOnDragRect)
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

        private void HandleInnerGadgetEvent()
        {
            if (Input.LeftMouseDown() && MouseOnDragRect)
                return;

            if (_activeGadget != null)
                _activeGadget.OnGUIEvent();
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(pos, size, Color.LightGray);
            spriteBatch.DrawString(Deft.Font12, label, pos.Add(5), Color.Black);

            foreach (var g in _gadgets)
                g.Render(spriteBatch);

            if (_activeGadget != null)
                spriteBatch.DrawRectangle(_activeGadget.Bounds.GetInflated(2, 2), Color.Blue, 2);

            // If a top layer panel, draw border.
            if (layer == 1)
            {
                spriteBatch.DrawRectangle(Bounds.GetInflated(2, 2), Color.Blue, 2);
                spriteBatch.DrawRectangle(_dragRect.GetInflated(2, 2), Color.Blue, 2);
            }                
        }

        protected static void RenderSideText(SpriteBatch spriteBatch, List<string> text, Panel instance)
        {
            int sideTextBoxHeight = Math.Max(text.Count * 20, 150);
            float sideTextBoxWidth = 0;
            // Width dictated by longest string.
            foreach (string s in text)
            {
                var stringWidth = Deft.Font14.MeasureString(s).X;

                if (stringWidth > sideTextBoxWidth)
                    sideTextBoxWidth = stringWidth;
            }

            sideTextBoxWidth = Math.Max(sideTextBoxWidth, 100);

            Vector2 startPos = new Vector2(instance.pos.X + instance.size.X + 5, instance.pos.Y + 30);
            
            Rectangle sideTextBoxRect = new Rectangle(startPos.ToPoint(), new Point((int)sideTextBoxWidth + 20, sideTextBoxHeight));    

            spriteBatch.FillRectangle(sideTextBoxRect, Color.LightGray);
            spriteBatch.DrawRectangle(sideTextBoxRect, Color.Blue, 2);  

            foreach (string s in text)
            {
                spriteBatch.DrawString(Deft.Font14, s, startPos + new Vector2(10, 0), Color.Black);
                startPos.Y += 20;
            }
        }
    }
}
