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
    class Enemy
    {

        int health;
        int str;
        int def;
        int width;
        int height;
        Vector2 position;
        bool flying;
        Rectangle destRect;
        Rectangle sourceRect;
        Texture2D sprite;
        public Vector2 center;
        String enemyString = "";
        //used for the dominate key
        Keys dominateKey = Keys.End;
        Keys secKey = Keys.End;


        float walkTimer = 0f;
        
        //TEMP MAX WALK FRAMES
        int walkFrames = 4;
        int currWalkFrame = 0;
        MapReader mapDesign;
      
        bool walking = false;
        bool simpleAI;
        int walkDir;
        float leftRightSpeed = 3f;
        float upDownSpeed = 2f;
        Vector2 savedPosition;
        Rectangle footArea;
      

        //AI movement
        float aiTimer = 0f;
        double percentage = 0;
        Random rand;

        public Enemy(int health, int str, int def, Vector2 position, int width, int height, bool flying,Texture2D sprite,bool simpleAI,Random rand){
            this.health = health;
            this.str = str;
            this.def = def;
            this.position = position;
            this.flying = flying;
            this.sprite = sprite;
            this.width = width;
            this.rand = rand;
            this.height = height;
            this.simpleAI = simpleAI;
            sourceRect = new Rectangle(0, 0, width, height);
            destRect = new Rectangle((int)position.X,(int)position.Y, width, height);
            center = new Vector2(destRect.X + (width / 2), destRect.Y + (height / 2));
            footArea = new Rectangle((int)(center.X - 13), (int)(destRect.Y + height - 20), 26, 18);
        }
        public void update(KeyboardState newState, GameTime timer, Vector2 playerPosition){
            walking = false;



            if (simpleAI)
                aiMovement(timer, playerPosition);
            else
            {
                  if (newState.GetPressedKeys().Length == 1 && onlyArrowsPressed(newState.GetPressedKeys()[0]))
                   {
                       dominateKey = newState.GetPressedKeys()[0];
                   }
                  else if (newState.GetPressedKeys().Length > 1 &&
                       (onlyArrowsPressed(newState.GetPressedKeys()[0]) ||
                         onlyArrowsPressed(newState.GetPressedKeys()[1])))
                   {
                         dominateKey = setDominateKey(newState.GetPressedKeys(), dominateKey);
                   }

                if (flying)
                    movementNoColl(newState);
                else
                    movement(newState);
            }
                
            if(walking || flying)
                movingAnimation(timer,walkDir);
            else 
                sourceRect = new Rectangle(0, height * walkDir, width, height);

        
            destRect.X = (int)position.X;
            destRect.Y = (int)position.Y;
            center = new Vector2(destRect.X + (width/2), destRect.Y + (height/2));
            footArea = new Rectangle((int)(center.X -13), (int)(destRect.Y + height -20), 26, 18);
        
          }
        public void connectMap(MapReader map){
            this.mapDesign = map;
        }
        public void turnOffAi(){
            if (simpleAI)
                simpleAI = false;
            else
                simpleAI = true;
        }
        public void aiMovement(GameTime gametime, Vector2 playerPosition){
            if (aiTimer > 0)
            {
                aiTimer -= gametime.ElapsedGameTime.Milliseconds;
                if(withinRange(playerPosition)){
                 
                    if (flying)
                        movementNoColl(dominateKey, secKey);
                    else
                        movement(dominateKey, secKey);
                }

                
            }
            else
            {
                percentage = ((rand.NextDouble() * 100));
                    //*100 * this.center.X *
                     //           this.center.Y * this.position.Y * this.position.X) ) % 100;
               
                if (percentage < 4)
                    aiTimer = rand.Next(1000, 2000);
                enemyString = percentage + " TIMER " + aiTimer;
            }
           
        }

        public bool withinRange(Vector2 playerPosition){
            dominateKey = Keys.Delete;
            secKey = Keys.Delete;
            if ((Math.Abs(playerPosition.X - position.X) > Math.Abs(playerPosition.Y - position.Y)))
            {

                if (this.position.X - 200 < playerPosition.X && this.position.X + 200 > playerPosition.X)
                {
                    if (playerPosition.X+15 < position.X)
                        dominateKey = Keys.Left;
                    else if (playerPosition.X-5 > position.X)
                        dominateKey = Keys.Right;
                }
                /*
                if (this.position.Y - 150 < playerPosition.Y && this.position.Y + 150 > playerPosition.Y)
                {
                    if (playerPosition.Y +20 < position.Y)
                        secKey = Keys.Up;
                    else if (playerPosition.Y-10 > position.Y)
                        secKey = Keys.Down;
                }*/
            }
            else
            {
                if (this.position.Y - 150 < playerPosition.Y && this.position.Y + 150 > playerPosition.Y)
                {
                    if (playerPosition.Y+15< position.Y)
                        dominateKey = Keys.Up;
                    else if (playerPosition.Y-5 > position.Y)
                        dominateKey = Keys.Down;
                }
          /*      if (this.position.X - 200 < playerPosition.X && this.position.X + 200 > playerPosition.X)
                {
                    if (playerPosition.X + 20< position.X)
                        secKey = Keys.Left;
                    else if (playerPosition.X -10> position.X)
                        secKey = Keys.Right;
                }
                */
            }
            return dominateKey != Keys.Delete;
        }
        public void movementNoColl(Keys dominateKey, Keys secondKey)
        {


            //This is moving right. We moving the according X-axis
            //and then check if we moved up and down while right is dom
            if (dominateKey == Keys.Right)
            {
                walking = true; walkDir = 1;
                position.X += leftRightSpeed;

                if (secondKey == Keys.Down)
                {
                    position.Y += upDownSpeed;


                }
                else if (secondKey == Keys.Up)
                {
                    position.Y -= upDownSpeed;


                }


            }
            //This is moving left. We moving the according X-axis
            //and then check if we moved up and down while left is dom
            if (dominateKey == Keys.Left ) 
            {
                walking = true; walkDir = 2;

                position.X -= leftRightSpeed;

                if (secondKey == Keys.Down)
                {
                    position.Y += upDownSpeed;


                }
                else if (secondKey == Keys.Up)
                {

                    position.Y -= upDownSpeed;

                }

            }
            //This is moving up. We moving the according Y-axis
            //and then check if we moved left and right while up is dom
            if (dominateKey == Keys.Up)
            {
                walking = true; walkDir = 3;


                position.Y -= upDownSpeed;

                if (secondKey == Keys.Left)
                {
                    position.X -= leftRightSpeed;

                }
                else if (secondKey == Keys.Right)
                {
                    position.X += leftRightSpeed;
                }
            }
            //This is moving down. We moving the according Y-axis
            //and then check if we moved left and right while down is dom
            if (dominateKey == Keys.Down)
            {
                walking = true; walkDir = 0;

                position.Y += upDownSpeed;

                if (secondKey == Keys.Right)
                {
                    position.X += leftRightSpeed;

                }
                else if (secondKey == Keys.Left)
                {
                    position.X -= leftRightSpeed;
                }
            }
        }


        public void movement(Keys dominateKey, Keys secondKey)
        {
            //Get the possible colliding blocks nsew
            WallTile[] possibleCollision = tilesNearPlayer(mapDesign.walls, this.footArea, this.upDownSpeed, this.leftRightSpeed);



            //This is moving right. We moving the according X-axis
            //and then check if we moved up and down while right is dom
            if (dominateKey == Keys.Right)
            {
                walking = true; walkDir = 1;
                savedPosition.X = position.X;


                //this will check if the player has collided with a wall object
                if (possibleCollision[2].isUsed)
                    position.X = savedPosition.X;
                else
                    position.X += leftRightSpeed;


                if (secondKey == Keys.Down)
                {


                    savedPosition.Y = position.Y;
                    if ((possibleCollision[1].isUsed || (!possibleCollision[2].isUsed && possibleCollision[7].isUsed)))
                        position.Y = savedPosition.Y;
                    else
                        position.Y += upDownSpeed;


                }
                else if (secondKey == Keys.Up)
                {
                    savedPosition.Y = position.Y;
                    if ((possibleCollision[0].isUsed || (!possibleCollision[2].isUsed && possibleCollision[5].isUsed)))
                        position.Y = savedPosition.Y;
                    else
                        position.Y -= upDownSpeed;


                }


            }
            //This is moving left. We moving the according X-axis
            //and then check if we moved up and down while left is dom
            if (dominateKey == Keys.Left)
            {
                walking = true; walkDir = 2;

                savedPosition.X = position.X;

                if (possibleCollision[3].isUsed)
                    position.X = savedPosition.X;
                else
                    position.X -= leftRightSpeed;

                if (secondKey == Keys.Down)
                {
                    savedPosition.Y = position.Y;

                    if ((possibleCollision[1].isUsed || (!possibleCollision[3].isUsed && possibleCollision[6].isUsed)))
                        position.Y = savedPosition.Y;
                    else
                        position.Y += upDownSpeed;


                }
                else if (secondKey == Keys.Up)
                {

                    savedPosition.Y = position.Y;
                    if ((possibleCollision[0].isUsed || (!possibleCollision[3].isUsed && possibleCollision[4].isUsed)))
                        position.Y = savedPosition.Y;
                    else
                        position.Y -= upDownSpeed;

                }

            }
            //This is moving up. We moving the according Y-axis
            //and then check if we moved left and right while up is dom
            if (dominateKey == Keys.Up)
            {
                walking = true; walkDir = 3;


                savedPosition.Y = position.Y;

                if (possibleCollision[0].isUsed)
                    position.Y = savedPosition.Y;
                else
                    position.Y -= upDownSpeed;

                if (secondKey == Keys.Left)
                {
                    savedPosition.X = position.X;

                    if ((possibleCollision[3].isUsed || (!possibleCollision[0].isUsed && possibleCollision[4].isUsed)))
                        position.X = savedPosition.X;
                    else
                        position.X -= leftRightSpeed;

                }
                else if (secondKey == Keys.Right)
                {
                    savedPosition.X = position.X;
                    if ((possibleCollision[2].isUsed || (!possibleCollision[0].isUsed && possibleCollision[5].isUsed)))
                        position.X = savedPosition.X;
                    else
                        position.X += leftRightSpeed;
                }
            }
            //This is moving down. We moving the according Y-axis
            //and then check if we moved left and right while down is dom
            if (dominateKey == Keys.Down)
            {
                walking = true; walkDir = 0;

                savedPosition.Y = position.Y;
                if (possibleCollision[1].isUsed)
                    position.Y = savedPosition.Y;
                else
                    position.Y += upDownSpeed;

                if (secondKey == Keys.Right)
                {
                    savedPosition.X = position.X;
                    if ((possibleCollision[2].isUsed || (!possibleCollision[1].isUsed && possibleCollision[7].isUsed)))
                        position.X = savedPosition.X;
                    else
                        position.X += leftRightSpeed;

                }
                else if (secondKey == Keys.Left)
                {
                    savedPosition.X = position.X;
                    if ((possibleCollision[3].isUsed || (!possibleCollision[1].isUsed && possibleCollision[6].isUsed)))
                        position.X = savedPosition.X;
                    else
                        position.X -= leftRightSpeed;
                }
            }
        }
        

        public void movementNoColl(KeyboardState newState){
            

             //This is moving right. We moving the according X-axis
            //and then check if we moved up and down while right is dom
            if (dominateKey == Keys.Right && newState.IsKeyDown(Keys.Right) && !newState.IsKeyDown(Keys.Left)){
                walking = true; walkDir = 1;
                position.X += leftRightSpeed;

                if (newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up)){
                    position.Y += upDownSpeed;
                    
                
                }else if (newState.IsKeyDown(Keys.Up) && !newState.IsKeyDown(Keys.Down)){
                       position.Y -= upDownSpeed;
                    
                
                }
             
                
            }
            //This is moving left. We moving the according X-axis
            //and then check if we moved up and down while left is dom
            if (dominateKey == Keys.Left && newState.IsKeyDown(Keys.Left) &&!newState.IsKeyDown(Keys.Right)){
                walking = true; walkDir = 2;

                    position.X -= leftRightSpeed;
                
                if (newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up)){
                     position.Y += upDownSpeed;
                    
                
                }else if (newState.IsKeyDown(Keys.Up) && !newState.IsKeyDown(Keys.Down)){
                    
                            position.Y -= upDownSpeed;
                    
                }
                
            }
            //This is moving up. We moving the according Y-axis
            //and then check if we moved left and right while up is dom
            if (dominateKey == Keys.Up && newState.IsKeyDown(Keys.Up) &&!newState.IsKeyDown(Keys.Down)){
                walking = true; walkDir = 3;

           
                        position.Y -= upDownSpeed;
                
                if (newState.IsKeyDown(Keys.Left) && !newState.IsKeyDown(Keys.Right)){
                        position.X -= leftRightSpeed;

                }else if (newState.IsKeyDown(Keys.Right) && !newState.IsKeyDown(Keys.Left)){
                        position.X += leftRightSpeed;
                }
            }
            //This is moving down. We moving the according Y-axis
            //and then check if we moved left and right while down is dom
            if (dominateKey == Keys.Down && newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up)){
                walking = true; walkDir = 0; 
              
                        position.Y += upDownSpeed;
                
                if (newState.IsKeyDown(Keys.Right) && !newState.IsKeyDown(Keys.Left)){
                        position.X += leftRightSpeed;

                }else if (newState.IsKeyDown(Keys.Left) && !newState.IsKeyDown(Keys.Right)){
                        position.X -= leftRightSpeed;
                }
             }
        }
        

        public void movement(KeyboardState newState){
            //Get the possible colliding blocks nsew
            WallTile[] possibleCollision = tilesNearPlayer(mapDesign.walls, this.footArea, this.upDownSpeed, this.leftRightSpeed);
          
          

             //This is moving right. We moving the according X-axis
            //and then check if we moved up and down while right is dom
            if (dominateKey == Keys.Right && newState.IsKeyDown(Keys.Right) && !newState.IsKeyDown(Keys.Left)){
                walking = true; walkDir = 1;
                savedPosition.X = position.X;
                

                //this will check if the player has collided with a wall object
                if (possibleCollision[2].isUsed )
                    position.X = savedPosition.X;
                else
                    position.X += leftRightSpeed;


                if (newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up)){
             

                    savedPosition.Y = position.Y;
                    if ((possibleCollision[1].isUsed  || (!possibleCollision[2].isUsed && possibleCollision[7].isUsed)) )
                        position.Y = savedPosition.Y;
                    else
                        position.Y += upDownSpeed;
                    
                
                }else if (newState.IsKeyDown(Keys.Up) && !newState.IsKeyDown(Keys.Down)){
                    savedPosition.Y = position.Y;
                    if ((possibleCollision[0].isUsed || (!possibleCollision[2].isUsed && possibleCollision[5].isUsed)) )
                       position.Y = savedPosition.Y;
                    else
                       position.Y -= upDownSpeed;
                    
                
                }
             
                
            }
            //This is moving left. We moving the according X-axis
            //and then check if we moved up and down while left is dom
            if (dominateKey == Keys.Left && newState.IsKeyDown(Keys.Left) &&!newState.IsKeyDown(Keys.Right)){
                walking = true; walkDir = 2;

                savedPosition.X = position.X;

                if (possibleCollision[3].isUsed )
                    position.X = savedPosition.X;
                else
                    position.X -= leftRightSpeed;
                
                if (newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up)){
                  savedPosition.Y = position.Y;
                    
                  if ((possibleCollision[1].isUsed  || (!possibleCollision[3].isUsed && possibleCollision[6].isUsed)))
                      position.Y = savedPosition.Y;
                  else
                     position.Y += upDownSpeed;
                    
                
                }else if (newState.IsKeyDown(Keys.Up) && !newState.IsKeyDown(Keys.Down)){
                    
                        savedPosition.Y = position.Y;
                        if ((possibleCollision[0].isUsed || (!possibleCollision[3].isUsed && possibleCollision[4].isUsed))  )
                            position.Y = savedPosition.Y;
                        else
                            position.Y -= upDownSpeed;
                    
                }
                
            }
            //This is moving up. We moving the according Y-axis
            //and then check if we moved left and right while up is dom
            if (dominateKey == Keys.Up && newState.IsKeyDown(Keys.Up) &&!newState.IsKeyDown(Keys.Down)){
                walking = true; walkDir = 3;

           
                    savedPosition.Y = position.Y;
           
                   if (possibleCollision[0].isUsed )
                        position.Y = savedPosition.Y;
                    else
                        position.Y -= upDownSpeed;
                
                if (newState.IsKeyDown(Keys.Left) && !newState.IsKeyDown(Keys.Right)){
                    savedPosition.X = position.X;

                    if ((possibleCollision[3].isUsed || (!possibleCollision[0].isUsed && possibleCollision[4].isUsed)) )
                        position.X = savedPosition.X;
                    else
                        position.X -= leftRightSpeed;

                }else if (newState.IsKeyDown(Keys.Right) && !newState.IsKeyDown(Keys.Left)){
                    savedPosition.X = position.X;
                    if ((possibleCollision[2].isUsed ||(!possibleCollision[0].isUsed && possibleCollision[5].isUsed))  )
                        position.X = savedPosition.X;
                    else
                        position.X += leftRightSpeed;
                }
            }
            //This is moving down. We moving the according Y-axis
            //and then check if we moved left and right while down is dom
            if (dominateKey == Keys.Down && newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up)){
                walking = true; walkDir = 0; 
              
                    savedPosition.Y = position.Y;
                    if (possibleCollision[1].isUsed)
                        position.Y = savedPosition.Y;
                    else
                        position.Y += upDownSpeed;
                
                if (newState.IsKeyDown(Keys.Right) && !newState.IsKeyDown(Keys.Left)){
                    savedPosition.X = position.X;
                    if ((possibleCollision[2].isUsed || (!possibleCollision[1].isUsed && possibleCollision[7].isUsed))  )
                        position.X = savedPosition.X;
                    else
                        position.X += leftRightSpeed;

                }else if (newState.IsKeyDown(Keys.Left) && !newState.IsKeyDown(Keys.Right)){
                    savedPosition.X = position.X;
                    if ((possibleCollision[3].isUsed || (!possibleCollision[1].isUsed && possibleCollision[6].isUsed))  )
                        position.X = savedPosition.X;
                    else
                        position.X -= leftRightSpeed;
                }
             }
        }
        


        public void movingAnimation(GameTime timer, int direction){
            walkTimer += (float)timer.ElapsedGameTime.Milliseconds;
            if (walkTimer > 120f)
            {
                currWalkFrame++;
                if (currWalkFrame > walkFrames - 1)
                    currWalkFrame = 0;
                walkTimer = 0;
            }
            sourceRect = new Rectangle((currWalkFrame * width) , height * direction, width, height);
       
        }

        public void draw(SpriteBatch sp,GraphicsDevice g, SpriteFont informFont){
            
            var t = new Texture2D(g, 1, 1);
            t.SetData(new[] { Color.White });
            sp.DrawString(informFont, enemyString, new Vector2(center.X + 50, center.Y - 40), Color.Red);
            sp.Draw(t, footArea, Color.White);
            sp.Draw(sprite, destRect, sourceRect, Color.White);
           
        }

        //This returns the dominate key pressed comparing it to the newly
        //pressed keys
        public Keys setDominateKey(Keys[] newKeys, Keys domKey)
        {

            if (domKey == newKeys[0] || domKey == newKeys[1])
                return domKey;

            return newKeys[0];
        }
        //This tests to see if only the arrow keys were pressed
        public bool onlyArrowsPressed(Keys k)
        {
            return !(k != Keys.Up && k != Keys.Down && k != Keys.Left && k != Keys.Right);
        }



        public WallTile[] tilesNearPlayer(List<WallTile> tiles, Rectangle playerFeet, float upDownSpeed, float leftRightSpeed)
        {
            //Array of tiles North, south, east, west in that order, then north west, north east, south west, south 
            WallTile[] nsew = {new WallTile(),new WallTile(),new WallTile(),new WallTile()
                                   ,new WallTile(),new WallTile(),new WallTile(),new WallTile()};

            Rectangle collWidth = new Rectangle((int)(playerFeet.X - leftRightSpeed), playerFeet.Y,
                                                (int)(playerFeet.Width + (leftRightSpeed * 2)), playerFeet.Height);
            Rectangle collHeight = new Rectangle(playerFeet.X, (int)(playerFeet.Y - upDownSpeed),
                                                 playerFeet.Width, (int)(playerFeet.Height + (2 * upDownSpeed)));
            Rectangle diagnolTesting = new Rectangle((int)(playerFeet.X - leftRightSpeed), (int)(playerFeet.Y - upDownSpeed),
                                                      (int)(playerFeet.Width + (leftRightSpeed * 2)), (int)(playerFeet.Height + (2 * upDownSpeed)));
            //This will set up the array and get the closest
            //tiles
            for (int i = 0; i < tiles.Count; i++)
            {
                WallTile t = tiles[i];

                //If we have collision all around us, we don't need to search anymore
                if (nsew[0].isUsed && nsew[1].isUsed && nsew[2].isUsed && nsew[3].isUsed)
                    break;

                //Check to see if there is a collision block, "Wall" on the left
                //side or the right side
                if (t.colliding(collWidth))
                {
                    //Check to see if this is in the closest distance and store properly               
                    if (t.destRect.X > playerFeet.X + (playerFeet.Width / 2))
                        nsew[2] = t;
                    if (t.destRect.X < playerFeet.X + (playerFeet.Width / 2))
                        nsew[3] = t;
                }
                //otherwise we need to see if its colliding above or belows us
                else if (t.colliding(collHeight))
                {
                    //if it does store properly once again
                    if (t.destRect.Y > playerFeet.Y + (playerFeet.Height / 2))
                        nsew[1] = t;
                    if (t.destRect.Y < playerFeet.Y + (playerFeet.Height / 2))
                        nsew[0] = t;
                }
                else if (t.colliding(diagnolTesting))
                {
                    if (t.destRect.Y > playerFeet.Y + (playerFeet.Height / 2) && t.destRect.X < playerFeet.X + (playerFeet.Width / 2))
                        nsew[7] = t;

                    if (t.destRect.Y > playerFeet.Y + (playerFeet.Height / 2) && t.destRect.X < playerFeet.X + (playerFeet.Width / 2))
                        nsew[6] = t;
                    if (t.destRect.Y < playerFeet.Y + (playerFeet.Height / 2) && t.destRect.X < playerFeet.X + (playerFeet.Width / 2))
                        nsew[5] = t;
                    if (t.destRect.Y < playerFeet.Y + (playerFeet.Height / 2) && t.destRect.X < playerFeet.X + (playerFeet.Width / 2))
                        nsew[4] = t;
                }
               
            }
            return nsew;
        }


    }
}
