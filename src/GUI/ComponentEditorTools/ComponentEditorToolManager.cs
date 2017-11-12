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
        private static Dictionary<Type, ComponentEditorTool> _tools =
            new Dictionary<Type, ComponentEditorTool>();

        private ComponentEditorTool _activeTool;

        static ComponentEditorToolManager()
        {
            AssociateToolType<SpatialComponent, SpatialComponentEditorTool>();
            AssociateToolType<PhysicsComponent, PhysicsComponentEditorTool>();
            AssociateToolType<CollisionComponent, CollisionComponentEditorTool>();
        }

        public ComponentEditorToolManager()
        {
        }

        public static void AssociateToolType<ComponentType, ToolType>() 
            where ComponentType : Component 
            where ToolType : ComponentEditorTool
        {
            _tools[typeof(ComponentType)] = Activator.CreateInstance<ToolType>();
        }

        public ComponentEditorTool GetTool(Type componentType)
        {
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
