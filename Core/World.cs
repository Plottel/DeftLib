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

            _gameStates.Push(new InEditorGameState());
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
            entities.Clear();

            using (BinaryReader reader = new BinaryReader(File.Open("world.bin", FileMode.Open)))
            {
                int numEntities = reader.ReadInt32();

                for (int i = 0; i < numEntities; ++i)
                {
                    Entity newEntity = new Entity();
                    newEntity.Deserialize(reader);
                    entities.Add(newEntity);
                }
            }

            Assets.LoadTileMaps();
        }

        public static void SaveWorld()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("world.bin", FileMode.OpenOrCreate)))
            {
                writer.Write(entities.Count);

                foreach (Entity e in entities)
                    e.Serialize(writer);
            }

            Assets.SaveTileMaps();
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
            foreach (var e in entities)
            {
                var spatial = e.GetComponent<SpatialComponent>();
                spriteBatch.FillRectangle(spatial.Bounds, Color.Blue);
            }

            tileGrid.RenderTiles(spriteBatch);

            ECSCore.Render(spriteBatch);

            CurrentGameState.Render(spriteBatch);

            programStatePanel.Render(spriteBatch);

            spriteBatch.DrawString(Deft.Font16, "Num Subscribed Gadgets: " + GUIEventHub.GadgetCount, new Vector2(800, 50), Color.Black);
            spriteBatch.DrawString(Deft.Font16, "Num Entities: " + entities.Count, new Vector2(800, 80), Color.Black);

            CurrentGameState.RenderGUI(spriteBatch);



        }
    }
}
