using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            AddGadget<StringBox>("Scene Name");
            AddGadget<Button>("Open Scene");
            AddGadget<Button>("New Scene");
            AddGadget<Button>("Delete Scene");

            AddGadget<Button>("Play Scene");
            AddGadget<Button>("Stop Scene");
            AddGadget<Button>("Save All Scenes");

            AddGadget<Button>("Entity Prototype Editor");
            AddGadget<Button>("Tile Map Editor");
            AddGadget<Button>("Entity Editor");
            AddGadget<Button>("World Editor");
        }

        public override void OnGUIEvent()
        {
            base.OnGUIEvent();

            if (GetGadget<Button>("Delete Scene").IsClicked)
                SceneManager.DeleteCurrentScene();
            if (GetGadget<Button>("Play Scene").IsClicked)
                SceneManager.PushState(new InGamePlayGameState());
            else if (GetGadget<Button>("Stop Scene").IsClicked)
                SceneManager.PushState(new InEditorMenuGameState());
            else if (GetGadget<Button>("Save All Scenes").IsClicked)
            {
                SceneManager.Scene.Name = GetGadget<StringBox>("Scene Name").Value;
                SceneManager.SaveWorld();
            }
            else if (GetGadget<Button>("Open Scene").IsClicked)
                SceneManager.ChangeScene(GetGadget<StringBox>("Scene Name").Value);
            else if (GetGadget<Button>("New Scene").IsClicked)
            {
                var sceneName = GetGadget<StringBox>("Scene Name").Value;
                SceneManager.AddScene(new Scene(sceneName));
                SceneManager.ChangeScene(sceneName);
            }
            else if (GetGadget<Button>("Tile Map Editor").IsClicked)
                SceneManager.PushState(new InTileMapEditorGameState());
            else if (GetGadget<Button>("World Editor").IsClicked)
                SceneManager.PushState(new InWorldEditorGameState());
            else if (GetGadget<Button>("Entity Editor").IsClicked)
                SceneManager.PushState(new InEntityEditorGameState());
            else if (GetGadget<Button>("Entity Prototype Editor").IsClicked)
                SceneManager.PushState(new InPrototypeEditorGameState());
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);

            if (ActiveGadget == GetGadget<StringBox>("Scene Name"))
                Panel.RenderSideText(spriteBatch, SceneManager.AllSceneNames, this, SideTextPos.Left);
        }
    }
}
