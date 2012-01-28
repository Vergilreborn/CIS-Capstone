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
    class Tile
    {

        //keeps track of position, the source rect on spritesheet
        //and the destination rect
        public Vector2 position;
        public Rectangle sourceRect;
        public Rectangle destRect;
        
        //Keeps track of the sprites location on tilesheet
        public Vector2 spriteNumber;
       
        //need to initiate all the variables and tile
        public void init(String [] data){
        
            spriteNumber = new Vector2(int.Parse(data[0]), int.Parse(data[1]));
            position = new Vector2(2 * int.Parse(data[2]), 2 * int.Parse(data[3]));
            destRect = new Rectangle((int)position.X, (int)position.Y, 32, 32);
            sourceRect= new Rectangle((int)spriteNumber.X * 32, (int)spriteNumber.Y * 32, 32, 32);
        }
    }
}
