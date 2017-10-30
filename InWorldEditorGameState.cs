using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public class InWorldEditorGameState : GameState
    {
        private WorldEditorTileMapPanel _tilePanel;

        private Tile _tileMapToPlace;

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
            var grid = World.tileGrid;
            var idx = grid.IndexAt(tile.pos);

            var newMask = TileNeighbourDirection.None;

            // TODO: Fix crashes when changing tiles next to border.
            if (grid[idx.Col() - 1, idx.Row()].srcTexture == tile.srcTexture)
                newMask |= TileNeighbourDirection.West;
            if (grid[idx.Col() + 1, idx.Row()].srcTexture == tile.srcTexture)
                newMask |= TileNeighbourDirection.East;
            if (grid[idx.Col(), idx.Row() - 1].srcTexture == tile.srcTexture)
                newMask |= TileNeighbourDirection.North;
            if (grid[idx.Col(), idx.Row() + 1].srcTexture == tile.srcTexture)
                newMask |= TileNeighbourDirection.South;

            var textureName = Assets.GetTextureName(tile.srcTexture);

            if (textureName != "")
            {
                textureName = textureName.Replace("tilemap", "");
                tile.srcTextureRegion = Assets.GetTileMap(textureName)[newMask].srcTextureRegion;
            }
        }

        //
        // TODO : Take advantage of the fact we KNOW what the new mask for neighbours is.
        // It's the old mask plus whichever direction the new tile is at. Speed things up.
        //
        public override void HandleInput()
        {
            if (_tilePanel.Bounds.Contains(Input.MousePos))
            {
                if (Input.LeftMouseClicked())
                    _tileMapToPlace = _tilePanel.TileAtMousePos;
            }
            else if (Input.LeftMouseDown())
            {
                if (_tileMapToPlace != null)
                {
                    // Change tile texture to match what's being placed.
                    var worldTile = World.tileGrid.TileAt(Input.MousePos);
                    worldTile.srcTexture = _tileMapToPlace.srcTexture;

                    UpdateTextureRegionBasedOnNeighbours(worldTile);

                    foreach (var neighbour in World.tileGrid.GetNESWNeighboursAroundTile(worldTile))
                        UpdateTextureRegionBasedOnNeighbours(neighbour);
                }
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
            World.tileGrid.RenderGridLines(spriteBatch);
            _tilePanel.Render(spriteBatch);
        }
    }
}
