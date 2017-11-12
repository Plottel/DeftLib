using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DeftLib
{
    public class Player : Entity
    {
        public override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (Input.KeyTyped(Keys.Space))
                SceneManager.ChangeScene("SCENETWO");
        
        }
    }
}
