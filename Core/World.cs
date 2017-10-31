using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.Reflection;
using System.IO;

namespace DeftLib
{
    public static class World
    {
        public static List<Entity> entities;
        public static List<Gadget> gadgets;
        public static ProgramStatePanel programStatePanel;
        private static Stack<GameState> _gameStates;

        public static TileGrid tileGrid;

        static World()
        {
            entities = new List<Entity>();
            gadgets = new List<Gadget>();
            _gameStates = new Stack<GameState>();
            tileGrid = new TileGrid(new Vector2(0, 0), 45, 23);

            _gameStates.Push(new InEditorMenuGameState());
            CurrentGameState.Enter();

            programStatePanel = new ProgramStatePanel("Program State", new Vector2(1050, 10), new Vector2(300, 200));
            GUIEventHub.Subscribe(programStatePanel);
        }

        private static GameState CurrentGameState
        {
            get { return _gameStates.Peek(); }
        }

        public static Type CurrentStateType
        {
            get { return _gameStates.Peek().GetType(); }
        }
        
        public static void PushState(GameState newState)
        {
            // Don't push to a new version of the current state
            if (newState.GetType() == CurrentGameState.GetType())
                return;

            CurrentGameState.Exit();
            _gameStates.Push(newState);
            CurrentGameState.Enter();
        }

        public static void PopState()
        {
            CurrentGameState.Exit();
            _gameStates.Pop();
            CurrentGameState.Enter();
        }

        public static void LoadWorld()
        {
            if (!File.Exists("world.bin"))
                return;

            using (BinaryReader reader = new BinaryReader(File.Open("world.bin", FileMode.Open)))
            {
                LoadEntities(reader);
                LoadTileGrid(reader);
            }

            Assets.LoadTileMaps();
            Prototypes.LoadPrototypes();

            #region Inner Load Methods
            void LoadEntities(BinaryReader reader)
            {
                entities.Clear();
                int numEntities = reader.ReadInt32();

                for (int i = 0; i < numEntities; ++i)
                {
                    Entity newEntity = new Entity();
                    newEntity.Deserialize(reader);
                    entities.Add(newEntity);
                    ECSCore.PlaceEntityInSystems(newEntity);
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

        public static void SaveWorld()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("world.bin", FileMode.OpenOrCreate)))
            {
                SaveEntities(writer);
                SaveTileGrid(writer);
            }

            Assets.SaveTileMaps();
            Prototypes.SavePrototypes();

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
       
        public static void HandleInput()
        {
            GUIEventHub.OnGUIEvent();
            CurrentGameState.HandleInput();
        }

        public static void Update(GameTime gameTime)
        {
            CurrentGameState.Update(gameTime);
        }

        public static void Render(SpriteBatch spriteBatch)
        {
            tileGrid.RenderTiles(spriteBatch);

            ECSCore.Render(spriteBatch);

            // TODO: Implement proper rendering ECS system
            foreach (var e in entities)
            {
                var spatial = e.GetComponent<SpatialComponent>();
                spriteBatch.FillRectangle(spatial.Bounds, Color.Blue);
            }

            CurrentGameState.Render(spriteBatch);

            programStatePanel.Render(spriteBatch);

            spriteBatch.DrawString(Deft.Font16, "Num Subscribed Gadgets: " + GUIEventHub.GadgetCount, new Vector2(800, 50), Color.Black);
            spriteBatch.DrawString(Deft.Font16, "Num Entities: " + entities.Count, new Vector2(800, 80), Color.Black);

            CurrentGameState.RenderGUI(spriteBatch);
        }
    }
}
