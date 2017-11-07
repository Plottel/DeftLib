using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    // NOTE: Not sure what is rendering Tile Grid - IF ANYTHING
    public class Scene
    {
        public string Name;
        public List<Entity> entities;
        public TileGrid tileGrid;

        public Scene(string name)
        {
            Name = name;
            entities = new List<Entity>();
            tileGrid = new TileGrid(new Vector2(0, 0), 45, 23);
        }

        public void Serialize()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("Scenes/" + Name + "_scene.bin", FileMode.OpenOrCreate)))
            {
                SaveEntities(writer);
                SaveTileGrid(writer);
            }

            #region Inner Save Methods
            void SaveEntities(BinaryWriter writer)
            {
                writer.Write(entities.Count);

                foreach (Entity e in entities)
                    e.Serialize(writer);
            }

            void SaveTileGrid(BinaryWriter writer)
            {
                writer.WriteVector2(tileGrid.Pos);
                writer.Write(tileGrid.Cols);
                writer.Write(tileGrid.Rows);

                foreach (var tile in tileGrid)
                    tile.Serialize(writer);

            }
            #endregion Inner Save Methods
        }

        public void Deserialize()
        {
            using (BinaryReader reader = new BinaryReader(File.Open("Scenes/" + Name + "_scene.bin", FileMode.Open)))
            {
                LoadEntities(reader);
                LoadTileGrid(reader);
            }

            foreach (var e in entities)
                ECSCore.UnsubscribeEntity(e);

            #region Inner Load Methods
            void LoadEntities(BinaryReader reader)
            {
                entities.Clear();
                int numEntities = reader.ReadInt32();

                for (int i = 0; i < numEntities; ++i)
                {
                    var entityTypeString = reader.ReadString();
                    Type entityType = Type.GetType("DeftLib." + entityTypeString, true, true);
                    Entity newEntity = (Entity)Activator.CreateInstance(entityType);

                    newEntity.Deserialize(reader);
                    entities.Add(newEntity);
                    ECSCore.SubscribeEntity(newEntity);
                }
            }

            void LoadTileGrid(BinaryReader reader)
            {
                Vector2 pos = reader.ReadVector2();
                int cols = reader.ReadInt32();
                int rows = reader.ReadInt32();

                tileGrid = new TileGrid(pos, cols, rows);

                for (int col = 0; col < cols; ++col)
                {
                    for (int row = 0; row < rows; ++row)
                        tileGrid[col, row].Deserialize(reader);
                }

            }
            #endregion Inner Load Methods
        }

        public void Enter()
        {
            foreach (var e in entities)
                ECSCore.SubscribeEntity(e);
        }

        public void Exit()
        {
            foreach (var e in entities)
                ECSCore.UnsubscribeEntity(e);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            tileGrid.RenderTiles(spriteBatch);
        }

        public void RenderGUI(SpriteBatch spriteBatch)
        {
        }

    }
}
