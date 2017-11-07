using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace DeftLib
{
    public class TextureRenderingSystem : EntitySystem, IRenderingSystem
    {
        private SpriteBatch _spriteBatch;

        public SpriteBatch SpriteBatch { get => _spriteBatch; }


        public TextureRenderingSystem() :
            base(typeof(TextureRendererComponent), typeof(SpatialComponent))
        {
            _spriteBatch = Deft.Get.spriteBatch;
        }

        public override void Process()
        {
            foreach (var entity in _entities)
                entity.GetComponent<TextureRendererComponent>().Render(_spriteBatch);
        }
    }
}
