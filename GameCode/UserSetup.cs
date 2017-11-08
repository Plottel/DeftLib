using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeftLib
{
    public static class UserSetup 
    {
        public static void Init()
        {
            // Subscribe any custom Entity Systems here
            // call ECSCore.SubscribeSystem(systemObject)
            ECSCore.SubscribeSystem(new SpinSystem());
        }
    }
}
