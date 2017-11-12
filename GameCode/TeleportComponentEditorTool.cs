using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;


namespace DeftLib
{
    public class TeleportComponentEditorTool : ComponentEditorTool
    {
        private TeleportComponent _editing;

        public override void Edit(Entity e)
        {
            _editing = e.GetComponent<TeleportComponent>();

            if (Input.RightMouseDown())
                _editing.position = Input.MousePos;
        }

        public override void RenderGUI(SpriteBatch spriteBatch)
        {
            if (_editing != null)
                spriteBatch.DrawPoint(_editing.position, TOOL_COLOR, 10);
        }
    }
}
