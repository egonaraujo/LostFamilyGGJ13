using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace exploracao 
{
    class RadarCoracao : BaseClass
    {
        public static float heartBeat;
        Texture2D[] textura;
        int indice = 0;

        public RadarCoracao(SpriteBatch spriteBatch, Vector2 vector2)
            : base(spriteBatch, vector2)
        {
            textura = new Texture2D[2];
        }

        public void SetTexture(int index, Texture2D texture)
        {
            textura[index] = texture;
        }

        new public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(textura[indice], posicao, Color.White);
            spriteBatch.End();
        }

        TimeSpan totalTimeElapsed = TimeSpan.Zero;
        public void Update(GameTime gameTime)
        {
            totalTimeElapsed += gameTime.ElapsedGameTime;
            if (totalTimeElapsed.Milliseconds > 140/heartBeat)
            {
                indice++;
                if (indice > 1) indice = 0;
                totalTimeElapsed = TimeSpan.Zero;
            }
        }
    }
}
