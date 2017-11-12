using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.IO;

namespace DeftLib
{
    public class OriginalImageTileMapEditorPanel : Panel
    {
        public const int DEFAULT_WIDTH = 300;
        public const int DEFAULT_HEIGHT = 300;
            
        private StringBox _openOriginalTileMapStringBox;
        
        private List<Tile> _tiles;
        private Vector2 _tileStartPos;

        private List<String> _allOriginalTileMapNames;


        // Default constructor for reflection instantiation
        public OriginalImageTileMapEditorPanel() 
            : this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instantiation
        public OriginalImageTileMapEditorPanel(int layer) 
            : this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public OriginalImageTileMapEditorPanel(string label, Vector2 pos, Vector2 size, int layer) 
            : base(label, pos, size, layer)
        {
            AddGadget<StringBox>("Open");
            _openOriginalTileMapStringBox = GetGadget<StringBox>("Open");
            _tiles = new List<Tile>();
            _tileStartPos = NextGadgetAt + new Vector2(50, 20);

            // XNB = Monogame XNA compiled texture file format?
            _allOriginalTileMapNames = Directory.GetFiles("Content/TileMaps/", "*.xnb").Select(Path.GetFileNameWithoutExtension).ToList();
        }

        public Tile TileAtMousePos
        {
            get
            {
                // TODO: Store tiles in a grid so we don't have to iterate here.
                foreach (var t in _tiles)
                {
                    if (t.Bounds.Contains(Input.MousePos))
                        return t;
                }
                return null;
            }
        }

        public override void OnGUIEvent()
        {
            base.OnGUIEvent();

            if (Input.KeyTyped(Keys.Enter))
            {
                if (ActiveGadget == _openOriginalTileMapStringBox)
                {
                    // Resync the Tiles
                    var textureMapFilename = _openOriginalTileMapStringBox.Value + "TileMap";
                    var texture = Assets.GetTexture(textureMapFilename);

                    if (texture != null)
                        SyncGadget(texture);
                }
            }
        }

        // TODO: Remove hardcoded 32 pixel size
        public override void SyncGadget(object toAttach)
        {
            Debug.Assert(toAttach.GetType() == typeof(Texture2D));

            _tiles.Clear();

            var tileMapTexture = (Texture2D)toAttach;

            // Fetch num rows and num cols from texture
            // Create tiles from sub regions, positioning them 
            // Row by row with row length of 4.
            int numCols = tileMapTexture.Width / 32;
            int numRows = tileMapTexture.Height / 32;

            int curCol = 0;
            int curRow = 0;

            for (int textureCol = 0; textureCol < numCols; ++textureCol)
            {
                for (int textureRow = 0; textureRow < numRows; ++textureRow)
                {
                    // Create a tile from part of the Texture
                    var textureRegion = new Rectangle(textureCol * 32, textureRow * 32, 32, 32);
                    var tilePos = _tileStartPos + new Vector2(curCol * 32, curRow * 32);

                    var newTile = new Tile(tileMapTexture, 
                        textureRegion, 
                        tilePos, 
                        new Vector2(32, 32));

                    _tiles.Add(newTile);

                    ++curCol;

                    if (curCol == 4)
                    {
                        curCol = 0;
                        ++curRow;
                    }
                }
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);

            // Render textures
            foreach (var tile in _tiles)
                spriteBatch.Draw(tile.srcTexture, tile.Bounds, tile.srcTextureRegion, Color.White);

            // Render tile bounds
            foreach (var tile in _tiles)
                spriteBatch.DrawRectangle(tile.Bounds, Color.Black, 1);     

            if (ActiveGadget == GetGadget<StringBox>("Open"))
                RenderSideText(spriteBatch, _allOriginalTileMapNames, this);
        }
    }
}
