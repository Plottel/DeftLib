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
    public class MovementComponentEditorTool : ComponentEditorTool
    {
        private MovementComponent _movement;
        private SpatialComponent _spatial;
        private Vector2 _movementVector;
        private CircleF _movementVectorCircle;
        private const int CIRCLE_RADIUS = 100;


        public override void Edit(Entity e)
        {
            _movement = e.GetComponent<MovementComponent>();
            _spatial = e.GetComponent<SpatialComponent>();

            _movementVectorCircle = new CircleF(
                _spatial.MidVector, 
                CIRCLE_RADIUS);

            if (Input.LeftMouseDown() && _movementVectorCircle.Contains(Input.MousePos))
            {
                _movementVector = Input.MousePos - _spatial.MidVector;
                _movementVector.Normalize();
                _movement.direction = _movementVector;
            }
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
            if (_movement != null)
            {
                spriteBatch.DrawCircle(_movementVectorCircle, 100, TOOL_COLOR, 2);

                spriteBatch.DrawLine(_spatial.MidVector,
                    _spatial.MidVector + (_movementVector * CIRCLE_RADIUS), 
                    TOOL_COLOR, 
                    5);

            }
        }
    }
}
