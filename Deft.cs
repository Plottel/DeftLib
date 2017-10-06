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
        SpriteBatch spriteBatch;

        public static SpriteFont Font10;
        public static SpriteFont Font12;
        public static SpriteFont Font14;
        public static SpriteFont Font16;

        private World _world;


        public Deft()
        {
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

            _world = new World();
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

            _world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();

            {
                spriteBatch.DrawString(Font10, "Hello HOW is it going?", new Vector2(100, 100), Color.Black);
                spriteBatch.DrawString(Font12, "Hello HOW is it going?", new Vector2(100, 150), Color.Black);
                spriteBatch.DrawString(Font14, "Hello HOW is it going?", new Vector2(100, 200), Color.Black);
                spriteBatch.DrawString(Font16, "Hello HOW is it going?", new Vector2(100, 250), Color.Black);

                _world.Render(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
