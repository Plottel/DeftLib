using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public static class UserSetup 
    {
        public static void Init()
        {
            // ComponentEditorToolManager.AssociateToolType<ComponentType, ToolType>();
            ECSCore.SubscribeSystem(new SpinSystem());
        }
    }
}
