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
    public class WorldEditorTileMapPanel : Panel
    {
        public const int DEFAULT_WIDTH = 200;
        public const int DEFAULT_HEIGHT = 300;
        private const int TILE_SPACING = 2;
        private const int NUM_TILE_ROWS = 5;

        private List<Tile> _tiles;

        private Vector2 _tileStartPos;

        public Tile TileAtMousePos
        {
            get
            {
                foreach (var t in _tiles)
                {
                    if (t.Bounds.Contains(Input.MousePos))
                        return t;
                }
                return null;
            }
        }

        // Default constructor for reflection instantiation
        public WorldEditorTileMapPanel() : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instantiation
        public WorldEditorTileMapPanel(int layer) : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public WorldEditorTileMapPanel(string label, Vector2 pos, Vector2 size, int layer) : 
            base(label, pos, size, layer)
        {
            _tiles = new List<Tile>();
            _tileStartPos = new Vector2(20, 50);

            // Fetch all possible tile maps and create gadgets to choose them.
            int curCol = 0;
            int curRow = 0;

            foreach (var tileMapName in Assets.AllTileMapNames)
            {
                // Copy asset tile and change pos to fit panel.
                var newTile = new Tile(Assets.GetTileMap(tileMapName)[TileNeighbourDirection.None]);
                newTile.pos = pos + _tileStartPos + new Vector2(curCol * (32 + TILE_SPACING), curRow * (32 + TILE_SPACING));

                _tiles.Add(newTile);

                ++curRow;

                if (curRow == NUM_TILE_ROWS)
                {
                    curRow = 0;
                    ++curCol;
                }
            }
        }

        public override void MoveBy(Vector2 amt)
        {
            base.MoveBy(amt);

            foreach (var tile in _tiles)
                tile.pos += amt;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);

            foreach (var tile in _tiles)
                spriteBatch.Draw(tile.srcTexture, tile.Bounds, tile.srcTextureRegion, Color.White);

            foreach (var tile in _tiles)
                spriteBatch.DrawRectangle(tile.Bounds.GetInflated(1, 1), Color.Black, 2);
        }
    }
}
