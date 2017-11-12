using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DeftLib
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Deft : Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        private static Random _rand = new Random();

        public static int RandInt(int min, int max)
            => _rand.Next(min, max);

        public static byte RandByte(byte min, byte max)
            => (byte)_rand.Next(min, max);

        private static Deft _instance;

        public static Deft Get
        {
            get { return _instance; }
        }

        public static SpriteFont Font8;
        public static SpriteFont Font6;
        public static SpriteFont Font10;
        public static SpriteFont Font12;
        public static SpriteFont Font14;
        public static SpriteFont Font16;

        public Deft()
        {
            if (_instance == null)
                _instance = this;
            else
                throw new System.Exception("Cannot have more than one instance of Deft");

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;

            // Restrict mouse to stay within the window.
            Input.SetMaxMouseX(graphics.PreferredBackBufferWidth);
            Input.SetMaxMouseY(graphics.PreferredBackBufferHeight);
        }

        protected override void Initialize()
        {
            base.Initialize();

            Mouse.WindowHandle = Window.Handle;

            SceneManager.Init();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);


            Font6 = Content.Load<SpriteFont>("font_6");
            Font8 = Content.Load<SpriteFont>("font_8");
            Font10 = Content.Load<SpriteFont>("font_10");
            Font12 = Content.Load<SpriteFont>("font_12");
            Font14 = Content.Load<SpriteFont>("font_14");
            Font16 = Content.Load<SpriteFont>("font_16");

            Assets.content = Content;
            Assets.LoadAssets();
            UserSetup.Init();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Input.UpdateStates();

            SceneManager.HandleInput();
            SceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();

            {
                SceneManager.Render(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
