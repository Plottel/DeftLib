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
    public class SpatialComponentEditorTool : ComponentEditorTool
    {
        private enum ToolState
        {
            Neutral,
            Moving,
            Resizing
        }


        private SpatialComponent _editing;
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
            _editing = e.GetComponent<SpatialComponent>();

            _resizePts[0] = new Vector2(_editing.pos.X, _editing.pos.Y + (_editing.size.Y / 2));                    // Left
            _resizePts[1] = new Vector2(_editing.pos.X + (_editing.size.X / 2), _editing.pos.Y);                    // Top
            _resizePts[2] = new Vector2(_editing.pos.X + _editing.size.X, _editing.pos.Y + (_editing.size.Y / 2));  // Right
            _resizePts[3] = new Vector2(_editing.pos.X + (_editing.size.X / 2), _editing.pos.Y + _editing.size.Y);  // Bottom

            _resizeRects[0] = new Rectangle(_resizePts[0].ToPoint(), new Point(10, 10));      // Left
            _resizeRects[1] = new Rectangle(_resizePts[1].ToPoint(), new Point(10, 10));      // Top
            _resizeRects[2] = new Rectangle(_resizePts[2].ToPoint(), new Point(10, 10));      // Right
            _resizeRects[3] = new Rectangle(_resizePts[3].ToPoint(), new Point(10, 10));      // Bottom

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
                    else if (_editing.Bounds.Contains(Input.MousePos))
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

            if (_selectedResizeRectIndex == 0)           // Left
            {
                _editing.pos.X += dx;
                _editing.size.X -= dx;
            }
            else if (_selectedResizeRectIndex == 1)      // Top
            {
                _editing.pos.Y += dy;
                _editing.size.Y -= dy;
            }
            else if (_selectedResizeRectIndex == 2)      // Right
            {
                _editing.size.X += dx;
            }
            else if (_selectedResizeRectIndex == 3)      // Bottom
            {
                _editing.size.Y += dy;
            }
        }

        private void HandleMove()
        {
            if (_editing.Bounds.Contains(Input.MousePos))
                _editing.pos += Input.DeltaMousePos;
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
            if (_editing != null)
            {
                spriteBatch.DrawRectangle(_editing.Bounds.GetInflated(2, 2), TOOL_COLOR, 2);

                foreach (var rect in _resizeRects)
                    spriteBatch.FillRectangle(rect, TOOL_COLOR);
            }
        }
    }
}
