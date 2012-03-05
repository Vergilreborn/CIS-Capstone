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
using System.Text;

namespace GaiaSequel
{
    class MainPlayerData
    {

        //data is composed of width,height, 
        //Frames windFrames   ,jumpFrames    ,walkFrames,    climbFrames
        //       windFLocationx,y, jumpFLocation,wFrameLocation,clumbFrameLocation
        public int[] willData = {3,0,0,5,7,0,4,3,0,4,3,4};
        public int[] freedanData = {4,0,0,5,8,0,4,4,0,4,4,4};
        public int[] shadowData;
        public int[] novaData;
        public int[] devonControllerData;
        Texture2D will;
        Texture2D freedan;

        public void setTextures(Texture2D will, Texture2D freedan){
            this.will = will;
            this.freedan = freedan;

        }
        public int[] loadPlayer(int player){
            switch (player)
            {
                case 1: return willData;
                case 2: return freedanData;
            }
            return willData;
        }
        public Texture2D loadSheet(int player)
        {
            switch (player)
            {
                case 1: return will;
                case 2: return freedan;
            }
            return will;
        }


    }
}
