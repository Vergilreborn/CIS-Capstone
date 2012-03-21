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
        //Read a File to import the data
        //---------------Implement----------//

        //data is composed of width,height, 
        //Frames windFrames   ,jumpFrames    ,walkFrames,    climbFrames
        //       windFLocationx,y, jumpFLocation,wFrameLocation,clumbFrameLocation
        public int[] willData = {3,0,0,5,7,0,4,3,0,4,3,4};
        public int[] freedanData = {4,0,0,5,8,0,4,4,0,4,4,4};
        public int[] willAttackingData = {7,0,0};
        public int[] freedanAttackingData = {0};
        public int[] shadowData;
        public int[] novaData;
        public int[] devonControllerData;
        Texture2D will;
        Texture2D willAttacking;
        Texture2D freedan;
        Texture2D freedanAttacking;

        //This method is used for instantiating the textures for the players
        public void setTextures(Texture2D will,Texture2D willAttacking, Texture2D freedan,Texture2D freedanAttacking){
            this.will = will;
            this.willAttacking = willAttacking;
            this.freedan = freedan;
            this.freedanAttacking = freedanAttacking;
        }

        //Load the correct sprite data
        public int[] loadPlayer(int player){
            switch (player)
            {
                case 1: return willData;
                case 2: return freedanData;
            }
            return willData;
        }
        //load the correcting attacking data
        public int[] loadAttackingData(int player)
        {
            switch (player)
            {
                case 1: return willAttackingData;
                case 2: return freedanAttackingData;
            }
            return willAttackingData;
        }

        //load the correct sprite sheet
        public Texture2D loadSheet(int player)
        {
            switch (player)
            {
                case 1: return will;
                case 2: return freedan;
            }
            return will;
        }

        //Load the attacking sheet Data for the player
        public Texture2D loadAttackingSheet(int player)
        {
            switch (player)
            {
                case 1: return willAttacking;
                case 2: return freedanAttacking;
            }
            return willAttacking;
        }
    }

}
