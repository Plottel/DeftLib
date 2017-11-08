using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class FloatBox : InputBox<float>
    {
        // Default constructor for reflection instantiation
        public FloatBox() :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instantiation
        public FloatBox(int layer) :
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public FloatBox(string label, Vector2 pos, Vector2 size, int layer) :
            base(label, pos, size, layer)
        { }

        public override void SyncGadget(object toAttach)
        {
            float f = (float)toAttach;

            bool applyDecimalSuffix = false;
            var decimalSuffix = "";

            //// If the chars immediately after a "." are zeros, let them concatenate
            if (Text.Contains('.'))
            {
                applyDecimalSuffix = true;
                int dotIndex = Text.IndexOf('.');
                decimalSuffix += ".";

                // If there are characters after the dot
                if (dotIndex < Text.Length - 1)
                {
                    for (int i = dotIndex + 1; i < Text.Length; ++i)
                    {
                        if (Text[i] == '0')
                        {
                            decimalSuffix += "0";
                        }
                        else // There are characters after zeros after decimal, the float will parse normally.
                        {
                            applyDecimalSuffix = false;
                            break;
                        }
                    }
                }
            }

            if (applyDecimalSuffix)
                Text = f.ToString() + decimalSuffix;
            else
                Text = f.ToString();
        }

        public override float Value
        {
            get
            {
                float result;

                if (float.TryParse(_text, out result))
                    return result;
                return 0f;
            }
        }
    }
}
