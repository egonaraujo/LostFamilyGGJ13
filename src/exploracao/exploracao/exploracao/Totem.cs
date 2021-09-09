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
    class Totem
    {
        public static Texture2D totemTexture, totemTexture2, totemTexture3;
        public List<Arrow> arrows = new List<Arrow>();

        public Vector2 totemPosition;
        public Rectangle totemPosition2;
        public int height
        {
            get { return totemTexture.Height; }
        }
        public int width
        {
            get { return totemTexture.Width; }
        }
        public int totemFireRate;
        public int time;

        public bool isFiring = true;

        public bool left = false;
        public bool right = false;
        public bool up = false;
        public bool down = false;

        public void Initialize(Vector2 position, float fireRate)
        {
            totemFireRate = (int)((fireRate+0.25f) * ((Level.GRADE * 2) / People.personMoveSpeed));
            time = totemFireRate+1;
            totemPosition = position;
            totemPosition2 = new Rectangle((int) position.X, (int) position.Y, Level.GRADE, Level.GRADE);
        }

        public static void LoadContent(ContentManager content)
        {
            totemTexture = content.Load<Texture2D>(@"sprites\toten1");
            totemTexture2 = content.Load<Texture2D>(@"sprites\toten2");
            totemTexture3 = content.Load<Texture2D>(@"sprites\toten3");
        }
        public Arrow Update(Texture2D arrowTexture)
        {
            if (isFiring)
            {
                Arrow arrow = new Arrow();
                arrow.Initialize(arrowTexture, new Vector2(totemPosition.X + Level.GRADE / 2, totemPosition.Y + Level.GRADE / 2));

                if (left)
                {
                    arrow.left = true;
                }
                else if (right)
                {
                    arrow.right = true;    
                }

                else if (up)
                {
                    arrow.up = true;
                }
                else if (down)
                {
                    arrow.down = true;
                }

                return arrow;
            }
            return null;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if(down)
                spriteBatch.Draw(totemTexture,totemPosition,Color.White );
            if (left)
                spriteBatch.Draw(totemTexture2, totemPosition, Color.White);
            if (right)
                spriteBatch.Draw(totemTexture2, totemPosition2, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            if (up)
                spriteBatch.Draw(totemTexture3, totemPosition, Color.White);
        }
    }

    class Totens
    {
        public Texture2D arrowTextureHere;
        public static List<Totem> totems= new List<Totem>();
        public static int arrowSize;

        Peoples peoples;

        public void Initialize(Peoples peoples)
        {

            this.peoples = peoples;

        }


        public void LoadContent(ContentManager content)
        {
            arrowTextureHere = content.Load<Texture2D>(@"sprites\flecha");
            arrowSize = Math.Max(arrowTextureHere.Width, arrowTextureHere.Height);
            Totem.LoadContent(content);
        }

        public void Update(GameTime gameTime, int w, int h)
        {
            UpdateArrow(gameTime);

            foreach (Totem totem in totems)
            {
                for (int i = 0; i < totem.arrows.Count; i++)
                {
                       totem.arrows[i].Update(w, h);
                       if (!totem.arrows.ElementAt(i).active)
                            totem.arrows.RemoveAt(i);
                }
            }
        }

        public void UpdateArrow(GameTime gameTime)
        {
            foreach (Totem totem in totems)
            {
                
                /*oduble time = (gameTime.TotalGameTime - totem.lastShot).TotalMilliseconds;
                double fireTime = totem.totemFireRate.TotalMilliseconds;
                
                if ( time >= totem.totemFireRate.TotalMilliseconds)
                {
                    totem.lastShot = gameTime.TotalGameTime;
                    Arrow a = totem.Update(arrowTextureHere);
                    if(a!=null)
                        totem.arrows.Add(a);
                }
                */
                totem.time++;
                if (totem.time >= totem.totemFireRate)
                {
                    Arrow a = totem.Update(arrowTextureHere);
                    if (a != null)
                        totem.arrows.Add(a);
                    totem.time=0;
                }
                
                /* if (totems[i].arrows.Count == 0)
                {
                    totems[i].arrows.Add(totems[i].Update(arrowTextureHere, i));
                }
                
                Arrow arrow = totems[i].arrows[totems[i].arrows.Count - 1];

                if (arrow.arrowPosition.X > totems[i].totemPosition.X + 15f + totems[i].distanceFire || arrow.arrowPosition.X < totems[i].totemPosition.X + 15f - totems[i].distanceFire ||
                arrow.arrowPosition.Y > totems[i].totemPosition.Y + 15f + totems[i].distanceFire || arrow.arrowPosition.Y < totems[i].totemPosition.Y + 15f - totems[i].distanceFire)
                {
                    totems[i].arrows.Add(totems[i].Update(arrowTextureHere, i));

                    break;

                }*/
            }
           

        }

        public static void resetar()
        {
 
        }


       public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
       {
            // Start drawing
            spriteBatch.Begin();


            for (int drawTotems = totems.Count - 1; drawTotems >= 0; drawTotems--)
            {
                totems[drawTotems].Draw(spriteBatch);
            }

            foreach (Totem totem in totems)
            {
                for (int i = 0; i < totem.arrows.Count; i++)
                {
                    totem.arrows[i].Draw(spriteBatch);
                    
                }
            }
            spriteBatch.End();
        }
    }
}
