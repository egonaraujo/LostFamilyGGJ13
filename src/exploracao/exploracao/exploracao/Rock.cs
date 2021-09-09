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
    class Rock
    {

        static Texture2D texture;
        Vector2 rockPosition;
        int Height
        {
            get { return texture.Height; }
        }
        int Width
        {
            get { return texture.Width; }
        }

        public float rockMoveSpeed = 2;

        public bool movingRight = false;
        public bool movingLeft = false;
        public bool movingUp = false;
        public bool movingDown = false;
        public Vector2 lastPosition;
        public Rectangle collision;

        public void Initialize(int x, int y)
        {
            Vector2 p = new Vector2();
            p.X = x * Level.GRADE;
            p.Y = y * Level.GRADE;
            rockPosition = p;
            lastPosition = p;
            collision = new Rectangle((int)p.X + 2, (int)p.Y + 2, Level.GRADE - 4, Level.GRADE - 4);
        }

        static public void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\p1");
        }

        public void Update()
        {
            if (movingLeft)
            {
                if (rockPosition.X > lastPosition.X - Level.GRADE && rockPosition.X != 0)
                {
                    rockPosition.X -= (int)rockMoveSpeed;
                }
                else
                {
                    lastPosition = rockPosition;
                    movingLeft = false;
                }
            }
            else if (movingRight)
            {
                if (rockPosition.X < lastPosition.X + Level.GRADE)
                {
                    rockPosition.X += (int)rockMoveSpeed;
                }
                else
                {
                    lastPosition = rockPosition;
                    movingRight = false;
                }
            }

            else if (movingUp)
            {
                if (rockPosition.Y > lastPosition.Y - Level.GRADE && rockPosition.Y != 0)
                {
                    rockPosition.Y -= (int)rockMoveSpeed;
                }
                else
                {
                    lastPosition = rockPosition;
                    movingUp = false;
                }
            }
            else if (movingDown)
            {
                if (rockPosition.Y < lastPosition.Y + Level.GRADE)
                {
                    rockPosition.Y += (int)rockMoveSpeed;
                }
                else
                {
                    lastPosition = rockPosition;
                    movingDown = false;
                }
            }

            collision.X = (int)rockPosition.X+2;
            collision.Y = (int)rockPosition.Y+2;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Begin();
            spriteBatch.Draw(texture, rockPosition, Color.White);
            spriteBatch.End();
        }

        public int getGradeX()
        {
            return (int)(rockPosition.X + Level.GRADE / 2) / Level.GRADE;
        }
        public int getGradeY()
        {
            return (int)(rockPosition.Y + Level.GRADE / 2) / Level.GRADE;
        }

    }

    class Rocks
    {
        public static List<Rock> rocks = new List<Rock>();

        public static void Update()
        {
            foreach (Rock rock in rocks)
            {
                rock.Update();
            }

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Rock rock in rocks)
            {
                rock.Draw(spriteBatch);
            }
        }
    }
}
