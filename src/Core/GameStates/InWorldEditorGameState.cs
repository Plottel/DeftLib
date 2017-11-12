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
    public class InWorldEditorGameState : GameState
    {
        private enum ActionState
        {
            Painting,
            Deleting,
            None
        }

        private WorldEditorTileMapPanel _tilePanel;

        private Tile _tileMapToPlace;

        private bool _isBoxSelecting;
        private Rectangle _selectBox;

        private ActionState _state = ActionState.None;

        public InWorldEditorGameState()
        {
            _tilePanel = new WorldEditorTileMapPanel(
                "Choose Tiles", 
                new Vector2(10, 10), 
                new Vector2(WorldEditorTileMapPanel.DEFAULT_WIDTH, WorldEditorTileMapPanel.DEFAULT_HEIGHT), 
                1
            );
        }

        public override void Enter()
        {
            GUIEventHub.Subscribe(_tilePanel);
        }

        public override void Exit()
        {
            GUIEventHub.Unsubscribe(_tilePanel);
        }
        
        private void UpdateTextureRegionBasedOnNeighbours(Tile tile)
        {
            var grid = Scene.tileGrid;
            var idx = grid.IndexAt(tile.pos);

            var newMask = TileNeighbourDirection.None;

            // TODO: Fix crashes when changing tiles next to border.
            if (IsNeighbour(tile, grid[idx.Col() - 1, idx.Row()]))
                newMask |= TileNeighbourDirection.West;
            if (IsNeighbour(tile, grid[idx.Col() + 1, idx.Row()]))
                newMask |= TileNeighbourDirection.East;
            if (IsNeighbour(tile, grid[idx.Col(), idx.Row() - 1]))
                newMask |= TileNeighbourDirection.North;
            if (IsNeighbour(tile, grid[idx.Col(), idx.Row() + 1]))
                newMask |= TileNeighbourDirection.South;

            var textureName = Assets.GetTextureName(tile.srcTexture);

            if (textureName != "")
            {
                textureName = textureName.Replace("tilemap", "");
                tile.srcTextureRegion = Assets.GetTileMap(textureName)[newMask].srcTextureRegion;
            }
        }

        private bool IsNeighbour(Tile source, Tile potentialNeighbour)
        {
            if (source == null || potentialNeighbour == null)
                return false;

            return source.srcTexture == potentialNeighbour.srcTexture;
        }

        //
        // TODO : Take advantage of the fact we KNOW what the new mask for neighbours is.
        // It's the old mask plus whichever direction the new tile is at. Speed things up.
        //
        public override void HandleInput()
        {
            if (Input.KeyTyped(Microsoft.Xna.Framework.Input.Keys.LeftControl))
            {
                _isBoxSelecting = true;
            }

            if (Input.LeftMousePressed())
                _state = ActionState.Painting;
            else if (Input.RightMousePressed())
                _state = ActionState.Deleting;

            if (Input.LeftMousePressed() || Input.RightMousePressed())
                _selectBox = new Rectangle(Input.MouseX, Input.MouseY, 0, 0);

            // Handle Box Select input
            if (_isBoxSelecting)
            {
                if (Input.LeftMouseDown() || Input.RightMouseDown())
                    _selectBox.Size += Input.DeltaMousePos.ToPoint();
                else if (Input.LeftMouseClicked() || Input.RightMouseClicked())
                {
                    if (_tileMapToPlace != null)
                    {
                        var tilesInSelectBox = Scene.tileGrid.TilesInRect(_selectBox);

                        if (_state == ActionState.Painting)
                        {
                            // Paint in the rect
                            foreach (var t in tilesInSelectBox)
                                UpdateLocalTileMaps(t, _tileMapToPlace.srcTexture);
                        }
                        else if (_state == ActionState.Deleting)
                        {
                            // Delete in the rect
                            foreach (var t in tilesInSelectBox)
                                UpdateLocalTileMaps(t, null);
                        }
                    }                    

                    // Action finished, reset state
                    _state = ActionState.None;
                    _isBoxSelecting = false;
                    _selectBox = Rectangle.Empty;
                }
            }
            // Handle NON box select input
            else
            {
                if (_tilePanel.Bounds.Contains(Input.MousePos))
                {
                    if (Input.LeftMouseClicked())
                        _tileMapToPlace = _tilePanel.TileAtMousePos;
                }
                else if (Input.LeftMouseDown())
                {
                    if (_tileMapToPlace != null)
                        UpdateLocalTileMaps(Scene.tileGrid.TileAt(Input.MousePos), _tileMapToPlace.srcTexture);
                }
                else if (Input.RightMouseDown())
                    UpdateLocalTileMaps(Scene.tileGrid.TileAt(Input.MousePos), null);
            }            
        }

        private void UpdateLocalTileMaps(Tile changed, Texture2D newTexture)
        {
            changed.srcTexture = newTexture;
            UpdateTextureRegionBasedOnNeighbours(changed);

            foreach (var neighbour in Scene.tileGrid.GetNESWNeighboursAroundTile(changed))
                UpdateTextureRegionBasedOnNeighbours(neighbour);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Scene.tileGrid.RenderGridLines(spriteBatch);
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
            _tilePanel.Render(spriteBatch);

            if (_isBoxSelecting)
                spriteBatch.DrawRectangle(_selectBox, ComponentEditorTool.TOOL_COLOR, 2);
        }
    }
}
