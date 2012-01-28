using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaiaSequel
{
    class StairTile:Tile
    {

        char type;
        //Constructor so we can add all the information to the tile
        //and add special treatment for each tile
        public StairTile(String[] data){
            init(data);
            type = data[4].ToArray()[0];

        }
    }
}
