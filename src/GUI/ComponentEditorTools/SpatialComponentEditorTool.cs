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
            Resizing,
            Rotating
        }


        private SpatialComponent _editing;
        private Vector2[] _resizePts = new Vector2[4];
        private Rectangle[] _resizeRects = new Rectangle[4];
        private Vector2 _rotationPt;
        private Rectangle _rotationRect;
        private Point HOOK_SIZE = new Point(10, 10);
        private Point HALF_HOOK_SIZE = new Point(5, 5);

        private Vector2 lengthVector;

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

            // Rotation point is a Vector projected outwards from center of Entity in direction
            // matching its rotation.
            // Rotation is a float expressed in degrees
            // Get a vector from an angle
            // V.x = cos(A)
            //V.y = sin(A)
            double radians = MathHelper.ToRadians(_editing.rotation);
            lengthVector = _editing.size * 1.5f;

            var normRotationVector = Vector2.Normalize(new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians)));
            _rotationPt = _editing.MidVector + (normRotationVector * lengthVector);
            _rotationRect = new Rectangle(_rotationPt.ToPoint() - HALF_HOOK_SIZE, HOOK_SIZE);

            _resizePts[0] = new Vector2(_editing.pos.X, _editing.pos.Y + (_editing.size.Y / 2));                    // Left
            _resizePts[1] = new Vector2(_editing.pos.X + (_editing.size.X / 2), _editing.pos.Y);                    // Top
            _resizePts[2] = new Vector2(_editing.pos.X + _editing.size.X, _editing.pos.Y + (_editing.size.Y / 2));  // Right
            _resizePts[3] = new Vector2(_editing.pos.X + (_editing.size.X / 2), _editing.pos.Y + _editing.size.Y);  // Bottom

            _resizeRects[0] = new Rectangle(_resizePts[0].ToPoint(), HOOK_SIZE);                            // Left
            _resizeRects[1] = new Rectangle(_resizePts[1].ToPoint(), HOOK_SIZE);                            // Top
            _resizeRects[2] = new Rectangle(_resizePts[2].ToPoint(), HOOK_SIZE);                            // Right
            _resizeRects[3] = new Rectangle(_resizePts[3].ToPoint(), HOOK_SIZE);                            // Bottom

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
                    else if (_rotationRect.Contains(Input.MousePos))
                        _state = ToolState.Rotating;
                }
            }
            else if (_state == ToolState.Moving)
                HandleMove();
            else if (_state == ToolState.Resizing)
                HandleResize();
            else if (_state == ToolState.Rotating)
                HandleRotate();

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
                _editing.MoveByX(dx);
                _editing.ResizeByX(-dx);
            }
            else if (_selectedResizeRectIndex == 1)      // Top
            {
                _editing.MoveByY(dy);
                _editing.ResizeByY(-dy);
            }
            else if (_selectedResizeRectIndex == 2)      // Right
            {
                _editing.ResizeByX(dx);
            }
            else if (_selectedResizeRectIndex == 3)      // Bottom
            {
                _editing.ResizeByY(dy);
            }
        }

        private void HandleMove()
        {
            if (_editing.Bounds.Contains(Input.MousePos))
                _editing.MoveBy(Input.DeltaMousePos);
        }

        private void HandleRotate()
        { 
            //      270
            //  180     0/360
            //      90

            // Get vector between mouse pos and spatial.MidVector
            // Convert that to an angle
            // Set rotation to that angle
            var angleVector = Vector2.Normalize(Input.MousePos - _editing.MidVector);
            var rotation = MathHelper.ToDegrees((float)Math.Atan2(angleVector.Y, angleVector.X));

            _editing.rotation = rotation;            
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
            if (_editing != null)
            {
                // Render bounds
                spriteBatch.DrawRectangle(_editing.Bounds.GetInflated(2, 2), TOOL_COLOR, 2);

                // Render resize points
                foreach (var rect in _resizeRects)
                    spriteBatch.FillRectangle(rect, Color.LightGoldenrodYellow);

                // Render rotation line
                spriteBatch.DrawLine(_editing.MidVector, _rotationPt, TOOL_COLOR, 2);
                spriteBatch.FillRectangle(_rotationRect, Color.LightGoldenrodYellow);
            }
        }
    }
}
