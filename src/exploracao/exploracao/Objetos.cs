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
using System.Xml;


namespace exploracao
{
    class Objeto
    {
        public int quadro;

        public Objeto(int camada, int x, int y)
        {
            Level.level.setObjeto(this, camada, x, y);
        }
        public virtual void atualizar(int x, int y)
        {
            quadro++;
        }

        static Vector2 v = new Vector2();
        public virtual void desenhar(SpriteBatch spriteBatch, int x, int y, float dx, float dy)
        {
            x *= Level.GRADE;
            y *= Level.GRADE;

            Texture2D texture = getTexture();
            v.X = x - (int)dx;
            v.Y = y - (int)dy;
            if (texture != null)
                spriteBatch.Draw(texture, v, Color.White);
        }

        public Texture2D getTexture()
        {
            return getTexture(quadro);
        }

        public virtual Texture2D getTexture(int quadro)
        {
            return null;
        }

        public virtual void onAtivar()
        {
        }

        public virtual void onPassar()
        {
        }

        public virtual Boolean ehCaminho()
        {
            return false;
        }

        public virtual Boolean ehBotao()
        {
            return false;
        }

        public void ativar()
        {
            onAtivar();
        }
    }

    class Grama : Objeto
    {
        static Texture2D texture;

        public Grama(int x, int y)
            : base(0, x, y)
        {
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\grama1");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }
    class Grama2 : Objeto
    {
        static Texture2D texture;

        public Grama2(int x, int y)
            : base(0, x, y)
        {
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\grama2");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }
    class Grama3 : Objeto
    {
        static Texture2D texture;

        public Grama3(int x, int y)
            : base(0, x, y)
        {
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\grama3");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }
    class Grama4 : Objeto
    {
        static Texture2D texture;

        public Grama4(int x, int y)
            : base(0, x, y)
        {
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\grama4");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }

    class Caminho : Objeto
    {
        static Texture2D texture;

        public Caminho(int x, int y)
            : base(0, x, y)
        {
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\chaovazio");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }

        override public Boolean ehCaminho()
        {
            return true;
        }
    }


    class CaminhoV : Objeto
    {
        static Texture2D texture;

        public CaminhoV(int x, int y)
            : base(0, x, y)
        {
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\caminho2");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }

        override public Boolean ehCaminho()
        {
            return true;
        }
    }

    class CaminhoH : Objeto
    {
        static Texture2D texture;

        public CaminhoH(int x, int y)
            : base(0, x, y)
        {
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\caminhoHoriz");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }

        override public Boolean ehCaminho()
        {
            return true;
        }
    }

    class Topo : Objeto
    {
        static Texture2D texture;

        public Topo(int x, int y)
            : base(1, x, y)
        {
            Level.level.bloqueios[x, y] = true;
            Level.level.bloqueios[x + 1, y] = true;
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\TopoCopa");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }

    class Topo2 : Objeto
    {
        static Texture2D texture;

        public Topo2(int x, int y)
            : base(1, x, y)
        {
            Level.level.bloqueios[x, y] = true;
            Level.level.bloqueios[x + 1, y] = true;
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\TopoCopa2");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }


    class AF1 : Objeto
    {
        static Texture2D texture;

        public AF1(int x, int y)
            : base(1, x, y)
        {
            Level.level.bloqueios[x, y] = true;
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\af1");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }
    class AF2 : Objeto
    {
        static Texture2D texture;

        public AF2(int x, int y)
            : base(1, x, y)
        {
            Level.level.bloqueios[x, y] = true;
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\af2");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }

    class AF3 : Objeto
    {
        static Texture2D texture;

        public AF3(int x, int y)
            : base(1, x, y)
        {
            Level.level.bloqueios[x, y] = true;
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\af3");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }

    class AF4 : Objeto
    {
        static Texture2D texture;

        public AF4(int x, int y)
            : base(1, x, y)
        {
            Level.level.bloqueios[x, y] = true;
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\af4");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }

    class AF5 : Objeto
    {
        static Texture2D texture;

        public AF5(int x, int y)
            : base(1, x, y)
        {
            Level.level.bloqueios[x, y] = true;
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\af5");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }

    class AF6 : Objeto
    {
        static Texture2D texture;

        public AF6(int x, int y)
            : base(1, x, y)
        {
            Level.level.bloqueios[x, y] = true;
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\af6");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }

    class AF7 : Objeto
    {
        static Texture2D texture;

        public AF7(int x, int y)
            : base(1, x, y)
        {
            Level.level.bloqueios[x, y] = true;
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\af7");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }

    class Parede : Objeto
    {
        static Texture2D texture;

        public Parede(int x, int y)
            : base(0, x, y)
        {
            Level.level.bloqueios[x, y] = true;
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\parede");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }

    class Arvore : Objeto
    {
        static Texture2D texture;

        public Arvore(int x, int y)
            : base(1, x, y)
        {
            Level.level.bloqueios[x + 1, y + 3] = true;
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\arvoreok");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }
    }

    class Pedra : Objeto
    {
        public Pedra(int x, int y)
            : base(1, x, y)
        {
            Rock rock = new Rock();
            rock.Initialize(x, y);
            Level.level.rocks.Add(rock);
        }

        override public void desenhar(SpriteBatch spriteBatch, int x, int y, float dx, float dy)
        {
        }
    }


    class TotemObj : Objeto
    {
        public TotemObj(int x, int y, int dir, float speed)
            : base(1, x, y)
        {
            Level.level.bloqueios[x, y] = true;
            Totem totem = new Totem();
            //totem.Initialize(new Vector2(x * Level.GRADE, y * Level.GRADE), new System.TimeSpan(50000000));
            if (speed < 1)
                speed += 0.25f;
            totem.Initialize(new Vector2(x * Level.GRADE, y * Level.GRADE), speed);
            if (dir == 0)
                totem.up = true;
            else if (dir == 1)
                totem.right = true;
            else if (dir == 2)
                totem.down = true;
            else
                totem.left = true;

            Level.level.totems.Add(totem);
        }

        override public void desenhar(SpriteBatch spriteBatch, int x, int y, float dx, float dy)
        {
        }
    }

    class Botao : Objeto
    {
        public int x, y;

        public Boolean down;

        public Botao(int x, int y)
            : base(1, x, y)
        {
            this.x = x;
            this.y = y;
        }

        public Boolean press()
        {
            List<People> people = Peoples.followers;
            for (int i = 0; i < people.Count; i++)
            {
                if (people.ElementAt(i).getGradeX() == x && people.ElementAt(i).getGradeY() == y)
                {
                    down = true;
                    return true;
                }
            }

            for (int i = 0; i < Rocks.rocks.Count; i++)
            {
                if (Rocks.rocks.ElementAt(i).getGradeX() == x && Rocks.rocks.ElementAt(i).getGradeY() == y)
                {
                    down = true;
                    return true;
                }
            }
            down = false;
            return false;
        }
    }

    class BotaoCircular : Botao
    {
        static Texture2D texture, texture2;

        public BotaoCircular(int x, int y)
            : base(x, y)
        {
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\b2_1");
            texture2 = content.Load<Texture2D>(@"sprites\b2_2");
        }

        override public Texture2D getTexture(int quadro)
        {
            if (down)
                return texture2;
            return texture;
        }

    }

    class BotaoRet : Botao
    {
        static Texture2D texture, texture2;

        public Boolean solved;

        public int x, y;

        public BotaoRet(int x, int y)
            : base(x, y)
        {
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\b1");
            texture2 = content.Load<Texture2D>(@"sprites\b2");
        }

        override public Texture2D getTexture(int quadro)
        {
            if (down || solved)
                return texture2;
            return texture;
        }
    }

    class Porta : Objeto
    {
        static Texture2D texture;
        int x, y;
        public Boolean aberta;

        public Porta(int x, int y)
            : base(1, x, y)
        {
            this.x = x;
            this.y = y;
            aberta = false;

            Level.level.bloqueios[x, y] = true;
        }

        public static void load(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"sprites\porta");
        }

        override public Texture2D getTexture(int quadro)
        {
            return texture;
        }

        public void abrir()
        {
            Level.level.bloqueios[x, y] = false;
            aberta = true;
        }

        public void fechar()
        {
            Level.level.bloqueios[x, y] = true;
            aberta = false;
        }

        static Rectangle rect = new Rectangle();
        static Rectangle rect2 = new Rectangle();
        override public void desenhar(SpriteBatch spriteBatch, int x, int y, float dx, float dy)
        {
            x *= Level.GRADE;
            y *= Level.GRADE;

            Texture2D texture = getTexture();
            rect.X = x - (int)dx;
            rect.Y = y - (int)dy;
            rect.Width = Level.GRADE;
            rect.Height = texture.Height;

            if (aberta)
            {
                rect2.X = Level.GRADE;
                rect2.Y = 0;
                rect2.Width = Level.GRADE;
                rect2.Height = texture.Height;
            }
            else
            {
                rect2.X = 0;
                rect2.Y = 0;
                rect2.Width = Level.GRADE;
                rect2.Height = texture.Height;
            }

            if (texture != null)
                spriteBatch.Draw(texture, rect, rect2, Color.White);
        }

    }


    class Init : Objeto
    {
        public Init(int x, int y)
            : base(3, x, y)
        {
        }

        override public void desenhar(SpriteBatch spriteBatch, int x, int y, float dx, float dy)
        {

        }

        public virtual void onPassar()
        {

        }
    }

}
