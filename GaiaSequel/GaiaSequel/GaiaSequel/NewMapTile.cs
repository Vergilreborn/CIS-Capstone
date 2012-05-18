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

    //The tile for changing the map to the correct level.
    class NewMapTile :Tile
    {

        //Checks to see if the door is a door inwhich you must
        //hit the button/action button
        bool clickableDoor;
        Vector2 newPosition;
        int nextLevel;

        //The newMapTile to indicate changing maps
        public NewMapTile(String[] data)
        {
            //lvl,0,0,20,28,0,3,3,False,letter,letter,True
            String[] newData = { data[1], data[2], (int.Parse(data[3])*16-5)+"", ""+(int.Parse(data[4])*16-5) };
            init(newData);
            nextLevel = int.Parse(data[5]);
            newPosition = new Vector2(int.Parse(data[6]), int.Parse(data[7]));

            if (data[8].Equals("True"))
                clickableDoor = true;
            else
                clickableDoor = false;

        }

        //Return to test if the door must be clickable
        public bool getClickable()
        {
            return clickableDoor;
        }

        public int getLevel()
        {
            return nextLevel;
        }

        public Vector2 getNextLvlPos()
        {
            return newPosition;
        }



    }
}
