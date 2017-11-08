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
    public static class SceneManager
    {
        public static List<Scene> scenes;

        private static Scene _currentScene;

        public static Scene Scene { get => _currentScene; }


        public static List<Entity> toBeDestroyed;
        public static List<Gadget> gadgets;
        public static ProgramStatePanel programStatePanel;
        private static Stack<GameState> _gameStates;

        public static TileGrid tileGrid;

        public static void Init()
        {
            scenes = new List<Scene>();
            toBeDestroyed = new List<Entity>();
            gadgets = new List<Gadget>();
            _gameStates = new Stack<GameState>();
            tileGrid = new TileGrid(new Vector2(0, 0), 45, 23);

            programStatePanel = new ProgramStatePanel("Program State", new Vector2(1050, 10), new Vector2(300, 200));

            _gameStates.Push(new InEditorMenuGameState());
            CurrentGameState.Enter();

            GUIEventHub.Subscribe(programStatePanel);
        }

        public static List<string> AllSceneNames
        {
            get
            {
                var result = new List<string>();
                foreach (var s in scenes)
                    result.Add(s.Name);
                return result;
            }
        }

        public static void AddEntity(Entity e)
        {
            _currentScene.entities.Add(e);
            ECSCore.SubscribeEntity(e);
        }

        public static void RemoveEntity(Entity e)
        {
            _currentScene.entities.Remove(e);
            ECSCore.UnsubscribeEntity(e);
        }

        public static void AddScene(Scene scene)
            => scenes.Add(scene);

        public static void ChangeScene(string name)
        {
            var newScene = scenes.Find(s => s.Name == name);

            if (newScene != null)
            {
                if (_currentScene != null)
                    _currentScene.Exit();

                _currentScene = newScene;
                _currentScene.Enter();

                programStatePanel.GetGadget<StringBox>("Scene Name").SyncGadget(_currentScene.Name);
            }
        }

        public static void DeleteCurrentScene()
        {
            if (_currentScene == null)
                return;

            _currentScene.Exit();
            scenes.Remove(_currentScene);

            var filePath = "Scenes/" + _currentScene.Name + "_scene.bin";

            if (File.Exists(filePath))
                File.Delete(filePath);

            if (scenes.Count > 0)
                ChangeScene(scenes[0].Name);


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
            if (_currentScene != null)
                _currentScene.Exit();
            _currentScene = null;
            scenes.Clear();


            var allSceneNames = Directory.GetFiles("Scenes/", "*.bin").Select(Path.GetFileNameWithoutExtension);

            foreach (var sceneName in allSceneNames)
            {
                string nameWithoutAffixes = sceneName.Replace("_scene", "");

                var newScene = new Scene(nameWithoutAffixes);
                newScene.Deserialize();
                scenes.Add(newScene);
            }

            // If no scenes to load, create a default one.
            if (scenes.Count == 0)
                AddScene(new Scene("DEFAULT SCENE"));

            _currentScene = scenes[0];
            ChangeScene(scenes[0].Name);

            Assets.LoadTileMaps();
            Prototypes.LoadPrototypes();           
        }

        public static void SaveWorld()
        {
            foreach (var scene in scenes)
                scene.Serialize();

            Assets.SaveTileMaps();
            Prototypes.SavePrototypes();            
        }        
       
        public static void HandleInput()
        {
            float test;
            string good = "0.5";
            string bad = "0.";

            if (!float.TryParse(good, out test))
                throw new Exception();

            if (!float.TryParse(bad, out test))
                throw new Exception();



            GUIEventHub.OnGUIEvent();
            CurrentGameState.HandleInput();
        }

        public static void Update(GameTime gameTime)
        {
            CurrentGameState.Update(gameTime);

            // At the end of each tick, process all destruction requests.

            // TODO: Reincorporate destruction requests
            //foreach (var e in toBeDestroyed)
              //  Scene.entities.Remove(e);

            toBeDestroyed.Clear();
        }

        public static void Render(SpriteBatch spriteBatch)
        {
            ECSCore.Render(spriteBatch);
            CurrentGameState.Render(spriteBatch);
            Scene.Render(spriteBatch);
            Scene.RenderGUI(spriteBatch);
            programStatePanel.Render(spriteBatch);
            CurrentGameState.RenderGUI(spriteBatch);
        }
    }
}
