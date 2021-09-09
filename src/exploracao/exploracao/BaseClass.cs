using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace exploracao
{
    class BaseClass
    {
        public Vector2 posicao;
        public Texture2D Texture;
        //public static GraphicsDevice GraphicsDevice;
        protected SpriteBatch spriteBatch;

        public BaseClass(SpriteBatch spriteBatch, Vector2 posicao)
        {
            this.spriteBatch = spriteBatch;
            // TODO: Complete member initialization
            this.spriteBatch = spriteBatch;
            this.posicao = posicao;
        }   

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, posicao, Color.White);
            spriteBatch.End();
        }
    }
}
