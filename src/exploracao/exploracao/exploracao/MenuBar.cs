using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Xml;

namespace exploracao
{
    class SpriteInfo
    {
        public Microsoft.Xna.Framework.Rectangle area { get; set; }
        public SpriteInfo(int x1, int y1, int x2, int y2)
        {
            area = new Microsoft.Xna.Framework.Rectangle(x1, y1, x2 - x1, y2 - y1);
        }
    }

    /**
     * For standands purposes
     */
    public abstract class GObject
    {
        internal SpriteInfo[] spritesInfo { get; set; }
        internal int spriteStatus { get; set; }

        // This is a texture we can render.
        internal Texture2D texture { get; set; }

        // Set the coordinates to draw the sprite at.
        internal Vector2 spritePosition = Vector2.Zero;
        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Vector2 origin = new Vector2(spritesInfo[spriteStatus].area.Width / 2,
                                         spritesInfo[spriteStatus].area.Height / 2);

            spriteBatch.Draw(texture, spritePosition, spritesInfo[spriteStatus].area, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            spriteBatch.End();
        }


    }
    class MenuBar : GObject
    {

        // Global variables
        enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }

        public const int NUMBER_OF_BUTTONS = 2,
            START_BUTTON_INDEX = 0,
            QUIT_BUTTON_INDEX = 1;

        private const int BUTTON_HEIGHT = 40,
                  BUTTON_WIDTH = 88;
        //private Color background_color;
        private Color[] button_color = new Color[NUMBER_OF_BUTTONS];
        private Rectangle[] button_rectangle = new Rectangle[NUMBER_OF_BUTTONS];
        private BState[] button_state = new BState[NUMBER_OF_BUTTONS];
        private Texture2D[] button_texture = new Texture2D[NUMBER_OF_BUTTONS];
        private double[] button_timer = new double[NUMBER_OF_BUTTONS];
        //mouse pressed and mouse just pressed
        private bool mpressed, prev_mpressed = false;
        //mouse location in window
        private int mx, my;
        private double frame_time;
        Texture2D background;

        private int width;
        private int height;

        public MenuBar(Texture2D[] button_texture, Texture2D background, int x, int y)
        {
            this.width = x;
            this.height = y;
            this.background = background;

            Option = -1;
            this.button_texture = button_texture;
            // starting x and y locations to stack buttons
            // vertically in the middle of the screen
            x =  x / 2 - BUTTON_WIDTH / 2;
            y =  y / 2 - NUMBER_OF_BUTTONS / 2 * BUTTON_HEIGHT -
                (NUMBER_OF_BUTTONS % 2) * BUTTON_HEIGHT / 2;

            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                button_state[i] = BState.UP;
                button_color[i] = Color.White;
                button_timer[i] = 0.0;
                button_rectangle[i] = new Rectangle(x, y, BUTTON_WIDTH, BUTTON_HEIGHT);
                y += BUTTON_HEIGHT;
            }
            //IsMouseVisible = true;
            //background_color = Color.CornflowerBlue;
        }

        public void update_buttons(GameTime gameTime)
        {
            // get elapsed frame time in seconds
            frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

            // update mouse variables
            MouseState mouse_state = Mouse.GetState();
            mx = mouse_state.X;
            my = mouse_state.Y;
            prev_mpressed = mpressed;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;
            update_buttons();
        }

        // wrapper for hit_image_alpha taking Rectangle and Texture
        private Boolean hit_image_alpha(Rectangle rect, Texture2D tex, int x, int y)
        {
            return hit_image_alpha(0, 0, tex, tex.Width * (x - rect.X) /
                rect.Width, tex.Height * (y - rect.Y) / rect.Height);
        }

        // wraps hit_image then determines if hit a transparent part of image
        private Boolean hit_image_alpha(float tx, float ty, Texture2D tex, int x, int y)
        {
            if (hit_image(tx, ty, tex, x, y))
            {
                uint[] data = new uint[tex.Width * tex.Height];
                tex.GetData<uint>(data);
                if ((x - (int)tx) + (y - (int)ty) *
                    tex.Width < tex.Width * tex.Height)
                {
                    return ((data[
                        (x - (int)tx) + (y - (int)ty) * tex.Width
                        ] &
                                0xFF000000) >> 24) > 20;
                }
            }
            return false;
        }

        // determine if x,y is within rectangle formed by texture located at tx,ty
        private Boolean hit_image(float tx, float ty, Texture2D tex, int x, int y)
        {
            return (x >= tx &&
                x <= tx + tex.Width &&
                y >= ty &&
                y <= ty + tex.Height);
        }

        // determine state and color of button
        private void update_buttons()
        {
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {

                if (hit_image_alpha(
                    button_rectangle[i], button_texture[i], mx, my))
                {
                    button_timer[i] = 0.0;
                    if (mpressed)
                    {
                        // mouse is currently down
                        button_state[i] = BState.DOWN;
                        button_color[i] = Color.Blue;
                    }
                    else if (!mpressed && prev_mpressed)
                    {
                        // mouse was just released
                        if (button_state[i] == BState.DOWN)
                        {
                            // button i was just down
                            button_state[i] = BState.JUST_RELEASED;
                        }
                    }
                    else
                    {
                        button_state[i] = BState.HOVER;
                        button_color[i] = Color.LightBlue;
                    }
                }
                else
                {
                    button_state[i] = BState.UP;
                    if (button_timer[i] > 0)
                    {
                        button_timer[i] = button_timer[i] - frame_time;
                    }
                    else
                    {
                        button_color[i] = Color.White;
                    }
                }

                if (button_state[i] == BState.JUST_RELEASED)
                {
                    take_action_on_button(i);
                }
            }
        }

        // Logic for each button click goes here
        private void take_action_on_button(int i)
        {
            sndMenu.Play();
            //take action corresponding to which button was clicked
            switch (i)
            {
                case START_BUTTON_INDEX:
                    //background_color = Color.Green;
                    break;
                case QUIT_BUTTON_INDEX:
                    //background_color = Color.Yellow;
                    break;
                default:
                    break;
            }
            Option = i;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle (0, 0, width, height), Color.White);
            
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
                spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);
            spriteBatch.End();
        }

        public int Option { get; set; }

        public Microsoft.Xna.Framework.Audio.SoundEffect sndMenu { get; set; }
    }
}
