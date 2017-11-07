using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public class ComponentEditorToolManager
    {
        private Dictionary<Type, ComponentEditorTool> _tools =
            new Dictionary<Type, ComponentEditorTool>();

        private ComponentEditorTool _activeTool;

        public ComponentEditorToolManager()
        {
            _tools[typeof(SpatialComponent)] = new SpatialComponentEditorTool();
            _tools[typeof(PhysicsComponent)] = new PhysicsComponentEditorTool();
            _tools[typeof(CollisionComponent)] = new CollisionComponentEditorTool();
        }

        public ComponentEditorTool GetTool(Type componentType)
        { // NOTE: Read ternary like "What'cha gonna return? Has tools got the key? If yes : if no
            return _tools.ContainsKey(componentType) ?
                _tools[componentType] :
                null;
        }

        public void Edit(Entity toEdit, Type componentType)
        {
            if (toEdit == null || componentType == null)
                return;

            var tool = GetTool(componentType);

            if (tool != null)
            {
                _activeTool = tool;
                tool.Edit(toEdit);
            }
        }

        public void RenderGUI(SpriteBatch spriteBatch)
        {
            if (_activeTool != null)
                _activeTool.RenderGUI(spriteBatch);
        }
    }
}
