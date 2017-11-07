using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace DeftLib
{
    public class CollisionComponentEditorTool : ComponentEditorTool
    {
        private enum ToolState
        {
            Neutral,
            Moving,
            Resizing
        }

        private CollisionComponent _editing;
        private Vector2[] _resizePts = new Vector2[4];
        private Rectangle[] _resizeRects = new Rectangle[4];
        private int _selectedResizeRectIndex;

        private ToolState _state = ToolState.Neutral;

        private bool MouseOnResizeRect
        {
            get
            {
                foreach (var rect in _resizeRects)
                {
                    if (rect.Contains(Input.MousePos))
                        return true;
                }
                return false;
            }
        }

        public override void Edit(Entity e)
        {
            _editing = e.GetComponent<CollisionComponent>();
            var HB = _editing.HitBox;

            _resizePts[0] = new Vector2(HB.Left, HB.Top + (HB.Height / 2));                     // Left
            _resizePts[1] = new Vector2(HB.Left + (HB.Width / 2), HB.Top);                      // Top
            _resizePts[2] = new Vector2(HB.Left + HB.Width, HB.Top + (HB.Height / 2));          // Right
            _resizePts[3] = new Vector2(HB.Left + (HB.Width / 2), HB.Top + HB.Height);          // Bottom

            _resizeRects[0] = new Rectangle(_resizePts[0].ToPoint(), new Point(10, 10));        // Left
            _resizeRects[1] = new Rectangle(_resizePts[1].ToPoint(), new Point(10, 10));        // Top
            _resizeRects[2] = new Rectangle(_resizePts[2].ToPoint(), new Point(10, 10));        // Right
            _resizeRects[3] = new Rectangle(_resizePts[3].ToPoint(), new Point(10, 10));        // Bottom

            // Move rects to sit where they should
            for (int i = 0; i < 4; ++i)
                _resizeRects[i].Location -= new Point(5, 5);


            if (_state == ToolState.Neutral)
            {
                // Which state? Move or Resize?
                if (Input.LeftMousePressed())
                {
                    if (MouseOnResizeRect)
                    {
                        _state = ToolState.Resizing;
                        _selectedResizeRectIndex = GetSelectedResizeRectIndex();
                    }
                    else if (HB.Contains(Input.MousePos))
                        _state = ToolState.Moving;
                }
            }
            else if (_state == ToolState.Moving)
                HandleMove();
            else if (_state == ToolState.Resizing)
                HandleResize();

            if (Input.LeftMouseClicked())
                _state = ToolState.Neutral;
        }

        private int GetSelectedResizeRectIndex()
        {
            for (int i = 0; i < 4; ++i)
            {
                if (_resizeRects[i].Contains(Input.MousePos))
                    return i;
            }
            return int.MaxValue; // Default for none of them being selected
        }

        private void HandleResize()
        {
            var dx = Input.DeltaMousePos.X;
            var dy = Input.DeltaMousePos.Y;

            var HB = _editing.HitBox;

            if (_selectedResizeRectIndex == 0)           // Left
            {
                HB.X += (int)dx;
                HB.Width -= (int)dx;
            }
            else if (_selectedResizeRectIndex == 1)      // Top
            {
                HB.Y += (int)dy;
                HB.Height -= (int)dy;
            }
            else if (_selectedResizeRectIndex == 2)      // Right
            {
                HB.Width += (int)dx;
            }
            else if (_selectedResizeRectIndex == 3)      // Bottom
            {
                HB.Height += (int)dy;
            }

            _editing.HitBox = HB;
        }

        private void HandleMove()
        {
            if (_editing.HitBox.Contains(Input.MousePos))
                _editing.HitBox.Location += Input.DeltaMousePos.ToPoint();
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
            if (_editing != null)
            {
                spriteBatch.DrawRectangle(_editing.HitBox.GetInflated(2, 2), TOOL_COLOR, 2);

                foreach (var rect in _resizeRects)
                    spriteBatch.FillRectangle(rect, TOOL_COLOR);
            }
        }
    }
}
