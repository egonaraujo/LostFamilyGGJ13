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
    class Audio
    {
        //SONS
        public static AudioEngine audioEngine;
        public static SoundBank soundBank;
        public static WaveBank waveBank;

        //AudioCategory heartCategory;
        public static SoundEffectInstance soundHeart;
        public static AudioCategory musicCategory;
        public static AudioCategory defaultCategory;
        public static AudioCategory effectCategory;

        public static Cue[] musicas = new Cue[4];
        public static Cue soundNatureza;

        public static void Initialize()
        {
            // TODO: Add your initialization logic here
            audioEngine = new AudioEngine("Content\\sounds\\HeartBeat.xgs");

            waveBank = new WaveBank(audioEngine, "Content\\sounds\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\sounds\\Sound Bank.xsb");

            // Get the category.
            defaultCategory = audioEngine.GetCategory("Default");
            effectCategory = audioEngine.GetCategory("Effects");
            musicCategory = audioEngine.GetCategory("Music");
            musicCategory.SetVolume(1);

            // Play the sound
            soundBank.PlayCue("natureza");
        }

        public static void Load(ContentManager content)
        {
            SoundEffect sndBatida = content.Load<SoundEffect>(@"sounds\Heartbeat");

            // carrega a trilha sonora
            musicas[0] = soundBank.GetCue("Musica1");
            musicas[1] = soundBank.GetCue("Musica2");
            musicas[2] = soundBank.GetCue("Musica3");
            musicas[3] = soundBank.GetCue("Musica4");
            //start menu
            musicas[3].Play();

            soundNatureza = soundBank.GetCue("natureza");

            // Play Sound
            soundHeart = sndBatida.CreateInstance();
            soundHeart.IsLooped = true;
            // TODO: use this.Content to load your game content here
        }
    }
}
