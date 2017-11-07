using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class PrototypeInstantiationPanel : Panel
    {
        private string _selectedPrototypeName = "";

        public string SelectedPrototypeName
        {
            get { return _selectedPrototypeName; }
        }

        // Default constructor for reflection instantiation
        public PrototypeInstantiationPanel() : 
            this("", Vector2.Zero, Vector2.Zero, 1)
        { }

        // Layer constructor for reflection instnatiation
        public PrototypeInstantiationPanel(int layer) : 
            this("", Vector2.Zero, Vector2.Zero, layer)
        { }

        public PrototypeInstantiationPanel(string label, Vector2 pos, Vector2 size, int layer) : 
            base(label, pos, size, layer)
        {
            foreach (string name in Prototypes.AllPrototypeNames)
                AddGadget<Button>(name);
        }

        public override void OnGUIEvent()
        {
            base.OnGUIEvent();

            foreach (string name in Prototypes.AllPrototypeNames)
            {
                if (GetGadget<Button>(name).IsClicked)
                {
                    _selectedPrototypeName = name;
                    break;
                }                
            }
        }
    }
}
