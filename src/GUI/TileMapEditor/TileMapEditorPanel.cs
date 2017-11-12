using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DeftLib
{
    using TileMapMap = Dictionary<string, Dictionary<TileNeighbourDirection, Tile>>;

    public class TileMapEditorPanel : Panel
    {
        private class TileMapDnDGadget
        {
            public Texture2D texture;
            public Vector2 pos;
            public Vector2 size;

            public void MoveBy(Vector2 amt)
                => pos += amt;

            public Rectangle Bounds
            {
                get { return new Rectangle(pos.ToPoint(), size.ToPoint()); }
            }
        }

        public const int DEFAULT_WIDTH = 300;
        public const int DEFAULT_HEIGHT = 400;

        private Dictionary<TileNeighbourDirection, Tile> _activeTileMap = new Dictionary<TileNeighbourDirection, Tile>();
        private Dictionary<TileNeighbourDirection, TileMapDnDGadget> _dragNDropGadgets = new Dictionary<TileNeighbourDirection, TileMapDnDGadget>();

        private Texture2D _gadgetTexture;
        private Vector2 _gadgetTexturePos;

        public Vector2 GadgetTextureAbsPos
        {
            get { return this.pos + _gadgetTexturePos; }
        }

        // Default constructor for reflection instantiation
        public TileMapEditorPanel() : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instnatiation
        public TileMapEditorPanel(int layer) : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public TileMapEditorPanel(string label, Vector2 pos, Vector2 size, int layer) : 
            base(label, pos, size, layer)
        {
            AddGadget<StringBox>("Open");
            AddGadget<StringBox>("Name");
            AddGadget<Button>("Save");
            AddGadget<Button>("Delete");
            AddGadget<Button>("New");

            _gadgetTexturePos = new Vector2(50, NextGadgetAt.Y + 10);           

            // Initialize drag and drop gadgets;
            var gSize = new Vector2(32, 32);

            _dragNDropGadgets[TileNeighbourDirection.None] = new TileMapDnDGadget { texture=null, pos= GadgetTextureAbsPos + new Vector2(107, 73), size=gSize };
            _dragNDropGadgets[TileNeighbourDirection.East] = new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(2, 107), size = gSize };
            _dragNDropGadgets[TileNeighbourDirection.North] = new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(36, 107), size = gSize };
            _dragNDropGadgets[TileNeighbourDirection.South] = new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(2, 73), size = gSize };
            _dragNDropGadgets[TileNeighbourDirection.West] = new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(36, 73), size = gSize };

            _dragNDropGadgets[TileNeighbourDirection.East | TileNeighbourDirection.North] = 
                new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(2, 36), size = gSize };

            _dragNDropGadgets[TileNeighbourDirection.East | TileNeighbourDirection.South] =
                new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(2, 2), size = gSize };

            _dragNDropGadgets[TileNeighbourDirection.East | TileNeighbourDirection.West] =
                new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(73, 107), size = gSize };

            _dragNDropGadgets[TileNeighbourDirection.North| TileNeighbourDirection.South] =
                new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(107, 107), size = gSize };

            _dragNDropGadgets[TileNeighbourDirection.North | TileNeighbourDirection.West] =
                new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(36, 36), size = gSize };

            _dragNDropGadgets[TileNeighbourDirection.South | TileNeighbourDirection.West] =
                new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(36, 2), size = gSize };

            _dragNDropGadgets[TileNeighbourDirection.South | TileNeighbourDirection.North | TileNeighbourDirection.West] =
                new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(107, 36), size = gSize };

            _dragNDropGadgets[TileNeighbourDirection.East | TileNeighbourDirection.West | TileNeighbourDirection.South] =
                new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(107, 2), size = gSize };

            _dragNDropGadgets[TileNeighbourDirection.East | TileNeighbourDirection.North | TileNeighbourDirection.South] =
                new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(73, 2), size = gSize };

            _dragNDropGadgets[TileNeighbourDirection.East | TileNeighbourDirection.North | TileNeighbourDirection.West] =
                new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(73, 36), size = gSize };

            _dragNDropGadgets[TileNeighbourDirection.East | TileNeighbourDirection.North | TileNeighbourDirection.South | TileNeighbourDirection.West] =
                new TileMapDnDGadget { texture = null, pos = GadgetTextureAbsPos + new Vector2(73, 73), size = gSize };

            _gadgetTexture = Assets.GetTexture("TileMapGadgets");

            // Initialize a default tilemap
            ResetTileMap();
            
        }

        public override void MoveBy(Vector2 amt)
        {
            base.MoveBy(amt);

            foreach (var gadget in _dragNDropGadgets.Values)
                gadget.MoveBy(amt);
        }

        private void ResetTileMap()
        {
            _activeTileMap = new Dictionary<TileNeighbourDirection, Tile>();
            for (int i = 0; i < 16; ++i)
            {
                var dir = (TileNeighbourDirection)i;

                _activeTileMap[dir] = new Tile();
                _activeTileMap[dir].pos = _dragNDropGadgets[dir].pos;
            }

            GetGadget<StringBox>("Open").SyncGadget("");
            GetGadget<StringBox>("Name").SyncGadget("");
        }

        public override void OnGUIEvent()
        {
            base.OnGUIEvent();

            if (GetGadget<Button>("Save").IsClicked)
            {
                var tileMapName = GetGadget<StringBox>("Name").Value;
                Assets.AddTileMap(tileMapName, _activeTileMap);
                ResetTileMap();
            }

            if (GetGadget<Button>("Delete").IsClicked)
            {
                var tileMapName = GetGadget<StringBox>("Name").Value;
                Assets.DeleteTileMap(tileMapName);
                ResetTileMap();
            }

            if (GetGadget<Button>("New").IsClicked)
            {
                ResetTileMap();
                GetGadget<StringBox>("Name").SyncGadget("");
            }

            if (Input.KeyTyped(Keys.Enter))
            {
                if (ActiveGadget == GetGadget<StringBox>("Open"))
                {
                    string toOpenName = GetGadget<StringBox>("Open").Value;
                    var openedTileMap = Assets.GetTileMap(toOpenName);

                    if (openedTileMap != null)
                    {
                        _activeTileMap = openedTileMap;
                        GetGadget<StringBox>("Open").SyncGadget("");
                        GetGadget<StringBox>("Name").SyncGadget(toOpenName);
                    }
                }
            }
        }

        public void ClearTileAtMousePos()
        {
            TileNeighbourDirection directionToApplyTileTo = TileNeighbourDirection.None;
            bool mouseOnGadget = false;

            foreach (var gadget in _dragNDropGadgets)
            {
                if (gadget.Value.Bounds.Contains(Input.MousePos))
                {
                    directionToApplyTileTo = gadget.Key;
                    mouseOnGadget = true;
                    break;
                }

            }

            if (!mouseOnGadget)
                return;

            _activeTileMap[directionToApplyTileTo].srcTexture = null;
        }


        public void ApplyTileAtMousePos(Tile newTile)
        {
            TileNeighbourDirection directionToApplyTileTo = TileNeighbourDirection.None;
            bool mouseOnGadget = false;

            foreach (var gadget in _dragNDropGadgets)
            {
                if (gadget.Value.Bounds.Contains(Input.MousePos))
                {
                    directionToApplyTileTo = gadget.Key;
                    mouseOnGadget = true;
                    break;
                }
                    
            }

            if (!mouseOnGadget)
                return;

            SetTileForDirection(directionToApplyTileTo, newTile);
        }

        private void SetTileForDirection(TileNeighbourDirection direction, Tile newTile)
        {
            var gadgetPos = _activeTileMap[direction].pos;

            _activeTileMap[direction] = new Tile(newTile);
            _activeTileMap[direction].pos = gadgetPos;            
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);

            spriteBatch.Draw(_gadgetTexture, this.pos + _gadgetTexturePos, Color.White);

            foreach (var tileMap in _activeTileMap)
            {
                TileNeighbourDirection dir = tileMap.Key;
                Tile t = tileMap.Value;

                if (t.srcTexture != null)
                    spriteBatch.Draw(t.srcTexture, _dragNDropGadgets[dir].Bounds, t.srcTextureRegion, Color.White);
            }

            if (ActiveGadget == GetGadget<StringBox>("Open"))
                RenderSideText(spriteBatch, Assets.AllTileMapNames, this);
        }
    }
}
