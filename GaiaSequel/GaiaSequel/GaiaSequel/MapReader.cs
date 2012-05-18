using System;
using System.IO;
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
    class MapReader
    {

        //String path
        String path = "";

        //Have storage for the Tiles to be filled
        public List<WalkableTile> walkable;
        public List<LadderTile> ladders;
        public List<WaterTile> water;
        public List<WallTile> walls;
        public List<EmptyTile> nothing;
        public List<StairTile> stairs;
        public List<NewMapTile> nextMap;
        
        //Map Size Variable (will be used for camera)
        int xMapSize = 0;
        int yMapSize = 0;

        //This will be used to read the file
        StreamReader reader;

        //String level
        public int currentLvl;

        //The tiles texture pack
        Texture2D mapTiles;

        //detects to change map with transition for ALLGUI CLASS
        bool change;

        //A constuctor that loads the textures
        public MapReader(Texture2D mapTiles){
            this.mapTiles = mapTiles;
            change = false;
        }
        //Initialize all the lists so we can add data to them
        //the data will be in forms of tiles and sorted by collision
        public void init() {
            this.walkable = new List<WalkableTile>();
            this.nothing = new List<EmptyTile>();
            this.walls = new List<WallTile>();
            this.water = new List<WaterTile>();
            this.ladders = new List<LadderTile>();
            this.stairs = new List<StairTile>();
            this.nextMap = new List<NewMapTile>();
        }

        //This will construct the map according to the given
        //path of the file name
        public void buildMap(String path,int lvl){
            //connect the streamreader to the file
            currentLvl = lvl;
            this.path = path;
            reader = new StreamReader(path + "" + currentLvl  + ".gmaps");

            //Grab the line and make sure its for the "Map Tiles"
            String line = reader.ReadLine();
            int next = 0;
            if (line.Equals("[MapTiles]")){
                //get the map sizes
                setSize(reader.ReadLine());
                next = fillTileLists();
            }
          
            //loads the objects as in enemies...etc
            if(next == 1)
                checkObjects(reader);

            //closes the file when we are finished
            reader.Close();

        }

        //Load the new map
        public void loadNext(int level){
            currentLvl = level;
            reader = new StreamReader(path + "" + currentLvl + ".gmaps");

            init();
            //Grab the line and make sure its for the "Map Tiles"
            String line = reader.ReadLine();
            int next = 0;

            //loads the map tiles
            if (line.Equals("[MapTiles]")){
                //get the map sizes
                setSize(reader.ReadLine());
                next = fillTileLists();
            }

            //checks to see if there are objects next
            if(next == 1)
                checkObjects(reader);
            
            //closes the file when we are finished
            reader.Close();


        }


        //Reset the tiles


        //sets the map sizes according to the given file
        public void setSize(String sizeLine){

            //Divide the info into an array and parse values
            String[] data = new String[2];
            data = sizeLine.Split(',');
            xMapSize = int.Parse(data[0]);
            yMapSize = int.Parse(data[1]);
        }

        //Fills the Lists with correct data
        public int fillTileLists(){
            //Checks to see if we are not at the end of the file
            while (reader.Peek() != -1){

              

                //Grab the line we peeked at
                String line = reader.ReadLine();

                if (line.Equals("[Objects]"))
                    return 1;

                //create array and store the data
                String[] data = new String[5];
                data = line.Split(',');
                //use the data according to the collision type and sort
                switch (line.ToCharArray()[line.Length - 1])
                {
                    case 'n': nothing.Add(new EmptyTile(data)); break;
                    case 'l': walkable.Add(new WalkableTile(data)); break;
                    case 'w': walls.Add(new WallTile(data)); break;
                    case 'e': ladders.Add(new LadderTile(data)); break;
                    case 'p': water.Add(new WaterTile(data)); break;
                    case 'r': stairs.Add(new StairTile(data)); break;
                    case 's': stairs.Add(new StairTile(data)); break;
                    case 'u': stairs.Add(new StairTile(data)); break;
                    case 'i': stairs.Add(new StairTile(data)); break;

                }

            }
            return 0;
            
        }

        public void checkObjects(StreamReader reader)
        {
        

            while (reader.Peek() != -1)
            {
                String line = reader.ReadLine();

                //create array and store the data
                String[] data = line.Split(',');

                switch (data[0])
                {
                    case "lvl": nextMap.Add(new NewMapTile(data)); break;  

                }
                


            }


        }

        //Change change
        public void setChange()
        {
            change = !change;
        }

        //get to see if the map is changing
        public bool getChange()
        {
            return change;   
        }

        //This will draw the tiles on the screen. Collision detection will
        //be done in the enemies and the players own .cs files
        public void Draw(SpriteBatch spriteBatch, bool normal){
            if (!normal)
            {
                foreach (EmptyTile n in nothing)
                {
                    spriteBatch.Draw(mapTiles, n.destRect, n.sourceRect, Color.White);
                }
                foreach (WalkableTile l in walkable)
                {
                    spriteBatch.Draw(mapTiles, l.destRect, l.sourceRect, Color.Blue);
                }
                foreach (LadderTile e in ladders)
                {
                    spriteBatch.Draw(mapTiles, e.destRect, e.sourceRect, Color.Green);
                }
                foreach (WaterTile p in water)
                {
                    spriteBatch.Draw(mapTiles, p.destRect, p.sourceRect, Color.Purple);
                }
                foreach (WallTile w in walls)
                {
                    spriteBatch.Draw(mapTiles, w.destRect, w.sourceRect, Color.Red);
                }
                foreach (StairTile s in stairs)
                {
                    spriteBatch.Draw(mapTiles, s.destRect, s.sourceRect, Color.Yellow);
                }
            }
            else
            {
                foreach (EmptyTile n in nothing)
                {
                    spriteBatch.Draw(mapTiles, n.destRect, n.sourceRect, Color.White);
                }
                foreach (WalkableTile l in walkable)
                {
                    spriteBatch.Draw(mapTiles, l.destRect, l.sourceRect, Color.White);
                }
                foreach (LadderTile e in ladders)
                {
                    spriteBatch.Draw(mapTiles, e.destRect, e.sourceRect, Color.White);
                }
                foreach (WaterTile p in water)
                {
                    spriteBatch.Draw(mapTiles, p.destRect, p.sourceRect, Color.White);
                }
                foreach (WallTile w in walls)
                {
                    spriteBatch.Draw(mapTiles, w.destRect, w.sourceRect, Color.White);
                }
                foreach (StairTile s in stairs)
                {
                    spriteBatch.Draw(mapTiles, s.destRect, s.sourceRect, Color.White);
                }

            }
        
        }
    }
}
