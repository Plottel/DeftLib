using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DeftLib
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Deft : Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        private static Deft _instance;

        public static Deft Get
        {
            get { return _instance; }
        }

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
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Font10 = Content.Load<SpriteFont>("font_10");
            Font12 = Content.Load<SpriteFont>("font_12");
            Font14 = Content.Load<SpriteFont>("font_14");
            Font16 = Content.Load<SpriteFont>("font_16");
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

            World.HandleInput();
            World.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();

            {
                World.Render(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
