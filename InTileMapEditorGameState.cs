using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    using OriginalPanel = OriginalImageTileMapEditorPanel;

    public class InTileMapEditorGameState : GameState
    {
        private OriginalPanel _originalPanel;
        private TileMapEditorPanel _tileMapPanel;

        private Tile _tileBeingDragged;

        public InTileMapEditorGameState()
        {
            _originalPanel = new OriginalPanel(
                "Original Texture",
                new Vector2(10, 10), 
                new Vector2(OriginalPanel.DEFAULT_WIDTH, OriginalPanel.DEFAULT_HEIGHT), 
                1
            );

            _tileMapPanel = new TileMapEditorPanel(
                "Tile Map", 
                new Vector2(450, 10), 
                new Vector2(TileMapEditorPanel.DEFAULT_WIDTH, TileMapEditorPanel.DEFAULT_HEIGHT), 
                1
            );
        }

        public override void Enter()
        {
            GUIEventHub.Subscribe(_originalPanel);
            GUIEventHub.Subscribe(_tileMapPanel);
        }

        public override void Exit()
        {
            GUIEventHub.Unsubscribe(_originalPanel);
            GUIEventHub.Unsubscribe(_tileMapPanel);
        }

        public override void HandleInput()
        {
            //
            // Left Mouse Press Event
            //
            if (Input.LeftMousePressed())
            {
                // Select a Tile to drag from the Original Image Panel
                if (_originalPanel.Bounds.Contains(Input.MousePos))
                {
                    var selectedTile = _originalPanel.TileAtMousePos;

                    if (selectedTile != null)
                    {
                        _tileBeingDragged = selectedTile;
                    }
                }
            }

            //
            // Left Mouse Click Event
            //
            if (Input.LeftMouseClicked())
            {
                if (_tileMapPanel.Bounds.Contains(Input.MousePos))
                {
                    if (_tileBeingDragged != null)
                    {
                        _tileMapPanel.ApplyTileAtMousePos(_tileBeingDragged);
                        _tileBeingDragged = null;
                    }
                }
            }

            //
            // Right Click Mouse Event
            //
            if (Input.RightMouseClicked())
            {
                if (_tileMapPanel.Bounds.Contains(Input.MousePos))
                    _tileMapPanel.ClearTileAtMousePos();
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Render(SpriteBatch spriteBatch)
        {           
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
            _originalPanel.Render(spriteBatch);
            _tileMapPanel.Render(spriteBatch);

            if (_tileBeingDragged != null)
                spriteBatch.Draw(_tileBeingDragged.srcTexture, Input.MousePos, _tileBeingDragged.srcTextureRegion, Color.White);
        }
    }
}
