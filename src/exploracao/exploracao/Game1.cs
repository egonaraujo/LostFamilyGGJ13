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
using Microsoft.Xna.Framework.Audio;

namespace exploracao
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Game1 game;

        public static RenderTarget2D cenario;
        public static Boolean updateCenario;
        public static float cenarioOpc;
        public static float cenarioVel=1/32f;

        public static float blurComprimetno = 1000;
        public RenderTarget2D[] blurImg=new RenderTarget2D[6];
        public int blurCount = 6;
        public float blurOpc = 0.25f;
        public float blurSize = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Levels levels;
        Peoples peoples = new Peoples();
        Totens totens = new Totens();
        Random rnd = new Random();

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        RadarCoracao radarCoracao;
        Ators ators;

        //Game Menu Options
        MenuBar menuBar;

        //Game status
        int gameStatus = STS_CHOSING_MENU;

        const int STS_PLAYING_GAME = 1;
        const int STS_CHOSING_MENU = 3;

        static Texture2D historia, fim;

        public static int historiaT = 0;
        public static int fimT = -1;

        public Game1()
        {
            game = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 896;

            Level.LINHAS = this.graphics.PreferredBackBufferHeight / Level.GRADE;
            Level.COLUNAS = this.graphics.PreferredBackBufferWidth / Level.GRADE;

            Content.RootDirectory = "Content";

            levels = new Levels();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Audio.Initialize();

            peoples.Initialize();
            Level.initialize(peoples);
            totens.Initialize(peoples);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            cenario = new RenderTarget2D(graphics.GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            for(int i=0; i<blurCount; i++)
                blurImg[i] = new RenderTarget2D(graphics.GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            ators = new Ators(spriteBatch);

            radarCoracao = new RadarCoracao(spriteBatch, new Vector2(12330, 12344));
            radarCoracao.posicao = new Vector2(0, 0);
            radarCoracao.SetTexture(0, Content.Load<Texture2D>(@"sprites\coracao1"));
            //radarCoracao.SetTexture(1, Content.Load<Texture2D>(@"sprites\coracao2"));
            radarCoracao.SetTexture(1, Content.Load<Texture2D>(@"sprites\coracao3"));

            // TODO: use this.Content to load your game content here
            Audio.Load(Content);

            Texture2D[] button_texture = new Texture2D[MenuBar.NUMBER_OF_BUTTONS];
            button_texture[MenuBar.START_BUTTON_INDEX] = Content.Load<Texture2D>(@"sprites/oi1");
            button_texture[MenuBar.QUIT_BUTTON_INDEX] = Content.Load<Texture2D>(@"sprites/exit");
            menuBar = new MenuBar(button_texture, Content.Load<Texture2D>(@"sprites\menuoi"), Window.ClientBounds.Width, Window.ClientBounds.Height);
            menuBar.sndMenu = Content.Load<SoundEffect>(@"sounds/menu");

            historia = Content.Load<Texture2D>(@"sprites/oioi");
            fim = Content.Load<Texture2D>(@"sprites/youwin");

            Levels.load(this.Content);
            Levels.initialize();

            peoples.load(this.Content);
            totens.arrowTextureHere = Content.Load<Texture2D>(@"sprites\flecha");
            totens.LoadContent(Content);

            Rock.Load(Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void readInputs()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape) &&
                previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                menuBar.Option = -1;
                if (historiaT > -1)
                {
                    historiaT = -1;
                    return;
                }

                if (gameStatus == STS_CHOSING_MENU)
                {
                    gameStatus = STS_PLAYING_GAME;
                    IsMouseVisible = false;
                }
                else
                {
                    gameStatus = STS_CHOSING_MENU;
                    IsMouseVisible = true;

                    Audio.soundNatureza.Pause();
                    Audio.soundHeart.Stop();
                }
            }
            if (keyboardState.IsKeyDown(Keys.R) &&
                previousKeyboardState.IsKeyUp(Keys.R))
                Levels.reset();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            if (historiaT > -1)
            {
                historiaT--;
                readInputs();
                return;
            }
            if (fimT > -1)
            {
                fimT--;
                if (fimT == 0)
                    this.Exit();
                return;
            }

            switch (gameStatus)
            {
                case STS_CHOSING_MENU:
                    {
                        menuBar.update_buttons(gameTime);

                        if (menuBar.Option == MenuBar.START_BUTTON_INDEX)
                        {
                            historiaT = 1000;

                            updateCenario = true;
                            gameStatus = STS_PLAYING_GAME;
                            IsMouseVisible = false;

                            // Resume the sound
                            Audio.soundNatureza.Resume();
                            Audio.soundHeart.Play();
                            // stop menu sound
                            Audio.musicas[3].Stop(AudioStopOptions.Immediate);
                        }

                        //Exit Game
                        if (menuBar.Option == MenuBar.QUIT_BUTTON_INDEX)
                        {
                            this.Exit();
                        }
                        return;
                    }
                default:
                    {
                        break;
                    }
            }


            // Allows the game to exit
           // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
             //   this.Exit();

            // TODO: Add your update logic here

            cenarioOpc -= cenarioVel;
            if (cenarioOpc == 0)
                updateCenario = true;
            if (cenarioOpc < -1)
                cenarioOpc = -1;
            blurSize =  (1000.0f/(Ators.distance));
            blurSize *= 15;
            blurSize -= 25;
           
            // Update the audio engine.
            Audio.audioEngine.Update();

            Level.level.atualizar();
            
            Rocks.Update();
            totens.Update(gameTime, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            //if (currentKeyboardState.IsKeyDown(Keys.Escape))
            //    this.Exit();

            peoples.Update(gameTime, currentKeyboardState);
            ators.update();
            Colisions.updateColisions(gameTime);

            radarCoracao.Update(gameTime);

            base.Update(gameTime);
            readInputs();
        }

        public static void end()
        {
            fimT = 500;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        void desenhar(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            Level.level.desenhar(spriteBatch, 0);
            Level.level.desenhar(spriteBatch, 1);
            totens.Draw(spriteBatch, gameTime);
            Rocks.Draw(spriteBatch);

            peoples.desenhar(spriteBatch);
        }

        static Rectangle rect = new Rectangle();
        protected override void Draw(GameTime gameTime)
        {
            if (historiaT > 0)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                rect.X = 0;
                rect.Y = 0;
                rect.Width = this.graphics.PreferredBackBufferWidth;
                rect.Height = this.graphics.PreferredBackBufferHeight;
                spriteBatch.Begin();
                spriteBatch.Draw(historia, rect, Color.White);
                spriteBatch.End();
                return;
            }

            if (fimT > 0)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                rect.X = 0;
                rect.Y = 0;
                rect.Width = this.graphics.PreferredBackBufferWidth;
                rect.Height = this.graphics.PreferredBackBufferHeight;
                spriteBatch.Begin();
                spriteBatch.Draw(fim, rect, Color.White);
                spriteBatch.End();
                return;
            }

            if (updateCenario)
            {
                GraphicsDevice.SetRenderTarget(cenario);

                Level.level.desenhar(spriteBatch, 0);
                Level.level.desenhar(spriteBatch, 1);
               
                GraphicsDevice.SetRenderTarget(null);
                updateCenario = false;
            }
            RenderTarget2D aux = blurImg[0];
            blurImg[0] = blurImg[blurCount - 1];
            blurImg[1] = aux;
            for (int i = blurCount - 1; i > 1; i--)
                blurImg[i] = blurImg[i - 1];

            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (gameStatus)
            {
                case STS_CHOSING_MENU:
                    {
                        //Menu
                        menuBar.Draw(spriteBatch);
                        IsMouseVisible = true;
                        return;
                    }

                default:
                    {
                        break;
                    }
            }

            GraphicsDevice.SetRenderTarget(blurImg[0]);
            desenhar(gameTime);

            GraphicsDevice.SetRenderTarget(null);
            desenhar(gameTime);

            if (blurSize >=1)
            {

                for (int i = 0; i < blurCount; i++)
                {

                    rect.X = (int)(-blurSize * ((i + 1) / (float)blurCount));
                    rect.Y = (int)(-blurSize * ((i + 1) / (float)blurCount));

                    rect.Width = (int)(this.graphics.PreferredBackBufferWidth + 2 * blurSize * ((i + 1) / (float)blurCount));
                    rect.Height = (int)(this.graphics.PreferredBackBufferHeight + 2 * blurSize * ((i + 1) / (float)blurCount));

                    spriteBatch.Begin();
                    spriteBatch.Draw(blurImg[i], rect, Color.White * (((i + 1) / (float)blurCount) * blurOpc));
                    spriteBatch.End();
                }
            }

            if (cenarioOpc > 0)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(cenario, Vector2.Zero, Color.White * cenarioOpc);
                spriteBatch.End();
            }

            radarCoracao.Draw(gameTime);

            base.Draw(gameTime);
        }
    }

}
