using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaiaSequel
{
    class WalkableTile:Tile
    {
       
        //Constructor so we can add all the information to the tile
        //and add special treatment for each tile
        public WalkableTile(String [] data){
            init(data);
        }

        //This tests to see if the sprite is on this sprite;
        public bool onThis(MainPlayer player)
        {
            

            //Players collision info
            float pX = player.footArea.X;
            float pY = player.footArea.Y;
            float pWidth = player.footArea.Width;
            float pHeight = player.footArea.Height;

            //this info
            float tX = this.destRect.X;
            float tY = this.destRect.Y;
            float tWidth = this.destRect.Width;
            float tHeight = this.destRect.Height;

            //returns if the player is touching this square revealing he is "techniqually" on this.
            return ((tX <= pX && pX <= (tX + tWidth) || (tX <= (pX + pWidth) && (pX + pWidth) <= (tX + tWidth))) &&
               ((tY <= pY && pY <= (tY + tHeight)) || (tY <= (pY + pHeight) && (pY + pHeight) <= (tY + tHeight)))) ;
        }


    }
}
