using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace DeftLib
{
    public static class Assets
    {
        private static Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, Dictionary<TileNeighbourDirection, Tile>> _tileMaps
            = new Dictionary<string, Dictionary<TileNeighbourDirection, Tile>>();

        public static ContentManager content;

        public static void LoadAssets()
        {
            AddTexture("TileMapGadgets", content.Load<Texture2D>("TileMapDragAndDropGadgets"));

            var allOriginalTileMapNames = Directory.GetFiles("Content/TileMaps/", "*.xnb").Select(Path.GetFileNameWithoutExtension).ToList();

            foreach (var tileMapName in allOriginalTileMapNames)
                AddTexture(tileMapName, content.Load<Texture2D>("TileMaps/" + tileMapName));

            LoadTileMaps();
        }

        public static List<String> AllTileMapNames
        {
            get
            {
                var result = new List<String>();
                foreach (string s in _tileMaps.Keys)
                    result.Add(s);

                return result;
            }
        }

        public static string GetTextureName(Texture2D texture)
        {
            foreach (var textureMap in _textures)
            {
                if (textureMap.Value == texture)
                    return textureMap.Key;
            }
            return "";
        }

        public static void LoadTileMaps()
        {
            _tileMaps.Clear();

            if (!File.Exists("tileMaps.bin"))
                return;

            using (BinaryReader reader = new BinaryReader(File.Open("tileMaps.bin", FileMode.Open)))
            {
                int numTileMaps = reader.ReadInt32();

                for (int i = 0; i < numTileMaps; ++i)
                {
                    string tileMapName = reader.ReadString();

                    var newTileMap = new Dictionary<TileNeighbourDirection, Tile>();

                    for (int neighbourDir = 0; neighbourDir < 16; ++neighbourDir)
                    {
                        var dir = (TileNeighbourDirection)reader.ReadInt32();

                        var newTile = new Tile();
                        newTile.srcTexture = GetTexture(reader.ReadString());
                        newTile.srcTextureRegion = reader.ReadRectangle();
                        newTile.pos = reader.ReadVector2();
                        newTile.size = reader.ReadVector2();

                        newTileMap[dir] = newTile;
                    }

                    // Tile map has been loaded, add to TileMapMap
                    _tileMaps[tileMapName] = newTileMap;
                }                
            }
        }

        public static void SaveTileMaps()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("tileMaps.bin", FileMode.OpenOrCreate)))
            {
                writer.Write(_tileMaps.Count);

                foreach (var tileMapMap in _tileMaps)
                {
                    string tileMapName = tileMapMap.Key;
                    writer.Write(tileMapName);

                    foreach (KeyValuePair<TileNeighbourDirection, Tile> tileMap in tileMapMap.Value)
                    {
                        TileNeighbourDirection dir = tileMap.Key;
                        Tile tile = tileMap.Value;

                        writer.Write((int)dir);

                        // Serialize tile
                        string tileTextureName = GetTextureName(tile.srcTexture);
                        writer.Write(tileTextureName);
                        writer.WriteRectangle(tile.srcTextureRegion);
                        writer.WriteVector2(tile.pos);
                        writer.WriteVector2(tile.size);
                    }
                }
            }
        }
        
        public static void AddTexture(string name, Texture2D texture)
        {
            var lowerName = name.ToLower();
            Debug.Assert(!_textures.ContainsKey(lowerName), "Tried to add texture which already existed : " + name);
            _textures[lowerName] = texture;
        }

        public static Texture2D GetTexture(string name)
        {
            var lowerName = name.ToLower();
            return _textures.ContainsKey(lowerName) ? _textures[lowerName] : default(Texture2D);
        }

        public static void AddTileMap(string name, Dictionary<TileNeighbourDirection, Tile> tileMap)
        {
            var lowerName = name.ToLower();
            Debug.Assert(!_tileMaps.ContainsKey(lowerName), "Tried to add TileMap with name that already existed : " + name);
            _tileMaps[lowerName] = tileMap;
        }

        public static Dictionary<TileNeighbourDirection, Tile> GetTileMap(string name)
        {
            var lowerName = name.ToLower();

            if (_tileMaps.ContainsKey(lowerName))
                return _tileMaps[lowerName];
            return null;
        }

        public static void DeleteTileMap(string name)
        {
            var lowerName = name.ToLower();

            if (_tileMaps.ContainsKey(lowerName))
                _tileMaps.Remove(lowerName);
        }
    }
}
