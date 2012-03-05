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
        SoundEffect[] sounds = new SoundEffect[3];
        public void init(ContentManager content)
        {

            this.contentInfo = content;

            sounds[0] = contentInfo.Load<SoundEffect>("Sounds/selectSound");
            sounds[1] = contentInfo.Load<SoundEffect>("Sounds/confirmSound");
            sounds[2] = contentInfo.Load<SoundEffect>("Sounds/healthIncreaseSound");
            
        }
        public void playSound(int soundIndex){
            sounds[soundIndex].Play();
           
        }
    }
}
