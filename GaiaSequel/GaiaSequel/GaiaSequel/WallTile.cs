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
    class WallTile:Tile
    {

       
        public bool isUsed;

        public WallTile(){
            isUsed = false;
        }
        //Constructor of the WallTile class
        public WallTile(String[] data){
            init(data);
            isUsed = true;
        }

        //This test to see if we collided with a Wall. If we did we will
        //put the player against the items and stuff
        public bool colliding(Rectangle ahead){

            

            //Players collision info
            float pX = ahead.X;
            float pY = ahead.Y;
            float pWidth = ahead.Width;
            float pHeight = ahead.Height;

            //this info
            float tX = this.destRect.X;
            float tY = this.destRect.Y;
            float tWidth = this.destRect.Width;
            float tHeight = this.destRect.Height;

            //returns if the player is touching this square revealing he is "techniqually" on this.
            return ((tX <= pX && pX <= (tX + tWidth) || (tX <= (pX + pWidth) && (pX + pWidth) <= (tX + tWidth))) &&
              ((tY <= pY && pY <= (tY + tHeight)) || (tY <= (pY + pHeight) && (pY + pHeight) <= (tY + tHeight))));
       
        }           
    }
}
