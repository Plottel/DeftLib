using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public abstract class GameState
    {
        public Scene Scene { get => SceneManager.Scene; }

        public abstract void Enter();
        public abstract void Exit();

        public abstract void HandleInput();
        public abstract void Update(GameTime gameTime);
        public abstract void Render(SpriteBatch spriteBatch);
        public abstract void RenderGUI(SpriteBatch spriteBatch);
    }
}
