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

    class Camera
    {
        public int linhas, colunas;
        public float x, y;

        public float x0, x1, y0, y1;

        public Camera(int linhas, int colunas)
        {
            this.linhas = linhas;
            this.colunas = colunas;
            setLimites();
        }

        public void set(float x, float y)
        {
            this.x = x;
            this.y = y;

            if (x < x0)
                x = x0;
            if (y < y0)
                y = y0;
            if (x + colunas * Level.GRADE > x1)
                x = x1;
            if (y + linhas * Level.GRADE > y1)
                y = y1;
        }

        public void setLimites()
        {
            x0 = 0;
            x1 = colunas * Level.GRADE;
            y0 = 0;
            y1 = linhas * Level.GRADE;
        }

        public void setLimites(float x0, float y0, float x1, float y1)
        {
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;
        }

        public void setCenter(float x, float y)
        {
            set(x - (colunas / 2.0f) * Level.GRADE, y - (linhas / 2.0f) * Level.GRADE);
        }
    }

    class Level
    {
        public static int GRADE = 64;
        public static int CAMADAS = 3;
        public static int LINHAS = 10;
        public static int COLUNAS = 10;

        public static Level level;
        public static int levelX, levelY;

        public Objeto[] objetos;

        public Boolean[,] bloqueios;

        public int respawX;
        public int respawY;

        public static Peoples peoples;

        public BotaoRet[] botao = new BotaoRet[5];
        public int botoes;
        public List<BotaoCircular> botaoRedondo = new List<BotaoCircular>();

        public List<Totem> totems = new List<Totem>();
        public List<Rock> rocks = new List<Rock>();

        public int resgate;

        public Level()
        {
            Level.level = this;
            objetos = new Objeto[CAMADAS * LINHAS * COLUNAS];
            bloqueios = new Boolean[COLUNAS, LINHAS];
        }

        static public void  initialize(Peoples peoples){
            Level.peoples=peoples;
        }

        public Objeto getObjeto(int camada, int x, int y)
        {
            if (x < 0 || y < 0 || x >= COLUNAS || y >= LINHAS)
                return null;
            return objetos[camada * LINHAS * COLUNAS + y * COLUNAS + x];
        }

        public void setObjeto(Objeto objeto, int camada, int x, int y)
        {
            objetos[camada * LINHAS * COLUNAS + y * COLUNAS + x] = objeto;
        }

        public void atualizar()
        {
            int i = 0;
            for (i=0; i < botoes; i++)
                botao[i].press();

            for (i=0; i < botoes; i++)
            {
                if (botao[i]!=null && !botao[i].press())
                    break;
            }
            if (i == botoes)
            {
                for (i = 0; i < botoes; i++)
                    botao[i].solved = true;
                onPuzzle();
            }

            
            for (i = 0; i < botaoRedondo.Count; i++)
            {
                if (botaoRedondo[i].press())
                    this.onPress(botaoRedondo[i].x, botaoRedondo[i].y);
                else
                    this.onRelease(botaoRedondo[i].x, botaoRedondo[i].y);
            }

            for (int k = 0; k < CAMADAS; k++)
            {
                for (i = 0; i < LINHAS; i++)
                {
                    for (int j = 0; j < COLUNAS; j++)
                    {
                        if (getObjeto(k, j, i) != null)
                            getObjeto(k, j, i).atualizar(j, i);
                    }
                }
            }
        }

        public void desenhar(SpriteBatch spriteBatch, int k)
        {
            spriteBatch.Begin();
            for (int i = 0; i < LINHAS; i++)
            {
                for (int j = 0; j < COLUNAS; j++)
                {
                    if (getObjeto(k, j, i) != null)
                        getObjeto(k, j, i).desenhar(spriteBatch, j, i, 0, 0);
                }
            }
            spriteBatch.End();
        }

        public Boolean bloqueio(int x, int y)
        {
            return bloqueios[x, y];
        }

        public void ativar(int x, int y)
        {
            getObjeto(0, x, y).ativar();
        }
        public void passar(int x, int y)
        {
            getObjeto(0, x, y).onPassar();
        }

        public static Level LoadFromFile(string fileName)
        {
            Level.level = new Level();

            XmlDocument doc = new XmlDocument();

            doc.Load(fileName);
            XmlNodeList gramas;
            try
            {
            gramas = doc.GetElementsByTagName("Grama").Item(0).ChildNodes;
            for (int i = 0; i < gramas.Count; i++)
            {
                int x = Int32.Parse(gramas.Item(i).Attributes[0].Value);
                int y = Int32.Parse(gramas.Item(i).Attributes[1].Value);
                new Grama(x, y);
            }
            }
            catch (Exception e) { }
            try
            {
            gramas = doc.GetElementsByTagName("Grama2").Item(0).ChildNodes;
            for (int i = 0; i < gramas.Count; i++)
            {
                int x = Int32.Parse(gramas.Item(i).Attributes[0].Value);
                int y = Int32.Parse(gramas.Item(i).Attributes[1].Value);
                new Grama2(x, y);
            }
            }
            catch (Exception e) { }
            try
            {
            gramas = doc.GetElementsByTagName("Grama3").Item(0).ChildNodes;
            for (int i = 0; i < gramas.Count; i++)
            {
                int x = Int32.Parse(gramas.Item(i).Attributes[0].Value);
                int y = Int32.Parse(gramas.Item(i).Attributes[1].Value);
                new Grama3(x, y);
            }
                }catch(Exception e){}
            try
            {
            gramas = doc.GetElementsByTagName("Grama4").Item(0).ChildNodes;
            for (int i = 0; i < gramas.Count; i++)
            {
                int x = Int32.Parse(gramas.Item(i).Attributes[0].Value);
                int y = Int32.Parse(gramas.Item(i).Attributes[1].Value);
                new Grama4(x, y);
            }
                }catch(Exception e){}

            XmlNodeList caminhos = doc.GetElementsByTagName("Caminho").Item(0).ChildNodes;
            for (int i = 0; i < caminhos.Count; i++)
            {
                int x = Int32.Parse(caminhos.Item(i).Attributes[0].Value);
                int y = Int32.Parse(caminhos.Item(i).Attributes[1].Value);
                new Caminho(x, y);
            }
            caminhos = doc.GetElementsByTagName("CaminhoVert").Item(0).ChildNodes;
            for (int i = 0; i < caminhos.Count; i++)
            {
                int x = Int32.Parse(caminhos.Item(i).Attributes[0].Value);
                int y = Int32.Parse(caminhos.Item(i).Attributes[1].Value);
                new CaminhoV(x, y);
            }
            caminhos = doc.GetElementsByTagName("CaminhoHoriz").Item(0).ChildNodes;
            for (int i = 0; i < caminhos.Count; i++)
            {
                int x = Int32.Parse(caminhos.Item(i).Attributes[0].Value);
                int y = Int32.Parse(caminhos.Item(i).Attributes[1].Value);
                new CaminhoH(x, y);
            }

            try
            {
            caminhos = doc.GetElementsByTagName("Copas1").Item(0).ChildNodes;
            for (int i = 0; i < caminhos.Count; i++)
            {
                int x = Int32.Parse(caminhos.Item(i).Attributes[0].Value);
                int y = Int32.Parse(caminhos.Item(i).Attributes[1].Value);
                new Topo(x*2, y);
            }
            }catch(Exception e){}
            try
            {
            caminhos = doc.GetElementsByTagName("Copas2").Item(0).ChildNodes;
            for (int i = 0; i < caminhos.Count; i++)
            {
                int x = Int32.Parse(caminhos.Item(i).Attributes[0].Value);
                int y = Int32.Parse(caminhos.Item(i).Attributes[1].Value);
                new Topo2(x*2, y);
            }
            }catch(Exception e){}


            try
            {
            caminhos = doc.GetElementsByTagName("af1").Item(0).ChildNodes;
            for (int i = 0; i < caminhos.Count; i++)
            {
                int x = Int32.Parse(caminhos.Item(i).Attributes[0].Value);
                int y = Int32.Parse(caminhos.Item(i).Attributes[1].Value);
                new AF1(x, y);
            }
            }catch(Exception e){}
            try
            {
            caminhos = doc.GetElementsByTagName("af2").Item(0).ChildNodes;
            for (int i = 0; i < caminhos.Count; i++)
            {
                int x = Int32.Parse(caminhos.Item(i).Attributes[0].Value);
                int y = Int32.Parse(caminhos.Item(i).Attributes[1].Value);
                new AF2(x, y);
            }
            }catch(Exception e){}
            try
            {
            caminhos = doc.GetElementsByTagName("af3").Item(0).ChildNodes;
            for (int i = 0; i < caminhos.Count; i++)
            {
                int x = Int32.Parse(caminhos.Item(i).Attributes[0].Value);
                int y = Int32.Parse(caminhos.Item(i).Attributes[1].Value);
                new AF3(x, y);
            }
            }catch(Exception e){}
            try
            {
            caminhos = doc.GetElementsByTagName("af4").Item(0).ChildNodes;
            for (int i = 0; i < caminhos.Count; i++)
            {
                int x = Int32.Parse(caminhos.Item(i).Attributes[0].Value);
                int y = Int32.Parse(caminhos.Item(i).Attributes[1].Value);
                new AF4(x, y);
            }
            }catch(Exception e){}
            try
            {
            caminhos = doc.GetElementsByTagName("af5").Item(0).ChildNodes;
            for (int i = 0; i < caminhos.Count; i++)
            {
                int x = Int32.Parse(caminhos.Item(i).Attributes[0].Value);
                int y = Int32.Parse(caminhos.Item(i).Attributes[1].Value);
                new AF5(x, y);
            }            
            }catch(Exception e){}
            try
            {
                caminhos = doc.GetElementsByTagName("a6").Item(0).ChildNodes;
                for (int i = 0; i < caminhos.Count; i++)
                {
                    int x = Int32.Parse(caminhos.Item(i).Attributes[0].Value);
                    int y = Int32.Parse(caminhos.Item(i).Attributes[1].Value);
                    new AF6(x, y);
                }
            }catch(Exception e){}
            try
            {
            caminhos = doc.GetElementsByTagName("af7").Item(0).ChildNodes;
            for (int i = 0; i < caminhos.Count; i++)
            {
                int x = Int32.Parse(caminhos.Item(i).Attributes[0].Value);
                int y = Int32.Parse(caminhos.Item(i).Attributes[1].Value);
                new AF7(x, y);
            }
            }catch(Exception e){}

            try
            {
                caminhos = doc.GetElementsByTagName("Totens").Item(0).ChildNodes;
                for (int i = 0; i < caminhos.Count; i++)
                {
                    int x = Int32.Parse(caminhos.Item(i).Attributes[1].Value) / GRADE;
                    int y = Int32.Parse(caminhos.Item(i).Attributes[2].Value) / GRADE;
                    int pessoa=0;
                    if(caminhos.Item(i).Attributes.Count==4)
                        pessoa = Int32.Parse(caminhos.Item(i).Attributes[3].Value);

                    String name = caminhos.Item(i).Name;
                    if (name == "Pedra")
                        new Pedra(x, y);
                    if (name == "TotenFrente")
                        new TotemObj(x, y, 2, pessoa);
                    if (name == "TotenEsquerda")
                        new TotemObj(x, y, 3, pessoa);
                    if (name == "TotenDireita")
                        new TotemObj(x, y, 1, pessoa);
                    if (name == "TotenTras")
                        new TotemObj(x, y, 0, pessoa);
                    if (name == "BotãoQuadrado")
                            Level.level.botao[Level.level.botoes++] = new BotaoRet(x, y);
                    if (name == "BotãoCirculo")
                            Level.level.botaoRedondo.Add(new BotaoCircular(x, y));
                    if (name == "PortaVertical")
                            new Porta(x, y);
                    if (name == "PortaHorizontal")
                        new Porta(x, y);
                }
            }
            catch (Exception e) { }
            /*
            try
            {
                XmlNodeList inits = doc.GetElementsByTagName("Init").Item(0).ChildNodes;
                Level.level.respawX = new int[inits.Count];
                Level.level.respawY = new int[inits.Count];
                for (int i = 0; i < inits.Count; i++)
                {
                    int x = Int32.Parse(inits.Item(i).Attributes[0].Value);
                    int y = Int32.Parse(inits.Item(i).Attributes[1].Value);
                    Level.level.respawX[i] = x;
                    Level.level.respawY[i] = y;
                }
            }
            catch (Exception e) { }
            */
            return Level.level;
        }

        public static void trocarLevel(int levelX, int levelY)
        {
            Level.levelX = levelX;
            Level.levelY = levelY;
            Level.level = Levels.levels[levelX, levelY];
            Level.level.respaw();
        }

        public int findV(int x)
        {
            for (int y = 0; y < LINHAS; y++)
            {
                if (getObjeto(0, x, y)!=null && getObjeto(0, x, y).ehCaminho())
                    return y;
            }

            for (int y = 0; y < LINHAS; y++)
            {
                if (this.bloqueios[x, y]==false)
                    return y;
            }
            return -1;
        }

        public int findH(int y)
        {
            for (int x = 0; x < COLUNAS; x++)
            {
                if (getObjeto(0, x, y) != null && getObjeto(0, x, y).ehCaminho())
                    return x;
            }
            for (int x = 0; x < COLUNAS; x++)
            {
                if (this.bloqueios[x, y] == false)
                    return x;
            }
            return -1;
        }

        public void trocar()
        {
            if(!Peoples.person1.isFollowing)
                Peoples.person1.ativo = false;
            if (!Peoples.person2.isFollowing)
                Peoples.person2.ativo = false;
            if (!Peoples.person3.isFollowing)
                Peoples.person3.ativo = false;
            if (!Peoples.person4.isFollowing)
                Peoples.person4.ativo = false;

            if (Level.level.resgate == 1)
                Peoples.person1.ativo = true;

            if (Level.level.resgate == 2)
                Peoples.person2.ativo = true;

            if (Level.level.resgate == 3)
                Peoples.person3.ativo = true;

            if (Level.level.resgate == 4)
                Peoples.person4.ativo = true;

            Totens.totems = Level.level.totems;
            Rocks.rocks = Level.level.rocks;
            Totens.resetar();

            Game1.cenarioOpc = 1;
        }

        public void toLeft()
        {
            if (levelX == 0)
                return;
            levelX--;
            Level.level = Levels.levels[levelY, levelX];

            Level.peoples.Die(COLUNAS-1, Level.level.findV(COLUNAS-1));
            Level.level.respawX = COLUNAS - 1;
            Level.level.respawY = Level.level.findV(COLUNAS - 1);

            Colisions.SetRespawn(Level.level.respawX, Level.level.respawY);

            trocar();
        }
        public void toRight()
        {
            if (levelX >= Levels.COLUNAS - 1)
                return;
            levelX++;
            Level.level = Levels.levels[levelY, levelX];

            Level.peoples.Die(0, Level.level.findV(0));
            Level.level.respawX = 0;
            Level.level.respawY = Level.level.findV(0);
            Colisions.SetRespawn(Level.level.respawX, Level.level.respawY);

            trocar();
        }
        public void toUp()
        {
            if (levelY == 0)
                return;
            levelY--;
            Level.level = Levels.levels[levelY, levelX];

            Level.peoples.Die(Level.level.findH(LINHAS-1), LINHAS-1);
            Level.level.respawX = Level.level.findH(LINHAS - 1);
            Level.level.respawY = LINHAS - 1;
            Colisions.SetRespawn(Level.level.respawX, Level.level.respawY);

            trocar();
        }
        public void toDown()
        {
            if (levelY >= Levels.LINHAS - 1)
                return;
            levelY++;
            Level.level = Levels.levels[levelY, levelX];

            Level.peoples.Die(Level.level.findH(0), 0);
            Level.level.respawX=Level.level.findH(0);
            Level.level.respawY = 0;
            Colisions.SetRespawn(Level.level.respawX, Level.level.respawY);

            trocar();
        }

        public void respaw()
        {
            peoples.Die(respawX, respawY);
        }

        public void onPuzzle()
        {
            if (levelX == 2 && levelY == 2)
            {

            }
            if (levelX == 3 && levelY == 1)
            {
                Totens.totems[0].isFiring = false;
            }
            if (levelX == 2 && levelY == 2)
            {
                Totens.totems[0].isFiring = false;
            }
            if (levelX == 3 && levelY == 3)
            {
                abrirPorta(1, 3);
            }
            if (levelX == 2 && levelY == 3)
            {
                Totens.totems[1].isFiring = false;
            }
            if (levelX == 1 && levelY == 2)
            {
                Totens.totems[0].isFiring = false;
            }
            if (levelX == 0 && levelY == 1)
            {
                abrirPorta(6, 1);
            }
        }

        public void onPress(int x, int y)
        {
            if (levelX == 0 && levelY == 2)
            {
                Totens.totems[1].isFiring = false;
            }
            if (levelX == 0 && levelY == 3)
            {
                if(x==6 && y==8)
                    Totens.totems[0].isFiring = false;
                if (x == 6 && y ==9)
                    Totens.totems[1].isFiring = false;
                if (x == 6 && y == 10)
                    Totens.totems[2].isFiring = false;
            }
            if (levelX == 1 && levelY == 3)
            {
                if(x==3 && y==6)
                    Totens.totems[2].isFiring = false;
                if (x == 6 && y == 2)
                    Totens.totems[0].isFiring = false;
                if (x == 11 && y == 8)
                    Totens.totems[1].isFiring = false;
            }
            if (levelX == 2 && levelY == 0)
            {
                abrirPorta(12, 5);
            }
            if (levelX == 3 && levelY == 0)
            {
                abrirPorta(6, 10);
            }
            if (levelX == 2 && levelY == 3)
            {
                if (x == 11 && y == 9)
                    abrirPorta(8, 8);
                if (x == 8 && y == 5)
                    abrirPorta(0, 6);
            }
        }

        public void onRelease(int x, int y)
        {
            if (levelX == 0 && levelY == 2)
            {
                Totens.totems[1].isFiring = true;
            }
            if (levelX == 0 && levelY == 3)
            {
                if (x == 6 && y == 8)
                    Totens.totems[0].isFiring = true;
                if (x == 6 && y == 9)
                    Totens.totems[1].isFiring = true;
                if (x == 6 && y == 10)
                    Totens.totems[2].isFiring = true;
            }
            if (levelX == 1 && levelY == 3)
            {
                if (x == 3 && y == 6)
                    Totens.totems[2].isFiring = true;
                if (x == 6 && y == 2)
                    Totens.totems[0].isFiring = true;
                if (x == 11 && y == 8)
                    Totens.totems[1].isFiring = true;
            }

            if (levelX == 2 && levelY == 0)
            {
                fecharPorta(12, 5);
            }
            if (levelX == 3 && levelY == 0)
            {
                fecharPorta(6, 10);
            }
            if (levelX == 2 && levelY == 3)
            {
                if (x == 11 && y == 9)
                    fecharPorta(8, 8);
                if (x == 8 && y == 5)
                    fecharPorta(0, 6);
            }
        }

        public void abrirPorta(int x, int y)
        {
            ((Porta)this.getObjeto(1, x, y)).abrir();
        }

        public void fecharPorta(int x, int y)
        {
            ((Porta)this.getObjeto(1, x, y)).fechar();
        }

        public Boolean ehRocha(int x, int y)
        {
            foreach (Rock rock in Rocks.rocks)
            {
                if (rock.getGradeX() == x && rock.getGradeY() == y)
                    return true;
            }
            return false;
        }

        public People getPessoa(int x, int y)
        {
            if (Peoples.person1.getGradeX() == x && Peoples.person1.getGradeY() == y)
                return Peoples.person1;
            if (Peoples.person2.getGradeX() == x && Peoples.person2.getGradeY() == y)
                return Peoples.person2;
            if (Peoples.person3.getGradeX() == x && Peoples.person3.getGradeY() == y)
                return Peoples.person3;
            if (Peoples.person4.getGradeX() == x && Peoples.person4.getGradeY() == y)
                return Peoples.person4;
            return null;
        }
    }

    class Levels
    {
        public static int LINHAS=4;
        public static int COLUNAS=4;

        public static Level [,] levels=new Level[LINHAS, COLUNAS];


        public static void initialize()
        {
            levels[0, 0].resgate = 1;
            levels[0, 2].resgate = 2;
            levels[3, 0].resgate = 3;
            levels[2, 3].resgate = 4;

            Level.level = levels[1, 0];
            Level.levelX = 0;
            Level.levelY = 1;
            Totens.totems = Level.level.totems;
            Rocks.rocks = Level.level.rocks;
        }

        public static void load(ContentManager content)
        {
            levels[0, 0] = Level.LoadFromFile("Content\\levels\\A1.oel");
            levels[0, 1] = Level.LoadFromFile("Content\\levels\\B1.oel");
            levels[0, 2] = Level.LoadFromFile("Content\\levels\\C1.oel");
            levels[0, 3] = Level.LoadFromFile("Content\\levels\\D1.oel");

            levels[1, 0] = Level.LoadFromFile("Content\\levels\\A2.oel");
            levels[1, 1] = Level.LoadFromFile("Content\\levels\\B2.oel");
            levels[1, 2] = Level.LoadFromFile("Content\\levels\\C2.oel");
            levels[1, 3] = Level.LoadFromFile("Content\\levels\\D2.oel");

            levels[2, 0] = Level.LoadFromFile("Content\\levels\\A3.oel");
            levels[2, 1] = Level.LoadFromFile("Content\\levels\\B3.oel");
            levels[2, 2] = Level.LoadFromFile("Content\\levels\\C3.oel");
            levels[2, 3] = Level.LoadFromFile("Content\\levels\\D3.oel");

            levels[3, 0] = Level.LoadFromFile("Content\\levels\\A4.oel");
            levels[3, 1] = Level.LoadFromFile("Content\\levels\\B4.oel");
            levels[3, 2] = Level.LoadFromFile("Content\\levels\\C4.oel");
            levels[3, 3] = Level.LoadFromFile("Content\\levels\\D4.oel");

            Grama.load(content);
            Grama2.load(content);
            Grama3.load(content);
            Grama4.load(content);
            Parede.load(content);
            Caminho.load(content);
            CaminhoH.load(content);
            CaminhoV.load(content);
            Arvore.load(content);
            AF1.load(content);
            AF2.load(content);
            AF3.load(content);
            AF4.load(content);
            AF5.load(content);
            AF6.load(content);
            AF7.load(content);
            Topo.load(content);
            Topo2.load(content);
            BotaoRet.load(content);
            BotaoCircular.load(content);
            Porta.load(content);
        }
        
    }
}
