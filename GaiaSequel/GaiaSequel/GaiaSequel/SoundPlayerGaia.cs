using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GaiaSequel
{
    class SoundPlayerGaia
    {
        ContentManager contentInfo;
        SoundEffect[] sounds = new SoundEffect[6];
        public void init(ContentManager content)
        {

            this.contentInfo = content;

            sounds[0] = contentInfo.Load<SoundEffect>("Sounds/selectSound");
            sounds[1] = contentInfo.Load<SoundEffect>("Sounds/confirmSound");
            sounds[2] = contentInfo.Load<SoundEffect>("Sounds/healthIncreaseSound");
            sounds[3] = contentInfo.Load<SoundEffect>("Sounds/attackSoundWill");
            sounds[4] = contentInfo.Load<SoundEffect>("Sounds/enemyHit");
            sounds[5] = contentInfo.Load<SoundEffect>("Sounds/talkingSound");
            
        }
        public void playSound(int soundIndex){
            sounds[soundIndex].Play();
           
        }

        public void playSounds(int[] sounds){
            for (int i = 0; i < sounds.Length; i++)
                playSound(sounds[i]);
        }
    }
}
