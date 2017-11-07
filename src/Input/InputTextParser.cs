using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace DeftLib
{
    public static class InputTextParser
    {
        public static string ParseKeys(Keys[] keys)
        {
            string result = "";

            foreach (var key in keys)
            {
                switch (key)
                {
                    case Keys.D0: result += "0"; break;
                    case Keys.D1: result += "1"; break;
                    case Keys.D2: result += "2"; break;
                    case Keys.D3: result += "3"; break;
                    case Keys.D4: result += "4"; break;
                    case Keys.D5: result += "5"; break;
                    case Keys.D6: result += "6"; break;
                    case Keys.D7: result += "7"; break;
                    case Keys.D8: result += "8"; break;
                    case Keys.D9: result += "9"; break;
                    case Keys.Space: result += " "; break;
                    case Keys.OemPeriod: result += "."; break;
                    case Keys.Back: break;
                    case Keys.Tab: break;
                    case Keys.Insert: break;
                    case Keys.Enter: break;
                    case Keys.Delete: break;

                    default: result += key.ToString(); break;
                }
            }

            return result;
        }
    }
}
