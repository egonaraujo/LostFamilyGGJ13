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
    class People
    {
        public Animation personAnimation;

        public Animation personAnimation1;
        public Animation personAnimation2;
        public Animation personAnimation3;

        public Vector2 lastPosition;
        public Vector2 personPosition;
        public Vector2 respawn;

        public int height
        {
            get { return personAnimation.FrameHeight; }
        }
        public int width
        {
            get { return personAnimation.FrameWidth; }
        }

        public static float personMoveSpeed=2;
        
        public bool walkingRight = false;
        public bool walkingLeft= false;
        public bool walkingUp= false;
        public bool walkingDown= false;
        public bool walking = false;
        public bool ativo, flip;

        public Rectangle collision;
        public int aditionalXCollision;
        public int aditionalYCollision;

        public bool isFollowing = false;

        public void Initialize(Animation animation1, Animation animation2, Animation animation3, int x, int y, int recXAditional, int recYAditional, int recHeight, int recWidth)
        {
            personAnimation = animation1;
            personAnimation1 = animation1;
            personAnimation2 = animation2;
            personAnimation3 = animation3;

            Vector2 p = new Vector2();
            p.X = x * Level.GRADE;
            p.Y = y * Level.GRADE;
            personPosition = p;
            respawn = p;

            lastPosition = personPosition;

            aditionalXCollision = recXAditional;
            aditionalYCollision = recYAditional;

            collision = new Rectangle((int)personPosition.X + aditionalXCollision, (int)personPosition.Y + aditionalYCollision, recHeight,recWidth);
        }

        public Boolean canLeft()
        {
            return getGradeX() > 0 && !Level.level.bloqueio(getGradeX() - 1, getGradeY())
            && !(Level.level.ehRocha(getGradeX() - 1, getGradeY()) && getGradeX() == 1)
            && !(Level.level.ehRocha(getGradeX() - 1, getGradeY()) && Level.level.bloqueio(getGradeX() - 2, getGradeY())) 
            && !(Level.level.ehRocha(getGradeX()-1, getGradeY()) && Level.level.ehRocha(getGradeX()-2, getGradeY()));
        }
        public Boolean canRight()
        {
            return getGradeX() < Level.COLUNAS-1 && !Level.level.bloqueio(getGradeX() + 1, getGradeY())
            && !(Level.level.ehRocha(getGradeX() + 1, getGradeY()) && getGradeX() == Level.COLUNAS - 2)
            && !(Level.level.ehRocha(getGradeX() + 1, getGradeY()) &&  Level.level.bloqueio(getGradeX() + 2, getGradeY()))
            && !(Level.level.ehRocha(getGradeX() + 1, getGradeY()) && Level.level.ehRocha(getGradeX() + 2, getGradeY()));
        }
        public Boolean canUp()
        {
            return getGradeY() > 0 && !Level.level.bloqueio(getGradeX(), getGradeY() - 1)
            && !(Level.level.ehRocha(getGradeX(), getGradeY() - 1) && getGradeY() == 1)
            && !(Level.level.ehRocha(getGradeX(), getGradeY() - 1) && Level.level.bloqueio(getGradeX(), getGradeY() - 2))
            && !(Level.level.ehRocha(getGradeX(), getGradeY() - 1) && Level.level.ehRocha(getGradeX(), getGradeY() - 2));
        }
        public Boolean canDown()
        {
            return getGradeY() < Level.LINHAS-1 && !Level.level.bloqueio(getGradeX(), getGradeY() + 1)
            && !(Level.level.ehRocha(getGradeX(), getGradeY() + 1) && getGradeY() == Level.LINHAS - 2)
            && !(Level.level.ehRocha(getGradeX(), getGradeY()+1) &&  Level.level.bloqueio(getGradeX(), getGradeY()+2))
            && !(Level.level.ehRocha(getGradeX(), getGradeY()+1) && Level.level.ehRocha(getGradeX(), getGradeY()+2));
        }

        public void Update(GameTime gameTime)
        {
            if (!ativo)
                return;
            if (walking)
            {
                personAnimation.Looping = true;
            }
            else
            {
                personAnimation.Looping = false;
            }

            if (walkingLeft)
            {
                flip = true;
                personAnimation = personAnimation3;
                if (personPosition.X > lastPosition.X - Level.GRADE)
                {
                    personPosition.X -= (int) personMoveSpeed;
                }
                else
                {
                    lastPosition = personPosition;
                    walking = false;
                    walkingLeft = false;
                }
            }
            else if (walkingRight)
            {
                flip = false;
                personAnimation = personAnimation3;
                if (personPosition.X < lastPosition.X + Level.GRADE)
                {
                    personPosition.X += (int)personMoveSpeed;
                }
                else
                {
                    
                    lastPosition = personPosition;
                    walking = false;
                    walkingRight = false;
                }
            }

            else if (walkingUp)
            {
                flip = false;
                personAnimation = personAnimation1;
                if (personPosition.Y > lastPosition.Y - Level.GRADE)
                {
                    personPosition.Y -= (int)personMoveSpeed;
                }
                else
                {
                    lastPosition = personPosition;
                    walking = false;
                    walkingUp = false;
                }
            }
            else if (walkingDown)
            {
                flip = false;
                personAnimation = personAnimation2;
                if (personPosition.Y < lastPosition.Y + Level.GRADE)
                {
                    personPosition.Y += (int)personMoveSpeed;
                }
                else
                {
                    lastPosition = personPosition;
                    walking = false;
                    walkingDown = false;
                }
            }

            collision.X = (int)personPosition.X + aditionalXCollision;
            collision.Y = (int)personPosition.Y + aditionalYCollision;

            personAnimation.Position = personPosition;

            personAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!ativo)
                return;

            personAnimation.Draw(spriteBatch, flip);
        }

        public int getGradeX()
        {
            return (int)(personPosition.X + Level.GRADE/2) / Level.GRADE;
        }
        public int getGradeY()
        {
            return (int)(personPosition.Y + Level.GRADE / 2) / Level.GRADE;
        }

        public float globalX()
        {
            return personPosition.X + (Level.GRADE/2) + Level.levelX * Level.COLUNAS * Level.GRADE;
        }
        public float globalY()
        {
            return personPosition.Y + (Level.GRADE / 2) + Level.levelY * Level.LINHAS * Level.GRADE;
        }
        public float globalX(int x)
        {
            return personPosition.X + (Level.GRADE / 2) + x* Level.COLUNAS * Level.GRADE;
        }
        public float globalY(int y)
        {
            return personPosition.Y + (Level.GRADE / 2) + y * Level.LINHAS * Level.GRADE;
        }

        public void ativar()
        {
            Level.level.ativar(getGradeX(), getGradeY());
        }

    }

    class Peoples
    {
        static public People player;
        public Animation playerAnimation;
        public Animation playerAnimation1;
        public Animation playerAnimation2;

        static public People person1;
        public Animation person1Animation;
        public Animation person1Animation1;
        public Animation person1Animation2;

        static public People person2;
        public Animation person2Animation;
        public Animation person2Animation1;
        public Animation person2Animation2;

        static public People person3;
        public Animation person3Animation;
        public Animation person3Animation1;
        public Animation person3Animation2;

        static public People person4;
        public Animation person4Animation;
        public Animation person4Animation1;
        public Animation person4Animation2;


        public static List<People> followers;

        public void load(ContentManager content)
        {
            Texture2D playerTexture = content.Load<Texture2D>(@"sprites\me1");
            Texture2D playerTexture1 = content.Load<Texture2D>(@"sprites\me3");
            Texture2D playerTexture2 = content.Load<Texture2D>(@"sprites\me5");

            playerAnimation.Initialize(playerTexture, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);
            playerAnimation1.Initialize(playerTexture1, Vector2.Zero, Level.GRADE, Level.GRADE, 1 , 100, Color.White, 1f, false);
            playerAnimation2.Initialize(playerTexture2, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);

            playerTexture = content.Load<Texture2D>(@"sprites\i1");
            playerTexture1 = content.Load<Texture2D>(@"sprites\i1");
            playerTexture2 = content.Load<Texture2D>(@"sprites\i1");

            person1Animation.Initialize(playerTexture, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);
            person1Animation1.Initialize(playerTexture1, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);
            person1Animation2.Initialize(playerTexture2, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);

            playerTexture = content.Load<Texture2D>(@"sprites\m6");
            playerTexture1 = content.Load<Texture2D>(@"sprites\m1");
            playerTexture2 = content.Load<Texture2D>(@"sprites\m3");
            
            person2Animation.Initialize(playerTexture, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);
            person2Animation1.Initialize(playerTexture1, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);
            person2Animation2.Initialize(playerTexture2, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);

            playerTexture = content.Load<Texture2D>(@"sprites\v6");
            playerTexture1 = content.Load<Texture2D>(@"sprites\v1");
            playerTexture2 = content.Load<Texture2D>(@"sprites\v3");

            person3Animation.Initialize(playerTexture, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);
            person3Animation1.Initialize(playerTexture1, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);
            person3Animation2.Initialize(playerTexture2, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);

            playerTexture = content.Load<Texture2D>(@"sprites\pa3");
            playerTexture1 = content.Load<Texture2D>(@"sprites\pa1");
            playerTexture2 = content.Load<Texture2D>(@"sprites\pa4");

            person4Animation.Initialize(playerTexture, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);
            person4Animation1.Initialize(playerTexture1, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);
            person4Animation2.Initialize(playerTexture2, Vector2.Zero, Level.GRADE, Level.GRADE, 1, 100, Color.White, 1f, false);
        }

        public void Initialize()
        {
            player = new People();
            playerAnimation = new Animation();
            playerAnimation1 = new Animation();
            playerAnimation2 = new Animation();

            person1Animation = new Animation();
            person1Animation1 = new Animation();
            person1Animation2 = new Animation();

            person2Animation = new Animation();
            person2Animation1 = new Animation();
            person2Animation2 = new Animation();

            person3Animation = new Animation();
            person3Animation1 = new Animation();
            person3Animation2 = new Animation();

            person4Animation = new Animation();
            person4Animation1 = new Animation();
            person4Animation2 = new Animation();

            player.Initialize(playerAnimation, playerAnimation1, playerAnimation2, 2, 5, -1, -1, Level.GRADE + 2, Level.GRADE + 2);
            player.ativo = true;

            person1 = new People();
            person1.Initialize(person1Animation, person1Animation1, person1Animation2, 6, 5, -1, -1, Level.GRADE + 2, Level.GRADE + 2);

            person2 = new People();
            person2.Initialize(person2Animation, person2Animation1, person2Animation2, 7, 4, -1, -1, Level.GRADE + 2, Level.GRADE + 2);

            person3 = new People();
            person3.Initialize(person3Animation, person3Animation1, person3Animation2, 11, 2, -1, -1, Level.GRADE + 2, Level.GRADE + 2);

            person4 = new People();
            person4.Initialize(person4Animation, person4Animation1, person4Animation2, 10, 7, -1, -1, Level.GRADE + 2, Level.GRADE + 2);

            followers = new List<People>();
            followers.Add(player);
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState)
        {
            UpdatePlayer(gameTime, currentKeyboardState);

            player.Update(gameTime);
            person1.Update(gameTime);
            person2.Update(gameTime);
            person3.Update(gameTime);
            person4.Update(gameTime);
        }

        public void UpdatePlayer(GameTime gameTime, KeyboardState currentKeyboardState)
        {
            if (player.getGradeX() == 0 && (currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.Left)))
                Level.level.toLeft();
            if (player.getGradeX() == Level.COLUNAS - 1 && (currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.Right)))
                Level.level.toRight();
            if (player.getGradeY() == 0 && (currentKeyboardState.IsKeyDown(Keys.W) || currentKeyboardState.IsKeyDown(Keys.Up)))
                Level.level.toUp();
            if (player.getGradeY() == Level.LINHAS - 1 && (currentKeyboardState.IsKeyDown(Keys.S) || currentKeyboardState.IsKeyDown(Keys.Down)))
                Level.level.toDown();

            if (currentKeyboardState.IsKeyDown(Keys.Enter))
            {
                player.ativar();
            }
            
            if (!player.walking)
            {
                if ((currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.Left)) && player.canLeft())
                {
                    player.walking = true;
                    player.walkingLeft = true;
                    UpdateFollowers(gameTime);

                }
                else if ((currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.Right)) && player.canRight())
                {
                    player.walking = true;
                    player.walkingRight = true;
                    UpdateFollowers(gameTime);
                    
                }
                else if ((currentKeyboardState.IsKeyDown(Keys.W) || currentKeyboardState.IsKeyDown(Keys.Up)) && player.canUp())
                {
                    player.walking = true;
                    player.walkingUp = true;
                    UpdateFollowers(gameTime);

                }
                else if ((currentKeyboardState.IsKeyDown(Keys.S) || currentKeyboardState.IsKeyDown(Keys.Down)) && player.canDown())
                {
                    player.walking = true;
                    player.walkingDown = true;
                    UpdateFollowers(gameTime);

                }
            }
            
            if (player.collision.Intersects(person1.collision) && !person1.isFollowing && person1.ativo
            && (player.personPosition.X == person1.personPosition.X || player.personPosition.Y == person1.personPosition.Y))
            {
                Game1.end();
                followers.Add(person1);
                person1.isFollowing = true;
            }

            if (player.collision.Intersects(person2.collision) && !person2.isFollowing && person2.ativo
            && (player.personPosition.X == person2.personPosition.X || player.personPosition.Y == person2.personPosition.Y))
            {
                followers.Add(person2);
                person2.isFollowing = true;
                Audio.soundBank.PlayCue("Musica1");
            }

            if (player.collision.Intersects(person3.collision) && !person3.isFollowing && person3.ativo
            && (player.personPosition.X == person3.personPosition.X || player.personPosition.Y == person3.personPosition.Y))
            {
                followers.Add(person3);
                person3.isFollowing = true;
                Audio.soundBank.PlayCue("Musica3");
            }

            if (player.collision.Intersects(person4.collision) && !person4.isFollowing && person4.ativo
            && (player.personPosition.X == person4.personPosition.X || player.personPosition.Y == person4.personPosition.Y))
            {
                followers.Add(person4);
                person4.isFollowing = true;
                Audio.soundBank.PlayCue("Musica2");
            }
             
            /*
            People person = Level.level.getPessoa(player.getGradeX(), player.getGradeY() - 1);
            if (person!=null && !person.isFollowing && person.ativo)
            {
                followers.Add(person);
                person.isFollowing = true;
            }
            person = Level.level.getPessoa(player.getGradeX()-1, player.getGradeY());
            if (person != null && !person.isFollowing && person.ativo)
            {
                followers.Add(person);
                person.isFollowing = true;
            }
            person = Level.level.getPessoa(player.getGradeX()+1, player.getGradeY());
            if (person != null && !person.isFollowing && person.ativo)
            {
                followers.Add(person);
                person.isFollowing = true;
            }
            person = Level.level.getPessoa(player.getGradeX(), player.getGradeY()+1);
            if (person != null && !person.isFollowing && person.ativo)
            {
                followers.Add(person);
                person.isFollowing = true;
            }
             * */
        }

        public void UpdateFollowers(GameTime gameTime)
        {
            for (int i = 1; i < followers.Count; i++)
            {
                if (followers.ElementAt(i - 1).personPosition.X > followers.ElementAt(i).personPosition.X)
                {
                    followers.ElementAt(i).walkingRight = true;
                }
                if (followers.ElementAt(i - 1).personPosition.X < followers.ElementAt(i).personPosition.X)
                {
                    followers.ElementAt(i).walkingLeft = true;
                }
                if (followers.ElementAt(i - 1).personPosition.Y > followers.ElementAt(i).personPosition.Y)
                {
                    followers.ElementAt(i).walkingDown = true;
                }
                if (followers.ElementAt(i - 1).personPosition.Y < followers.ElementAt(i).personPosition.Y)
                {
                    followers.ElementAt(i).walkingUp = true;
                }
            }
        }

        public void desenhar(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            person1.Draw(spriteBatch);
            person2.Draw(spriteBatch);
            person3.Draw(spriteBatch);
            person4.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void Die()
        {
            for (int dead = 0; dead < followers.Count; dead++)
            {
                followers[dead].walking = followers[dead].walkingDown = followers[dead].walkingLeft = followers[dead].walkingRight = followers[dead].walkingUp = false;
                
                followers[dead].personPosition = player.respawn;
                followers[dead].lastPosition = player.respawn;
            }
        }

        public void Die(int x, int y)
        {
            for (int dead = 0; dead < followers.Count; dead++)
            {
                followers[dead].walking = followers[dead].walkingDown = followers[dead].walkingLeft = followers[dead].walkingRight = followers[dead].walkingUp = false;
                followers[dead].personPosition.X = x * Level.GRADE;
                followers[dead].personPosition.Y = y * Level.GRADE;

                followers[dead].lastPosition.X = x * Level.GRADE;
                followers[dead].lastPosition.Y = y * Level.GRADE;
            }
        }
    }
}
