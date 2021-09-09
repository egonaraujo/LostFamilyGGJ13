using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace exploracao
{
    class Colisions
    {
        public static int XRespawn;
        public static int YRespawn;

        public static void SetRespawn(int x, int y)
        {
            XRespawn = x;
            YRespawn = y;

        }

        public static void Die(int x, int y)
        {
            Level.peoples.Die(x, y);
        }

        public static void updateColisions(GameTime gameTime)
        {
            foreach (Totem totem in Totens.totems)
            {
                foreach (Arrow arrow in totem.arrows)
                {
                    foreach (People person in Peoples.followers)
                    {
                        if (arrow.collision.Intersects(person.collision))
                        {
                            Audio.soundBank.PlayCue("Flash2");
                            arrow.active = false;
                            Die(XRespawn, YRespawn);

                            break;
                        }
                    }
                }

            }
            foreach (Rock rock in Rocks.rocks)
            {
                foreach (People person in Peoples.followers)
                {
                    if (rock.collision.Intersects(person.collision))
                    {
                        if (person.walkingDown)
                            rock.movingDown = true;
                        else if (person.walkingLeft)
                            rock.movingLeft = true;
                        else if (person.walkingRight)
                            rock.movingRight = true;
                        else if (person.walkingUp)
                            rock.movingUp = true;

                        break;
                    }
                }
                foreach (Totem totem in Totens.totems)
                {
                    foreach (Arrow arrow in totem.arrows)
                    {
                        if (rock.collision.Intersects(arrow.collision))
                        {
                            Audio.soundBank.PlayCue("Flash2");
                            arrow.active = false;
                        }
                    }
                }
            }

        }

    }
}
