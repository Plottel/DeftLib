using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace DeftLib
{
    public class PrototypeConstructionPanel : Panel
    {
        public const int DEFAULT_WIDTH = 1000;
        public const int DEFAULT_HEIGHT = 715;

        private EntityPanel _entityPanel;
        public EntityPanel EntityPanel { get => _entityPanel; }

        private Entity _currentPrototype;
        private Vector2 _prototypePos;

        // Default constructor for reflection instantiation
        public PrototypeConstructionPanel() : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), 1)
        { }

        // Layer constructor for reflection instantiation
        public PrototypeConstructionPanel(int layer) : 
            this("", Vector2.Zero, new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT), layer)
        { }

        public PrototypeConstructionPanel(string label, Vector2 pos, Vector2 size, int layer) : base(label, pos, size, layer)
        {
            AddGadget<StringBox>("Open");
            AddGadget<StringBox>("Name");
            AddGadget<Button>("New");
            AddGadget<Button>("Save");
            AddGadget<Button>("Delete");
            AddGadget<EntityPanel>("Current Prototype");
            _entityPanel = GetGadget<EntityPanel>("Current Prototype");
            _prototypePos = new Vector2(700, 300);

            ResetPrototype();
            _entityPanel.SetEntity(_currentPrototype);
        }

        public override void OnGUIEvent()
        {
            base.OnGUIEvent();

            if (GetGadget<Button>("Save").IsClicked)
            {
                var nameSB = GetGadget<StringBox>("Name");

                Prototypes.AddPrototype(nameSB.Value, _currentPrototype);
                ResetPrototype();
                nameSB.SyncGadget("");
            }

            if (GetGadget<Button>("New").IsClicked)
                ResetPrototype();

            if (Input.KeyTyped(Keys.Enter))
            {
                var openSB = GetGadget<StringBox>("Open");

                if (ActiveGadget == openSB)
                {
                    if (Prototypes.AllPrototypeNames.Contains(openSB.Value))
                    {
                        _currentPrototype = Prototypes.Create(openSB.Value, _prototypePos);
                        _entityPanel.SetEntity(_currentPrototype);
                        openSB.SyncGadget("");
                        GetGadget<StringBox>("Name").SyncGadget(openSB.Value);
                    }
                }
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);

            // TODO: When program is larger, "Entity.Render()" is a consideration -> can't do it without the ECS.
            if (_currentPrototype != null)
                spriteBatch.FillRectangle(_currentPrototype.GetComponent<SpatialComponent>().Bounds, Color.Blue);

            var prototypeBorderRect = new Rectangle((this.pos + _prototypePos - new Vector2(150, 150)).ToPoint(), new Point(350, 350));

            spriteBatch.DrawRectangle(prototypeBorderRect, Color.Blue, 5);
            spriteBatch.DrawRectangle(prototypeBorderRect.GetInflated(5, 5), Color.Blue, 3);
            spriteBatch.DrawRectangle(prototypeBorderRect.GetInflated(10, 10), Color.Blue, 2);

            if (ActiveGadget == GetGadget<StringBox>("Open"))
                Panel.RenderSideText(spriteBatch, Prototypes.AllPrototypeNames, this);
        }

        public void ResetPrototype()
        {
            _currentPrototype = new Entity();
            var spatial = new SpatialComponent() { pos = this.pos + _prototypePos, size = new Vector2(50, 50) };
            _currentPrototype.AddComponent(spatial);
            _entityPanel.SetEntity(_currentPrototype);
        }
    }
}
