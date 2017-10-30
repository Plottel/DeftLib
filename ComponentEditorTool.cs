using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DeftLib
{
    public abstract class ComponentEditorTool
    {
        public static Color TOOL_COLOR
        {
            get { return Color.LawnGreen; }
        }

        public abstract void Edit(Entity e);
        public abstract void RenderGUI(SpriteBatch spriteBatch);
    }
}
