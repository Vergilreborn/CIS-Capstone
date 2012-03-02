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
    class LadderTile:Tile
    {

        public LadderTile(String[] data){
            init(data);
        }

        public bool onLadder(Rectangle playerFeet){
            bool checker = (playerFeet.X >= this.destRect.X) && ((playerFeet.X + playerFeet.Width) <= (this.destRect.X + this.destRect.Width));
            return checker && this.destRect.Intersects(playerFeet);
            //return true;

        }
        
    }
}
