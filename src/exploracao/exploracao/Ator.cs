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
    class Ator : BaseClass
    {
        public string nome;
        private SpriteBatch spriteBatch;
        private Vector2 vector2;

        public Boolean encontrado;

        public Ator(SpriteBatch spriteBatch, Vector2 vector2)
            : base(spriteBatch, vector2)
        {
            // TODO: Complete member initialization
            this.spriteBatch = spriteBatch;
            this.vector2 = vector2;
            encontrado = false;
        }

        public void update(float x, float y)
        {
            posicao.X = x;
            posicao.Y = y;
        }

    }

    class Ators
    {
        static float BEAT_RAY = 1600;
        static float FATOR_NORMALIDADE = 0.4f;

        public static float distance;

        public Ator atorPrincipal;
        static public List<Ator> membroFamilia;

        public Ators(SpriteBatch spriteBatch)
        {
            membroFamilia = new List<Ator>();

            Ator ator1 = new Ator(spriteBatch, new Vector2(0, 0));
            membroFamilia.Add(ator1);
            Ator ator2 = new Ator(spriteBatch, new Vector2(0, 0));
            membroFamilia.Add(ator2);
            Ator ator3 = new Ator(spriteBatch, new Vector2(0, 0));
            membroFamilia.Add(ator3);
            Ator ator4 = new Ator(spriteBatch, new Vector2(0, 0));
            membroFamilia.Add(ator4);
            Ator ator5 = new Ator(spriteBatch, new Vector2(0, 0));
            //membroFamilia.Add(ator5);

            atorPrincipal = new Ator(spriteBatch, new Vector2(0, 0));
        }


        public void update()
        {
            {
                atorPrincipal.update(Peoples.player.globalX(), Peoples.player.globalY());
                membroFamilia[0].update(Peoples.person1.globalX(0), Peoples.person1.globalY(0));
                membroFamilia[1].update(Peoples.person2.globalX(2), Peoples.person2.globalY(0));
                membroFamilia[2].update(Peoples.person3.globalX(0), Peoples.person3.globalY(3));
                membroFamilia[3].update(Peoples.person4.globalX(3), Peoples.person4.globalY(2));

                if (Peoples.person1.isFollowing)
                    membroFamilia[0].encontrado=true;
                if (Peoples.person2.isFollowing)
                    membroFamilia[1].encontrado = true;
                if (Peoples.person3.isFollowing)
                    membroFamilia[2].encontrado = true;
                if (Peoples.person4.isFollowing)
                    membroFamilia[3].encontrado = true;

                distance = float.MaxValue;

                //procura o membro da familia mais proximo
               
                foreach (Ator ator in membroFamilia)
                {
                    if (ator.encontrado) continue;
                    float dist = Vector2.Distance(atorPrincipal.posicao, ator.posicao);

                    if (dist < distance)
                    {
                        distance = dist;
                    }
                }
                
                float beatVolume = FATOR_NORMALIDADE;
                // intensifica a batida quando se aproxima de alguem da familia
                if (distance < BEAT_RAY)
                {
                    beatVolume = 1 - (distance / BEAT_RAY);
                }

                Audio.soundHeart.Volume = beatVolume;
                Audio.soundHeart.Pitch = beatVolume;

                RadarCoracao.heartBeat = beatVolume;
            }
        }
    }
}
