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
        }
    }
}
