using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace exploracao
{
    class Arrow
    {
        public static Texture2D arrowTexture;

        public Vector2 arrowPosition;
        public int height
        {
            get { return arrowTexture.Height; }
        }
        public int width
        {
            get { return arrowTexture.Width; }
        }

        public int tam;

        public static float arrowMoveSpeed=2;
        public float rotation;
        public bool active;

        public bool left=false;
        public bool right = false;
        public bool up = false;
        public bool down = false;

        public Rectangle collision;

        public void Initialize(Texture2D texture, Vector2 position)
        {
            arrowTexture = texture;

            arrowPosition = position;

            tam = Math.Max(width, height);
            collision = new Rectangle((int)(position.X - tam / 2.0f), (int)(position.Y - tam / 2.0f), tam, tam);

            Audio.soundBank.PlayCue("Flash1");

            active = true;

        }
        public void Update(int screenWidth, int screenHeight)
        {
            if (active)
            {
                if (!(Level.level.getObjeto(1,getGradeX(), getGradeY()) is TotemObj) && Level.level.bloqueios[getGradeX(), getGradeY()] == true)
                {
                    Audio.soundBank.PlayCue("Flash2");

                    active = false;
                    return;
                }

                if (left)
                {
                    //1.570 it's the same as 90° clockwise
                    rotation = (float)Math.PI/2;
                    arrowPosition.X -= (int)arrowMoveSpeed;
                }
                else if (right)
                {
                    rotation = 3 * ((float)Math.PI / 2);
                    arrowPosition.X += (int)arrowMoveSpeed;
                }

                else if (up)
                {
                    rotation = (float)Math.PI;
                    arrowPosition.Y -= (int)arrowMoveSpeed;
                }
                else if (down)
                {
                    rotation = 2*(float)Math.PI;
                    arrowPosition.Y += (int)arrowMoveSpeed;
                }
            }

            if (arrowPosition.X <= 0 || arrowPosition.X >= screenWidth ||
                arrowPosition.Y <=0 || arrowPosition.Y>= screenHeight)
            {
                active = false;
            }
            collision.X = (int)(arrowPosition.X - tam / 2.0f);
            collision.Y = (int)(arrowPosition.Y - tam / 2.0f);
        }

        public int getGradeX()
        {
            return (int)(arrowPosition.X) / Level.GRADE;
        }
        public int getGradeY()
        {
            return (int)(arrowPosition.Y) / Level.GRADE;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(arrowTexture, arrowPosition, null, Color.White, rotation, new Vector2(width / 2.0f, height / 2.0f), 1.0f, SpriteEffects.None, 0f);
        }

    }
}
