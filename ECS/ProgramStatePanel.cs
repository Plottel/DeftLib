using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public class ProgramStatePanel : Panel
    {
        public ProgramStatePanel() : 
            this("", Vector2.Zero, Vector2.Zero, 1)
        { }

        // Layer constructor for reflection instnatiation
        public ProgramStatePanel(int layer) : 
            this("", Vector2.Zero, Vector2.Zero, layer)
        { }

        public ProgramStatePanel(string label, Vector2 pos, Vector2 size) :
            this(label, pos, size, 1)
        { }

        public ProgramStatePanel(string label, Vector2 pos, Vector2 size, int layer) : 
            base(label, pos, size, layer)
        {
            AddGadget<Button>("Play Scene");
            AddGadget<Button>("Stop Scene");
            AddGadget<Button>("Save Scene");
            AddGadget<Button>("Load Scene");
            AddGadget<Button>("Tile Map Editor");
            AddGadget<Button>("World Editor");
        }

        public override void OnGUIEvent()
        {
            base.OnGUIEvent();

            if (GetGadget<Button>("Play Scene").IsClicked)
                World.PushState(new InGamePlayGameState());
            else if (GetGadget<Button>("Stop Scene").IsClicked)
                World.PushState(new InEditorGameState());
            else if (GetGadget<Button>("Save Scene").IsClicked)
                World.SaveWorld();
            else if (GetGadget<Button>("Load Scene").IsClicked)
                World.LoadWorld();
            else if (GetGadget<Button>("Tile Map Editor").IsClicked)
                World.PushState(new InTileMapEditorGameState());
            else if (GetGadget<Button>("World Editor").IsClicked)
                World.PushState(new InWorldEditorGameState());
        }
    }
}
