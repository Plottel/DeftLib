using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;


namespace DeftLib
{
    public abstract class ComponentEditorTool
    {
        public abstract void Edit(Entity e);
        public abstract void RenderGUI(SpriteBatch spriteBatch);
    }
}
