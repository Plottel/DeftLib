using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public class RectComponentPanel : Panel
    {
        public RectComponent editing;

        public RectComponentPanel(string label, Vector2 pos) : base(label, pos, new Vector2(300, 400))
        {
            editing = new RectComponent();

            AddGadget<IntBox>("Rect X");
            AddGadget<IntBox>("Rect Y");
            AddGadget<IntBox>("Rect W");
            AddGadget<IntBox>("Rect H");
            AddGadget<ColorPanel>("Rect Color");
        }

        public override void OnGUIEvent()
        {
            base.OnGUIEvent();

            editing.rect.X = GetGadget<IntBox>("Rect X").Value;
            editing.rect.Y = GetGadget<IntBox>("Rect Y").Value;
            editing.rect.Width = GetGadget<IntBox>("Rect W").Value;
            editing.rect.Height = GetGadget<IntBox>("Rect H").Value;
            editing.color = GetGadget<ColorPanel>("Rect Color").Value;
        }
    }
}
