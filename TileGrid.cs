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
    public class TileGrid : Grid<Tile>
    {
        private int _tileSize = 32; // TODO: Stop harcoding tile size

        private Vector2 _pos;
        public Vector2 Pos { get => _pos; }

        /// <summary>
        /// The width of the grid.
        /// -1 so the absolute edge doesn't register next cell and out of bounds.
        /// </summary>
        public int Width
        {
            get { return Cols * _tileSize - 1; }
        }

        /// <summary>
        /// The height of the grid.
        /// -1 so the absolute edge doesn't register next cell and out of bounds.
        /// </summary>
        public int Height
        {
            get { return Rows * _tileSize - 1; }
        }

        public Rectangle Bounds
        {
            get => new Rectangle((int)_pos.X, (int)_pos.Y, Width, Height);
        }

        public TileGrid(Vector2 pos, int cols, int rows)
        {
            _pos = pos;
            AddColumns(cols);
            AddRows(rows);
        }

        public void AddTileToCollectionIfNotNull(Tile tile, ICollection<Tile> list)
        {
            if (tile != null)
                list.Add(tile);
        }

        public Tile TileAt(Vector2 pos)
        {
            int col = (int)Math.Floor((pos.X - Pos.X) / _tileSize);
            int row = (int)Math.Floor((pos.Y - Pos.Y) / _tileSize);

            return this[col, row];
        }

        public List<Tile> GetSquareNeighboursAroundTile(Tile tile)
        {
            var idx = IndexAt(tile.pos);

            var result = new List<Tile>();

            for (int col = idx.Col() - 1; col <= idx.Col() + 1; ++col)
            {
                for (int row = idx.Row() - 1; row <= idx.Row() + 1; ++row)
                    AddTileToCollectionIfNotNull(this[col, row], result);
            }

            result.Remove(tile);
            return result;
        }

        public List<Tile> GetNESWNeighboursAroundTile(Tile tile)
        {
            var idx = IndexAt(tile.pos);

            var result = new List<Tile>();

            AddTileToCollectionIfNotNull(this[idx.Col() - 1, idx.Row()], result);       // West
            AddTileToCollectionIfNotNull(this[idx.Col() + 1, idx.Row()], result);       // East
            AddTileToCollectionIfNotNull(this[idx.Col(), idx.Row() - 1], result);       // North
            AddTileToCollectionIfNotNull(this[idx.Col(), idx.Row() + 1], result);       // South

            return result;
        }

        public Point IndexAt(Vector2 pos)
        {
            var col = (int)Math.Floor((pos.X - Pos.X) / _tileSize);
            var row = (int)Math.Floor((pos.Y - Pos.Y) / _tileSize);

            return new Point(col, row);
        }

        /// <summary>
        /// Adds columns to the grid.
        /// Columns are the outer list, so adding a column creates a new list and populates it.
        /// </summary>
        /// <param name="amountToAdd">The number of columns to add.</param>
        public void AddColumns(int amountToAdd)
        {
            // For each column to be added.
            for (int col = 0; col < amountToAdd; col++)
            {
                var newCol = new List<Tile>();

                // For each row in the newly created column.
                for (int row = 0; row < Rows; row++)
                {
                    // Add the new tile.
                    var newTile = new Tile();
                    newTile.pos = new Vector2(Pos.X + Cols * _tileSize, Pos.Y + Rows * _tileSize);
                    newTile.size = new Vector2(32, 32); // TODO: Stop harcoding tile size.

                    newCol.Add(newTile);
                }

                // Add the column to cells, increment number of columns in the grid.
                Cells.Add(newCol);
                ++Cols;
            }
        }

        /// <summary>
        /// Adds rows to the grid.
        /// Rows are the inner list, so adding a row appends a new cell to the end of each list.
        /// </summary>
        /// <param name="amountToAdd">The number of rows to add.</param>
        public void AddRows(int amountToAdd)
        {
            // For each row to be added.
            for (int row = 0; row < amountToAdd; row++)
            {
                // For each column to have a new row added to it.
                for (int col = 0; col < Cols; col++)
                {
                    // Add the new cell to the column.
                    var newTile = new Tile();
                    newTile.pos = new Vector2(Pos.X + col * _tileSize, Pos.Y + Rows * _tileSize);
                    newTile.size = new Vector2(32, 32);

                    Cells[col].Add(newTile);
                }

                // Increment the number of rows in the grid.
                ++Rows;
            }
        }

        // TODO : Implement IEnumerable and remove this method.
        public void RenderTiles(SpriteBatch spriteBatch)
        {
            foreach (var tile in this)
            {
                if (tile.srcTexture != null)
                    spriteBatch.Draw(tile.srcTexture, tile.Bounds, tile.srcTextureRegion, Color.White);
            }
        }

        public void RenderGridLines(SpriteBatch spriteBatch)
        {
            foreach (var tile in this)
                spriteBatch.DrawRectangle(tile.Bounds, Color.Black, 1);
        }
    }
}
