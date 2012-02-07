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
    class MainPlayer
    {
        //Players Stats
        int health = 4;
        int mana = 0;
        int strength = 1;
        int intelligence = 0;
        int defense = 0;
        int currEXP = 0;
        int currLvl = 1;
        public String simpleGui;

        //Error Checking information
        public String posString;
        public String groundString;
        public String jumpingString;
        public String jumpVectorString;

        //reset to orginal position Debugging purpose
        public Vector2 resetPosition;

        //direction facing information
        int walkDir = 0;

        //Jumping information
        float jumpYPosition; //Saves Y position when jumping
        float jumpHeight = -9f;
        public float freeFallVelocity = 11f;


        //boolean actions
        public bool jumping;
        public bool freeFalling = true;
        bool touchingGround;
        bool jumpFalling = false;
        bool windBlowing = true;
        bool walking;

        //Animation times and frame counters
        float windTimer = 0f;
        float walkTimer = 0f;
        float jumpTimer = 0f;
        float blinkingTimer = 0f;
        int currJumpFrame = 0;
        int currWindFrame = 0;
        int currWalkFrame = 0;
        int windFrames = 3;
        int jumpFrames = 5;
        int walkFrames = 4;

        //The sprite sheet for Will (Main Character)
        public Texture2D spriteSheet;

        //Sprite information
        public Vector2 position;
        public Vector2 center;
        public Vector2 savedPosition;

        //keyboard state from previous update
        KeyboardState oldState = Keyboard.GetState();

        //used for the dominate key
        Keys dominateKey = Keys.End;

        //Collision Rectangles
        public Rectangle collisionRect;
        public Rectangle footArea;

        //this is used for position the player on the screen
        //and the information on spritesheet
        public Rectangle sourceRect;
        public Rectangle destRect;

        //players,width and height
        public int width;
        public int height;

        //movement speed
        public float leftRightSpeed = 4f;
        public float upDownSpeed = 3f;

        //MapReader
        MapReader mapDesign;

        public MainPlayer(Texture2D sprite, Vector2 position)
        {
            //set everything up
            this.spriteSheet = sprite;
            this.position = position;
            this.resetPosition = position;

            //This shows where the rectangle is drawn (width = 64 height = 84)
            //twice times normal sprite size
            destRect = new Rectangle((int)position.X, (int)position.Y, 32 * 2, 42 * 2);

            //this is the rectangle to get info off the sprite sheet to show
            sourceRect = new Rectangle(0, 0, 32 * 2, 42 * 2);

            width = destRect.Width;
            height = destRect.Height;

            //Center of the sprite
            center = new Vector2(destRect.X + 32, destRect.Y + 42);

            //rectangle for collision
            collisionRect = new Rectangle((int)position.X, (int)position.Y, width, height);

        }

        public void update(KeyboardState newState, GameTime time) {

            simpleGui = "Health: " + health + "\nMana: " + mana + "\nStr: " + strength
                                + "\nDef: " + defense + "\nInt: " + intelligence + "\nLVL: " + currLvl + "\nExp: " + currEXP;

            //beginning walking starts off
            walking = false;
            freeFalling = checkFreeFalling();

            
            
            //Error checking for the sprite
            posString = "Pos:(" + destRect.X + "," + destRect.Y + ") Sav:(" + savedPosition.X + "," + savedPosition.Y + ")";
            groundString = "On ground: " + touchingGround;
            jumpingString = "Jumping: " + jumping + "  JumpHeight: " + jumpHeight;
            jumpVectorString = "Position: " + jumpYPosition + "\nJumpFrame:" + currJumpFrame;


            //Get the dominate like if left is pressed first left is dominate
            //if there are 2 keys and both are different than before then first
            //in the array is the dominate key
            if(newState.GetPressedKeys().Length == 1 && onlyArrowsPressed(newState.GetPressedKeys()[0])){
                dominateKey = newState.GetPressedKeys()[0];
            }else if(newState.GetPressedKeys().Length > 1 &&
                 ( onlyArrowsPressed(newState.GetPressedKeys()[0]) ||
                  onlyArrowsPressed(newState.GetPressedKeys()[1]))){
               dominateKey = setDominateKey(newState.GetPressedKeys(),dominateKey);
            }

            //inform of keys pressed
            jumpVectorString += " Dominate: " + dominateKey;
           

            //Now we need move our character upon input
            spriteMovement(newState);



            //Debug button in case we move too far away from the 
            //map or we begin to glitch
            if (newState.IsKeyDown(Keys.R) && oldState.IsKeyUp(Keys.R)){
                position = resetPosition;
                jumpYPosition = position.Y;
            }

            //Button to turn the wind off
            if (newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
                if (windBlowing)
                    windBlowing = false;
                else
                    windBlowing = true;

            //load next map
            if (newState.IsKeyDown(Keys.T) && oldState.IsKeyUp(Keys.T))
                if (mapDesign.currentLvl == 0)
                    mapDesign.loadNext(1);
                else
                    mapDesign.loadNext(0);

            //Now to add the walking animations
            if (walking && !jumping){
                walkingAnimation(time, walkDir);

            //Else the sprite is doing nothing...check what position they are facing
            //and make corresponding windAnimation;
            }else if (!jumping && !walking && windBlowing){
                windAnimation(time, walkDir);
           
            //reset standing position
            }else{
                currWalkFrame = 0;
                sourceRect = new Rectangle(0, 42 * 2 * walkDir, 32 * 2, 42 * 2);
                
            }
              
            //Now for the jumping motion and calculation
            //NEED TO ADD -animations
            if (jumping)
            {
                jumpHeight += .6f;
                position.Y += jumpHeight;
                jumpingAnimation(time, walkDir);
                if (jumpHeight >= 0)
                    jumpFalling = true;
                else
                    jumpFalling = false;
                //time to check if we are jump falling and if we are
                //check to see if we are on the ground
                if (jumpFalling)
                    touchingGround = onGround(jumpYPosition);
            }
            else
                currJumpFrame = 0;

            //time to the freefalling calculation
            //this is when we are falling and not falling from
            //a jump
            if(freeFalling && !jumping)
                position.Y += freeFallVelocity;

            //This tests to see if you pressed the space to jump that is if you aren't already jumping
            if (newState.IsKeyDown(Keys.Z) && oldState.IsKeyUp(Keys.Z))
                if (!jumping){
                    jumping = true;
                    touchingGround = false;
                    jumpYPosition = position.Y;
                    jumpHeight = -9;
                  }

          

            //update the sprites position on the map.
            destRect.X = (int)position.X;
            destRect.Y = (int)position.Y;
            center = new Vector2(destRect.X + 32, destRect.Y + 42);
            oldState = newState;
            collisionRect = new Rectangle((int)position.X, (int)position.Y, width, height);
            footArea = new Rectangle((int)center.X-14, (int)center.Y+27 , 26, 14);
        }


        //this will check if the player is not on a walkable area and 
        //is not jumping. 
        public bool checkFreeFalling(){
            for(int i = 0; i < mapDesign.walkable.Count; i++){
                if (mapDesign.walkable[i].onThis(this))
                    return false;
            }
            return true;
        }

        //This plays the wind animation
        public void windAnimation(GameTime timer, int direction)
        {

            //add the current elapsed time to the animation
            windTimer += (float)timer.ElapsedGameTime.Milliseconds;
            
            //this is how frequient in milliseconds we will update the animation
            if (windTimer > 120f){
                currWindFrame++;
                if (currWindFrame > windFrames - 1)
                    currWindFrame = 0;
                windTimer = 0;
            }
            //now to simulate the animation to the according position on the spritesheet
            sourceRect = new Rectangle(currWindFrame * 32 * 2, 42 * 2 * direction, 32 * 2, 42 * 2);
        }


        //This iss to play the jumping animation
        public void jumpingAnimation(GameTime timer, int direction){
           jumpTimer += (float)timer.ElapsedGameTime.Milliseconds;
           if (jumpTimer > 60f){
               currJumpFrame++;

               if (this.jumpYPosition - this.position.Y < 40){
                   if (this.jumpYPosition - this.position.Y  > 28)
                       currJumpFrame = 1;
                   else
                       currJumpFrame = 0;
               
               }else if (currJumpFrame > jumpFrames - 1)
               {
                      currJumpFrame = 2;
               }
               jumpTimer = 0;
            }
           sourceRect = new Rectangle((currJumpFrame * 32 * 2) + 448, 42 * 2 * direction, 32 * 2, 42 * 2);
        
        }

     


        //This is for walkingAnimation
        //Same stuff as wind animation
        public void walkingAnimation(GameTime timer, int direction){
            walkTimer += (float)timer.ElapsedGameTime.Milliseconds;
            if (walkTimer > 120f){
                currWalkFrame++;
                if (currWalkFrame > walkFrames - 1)
                    currWalkFrame = 0;
                walkTimer = 0;
            }
            sourceRect = new Rectangle((currWalkFrame * 32 * 2) + 192, 42 * 2 * direction, 32 * 2, 42 * 2);
        }
        public void blinking(GameTime time,int multiplier, int direction){
            if (direction != 3){
                blinkingTimer += time.ElapsedGameTime.Milliseconds;
                if (blinkingTimer > 10f){
                    sourceRect = new Rectangle((direction * 32 * 2), 42 * 2 * 4, 32 * 2, 42 * 2);
                    blinkingTimer = 0;
                }
            }


        }

        //This test if we are falling or jumping that we touch the same spot
        //as we jumped from.
        public bool onGround(float jumpYPosition){
            if (jumpYPosition < destRect.Y){
                position.Y = jumpYPosition;
                destRect.Y = (int)position.Y;
                jumping = false;
                return true;
            }
            return false;
        }

        //We check what the dominate key is and move according to it. 
        //this is where we physically begin to move our sprite to other
        //locations on the screen depending on the input.
        public void spriteMovement(KeyboardState newState){
            

            //Get the possible colliding blocks nsew
            WallTile[] possibleCollision = tilesNearPlayer(mapDesign.walls, this.footArea, this.upDownSpeed, this.leftRightSpeed);
          
          

            //This is moving right. We moving the according X-axis
            //and then check if we moved up and down while right is dom
            if (dominateKey == Keys.Right && newState.IsKeyDown(Keys.Right) && !newState.IsKeyDown(Keys.Left)){
                walking = true; walkDir = 1;
                savedPosition.X = position.X;
                

                //this will check if the player has collided with a wall object
                if (possibleCollision[2].isUsed && !jumping)
                    position.X = savedPosition.X;
                else
                    position.X += leftRightSpeed;


                if (newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up)){
                    if (jumping){
                        jumpYPosition += .6f;
                        jumpHeight += .1f;
                    }else{

                        savedPosition.Y = position.Y;
                        if (possibleCollision[1].isUsed && !jumping)
                            position.Y = savedPosition.Y;
                        else
                            position.Y += upDownSpeed;
                    }
                
                }else if (newState.IsKeyDown(Keys.Up) && !newState.IsKeyDown(Keys.Down)){
                    if (jumping){
                        jumpYPosition -= 2f;
                        jumpHeight -= .1f;
                    }else{

                        savedPosition.Y = position.Y;
                        if (possibleCollision[0].isUsed && !jumping)
                            position.Y = savedPosition.Y;
                        else
                            position.Y -= upDownSpeed;
                    }
                
                }
            }
            //This is moving left. We moving the according X-axis
            //and then check if we moved up and down while left is dom
            if (dominateKey == Keys.Left && newState.IsKeyDown(Keys.Left) &&!newState.IsKeyDown(Keys.Right)){
                walking = true; walkDir = 2;

                savedPosition.X = position.X;

                if (possibleCollision[3].isUsed && !jumping)
                    position.X = savedPosition.X;
                else
                    position.X -= leftRightSpeed;
                
                if (newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up)){
                    if (jumping){
                        jumpYPosition += .6f;
                        jumpHeight += .1f;
                    }else{
                        savedPosition.Y = position.Y;
                    
                        if (possibleCollision[1].isUsed && !jumping)
                            position.Y = savedPosition.Y;
                        else
                            position.Y += upDownSpeed;
                    }
                
                }else if (newState.IsKeyDown(Keys.Up) && !newState.IsKeyDown(Keys.Down)){
                    if (jumping){
                        jumpYPosition -= 2f;
                        jumpHeight -= .1f;
                    }else{
                        savedPosition.Y = position.Y;
                        if (possibleCollision[0].isUsed && !jumping)
                            position.Y = savedPosition.Y;
                        else
                            position.Y -= upDownSpeed;
                    }
                }
            }
            //This is moving up. We moving the according Y-axis
            //and then check if we moved left and right while up is dom
            if (dominateKey == Keys.Up && newState.IsKeyDown(Keys.Up) &&!newState.IsKeyDown(Keys.Down)){
                walking = true; walkDir = 3;

                if (jumping){
                    jumpYPosition -= 2f;
                    jumpHeight -= .1f;
                   
                }else{
                    savedPosition.Y = position.Y;
                    if (possibleCollision[0].isUsed && !jumping)
                        position.Y = savedPosition.Y;
                    else
                        position.Y -= upDownSpeed;
                }
                if (newState.IsKeyDown(Keys.Left) && !newState.IsKeyDown(Keys.Right)){
                    savedPosition.X = position.X;

                    if (possibleCollision[3].isUsed && !jumping)
                        position.X = savedPosition.X;
                    else
                        position.X -= leftRightSpeed;
                }else if (newState.IsKeyDown(Keys.Right) && !newState.IsKeyDown(Keys.Left)){
                    savedPosition.X = position.X;
                    if (possibleCollision[2].isUsed && !jumping)
                        position.X = savedPosition.X;
                    else
                        position.X += leftRightSpeed;
                }
            }
            //This is moving down. We moving the according Y-axis
            //and then check if we moved left and right while down is dom
            if (dominateKey == Keys.Down && newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up)){
                walking = true; walkDir = 0; 
                if (jumping){
                    jumpYPosition += .6f;
                    jumpHeight += .1f;
                }else{
                    savedPosition.Y = position.Y;
                    if (possibleCollision[1].isUsed)
                        position.Y = savedPosition.Y;
                    else
                        position.Y += upDownSpeed;
                }
                if (newState.IsKeyDown(Keys.Right) && !newState.IsKeyDown(Keys.Left)){
                    savedPosition.X = position.X;
                    if (possibleCollision[2].isUsed && !jumping)
                        position.X = savedPosition.X;
                    else
                        position.X += leftRightSpeed;

                }else if (newState.IsKeyDown(Keys.Left) && !newState.IsKeyDown(Keys.Right)){
                    savedPosition.X = position.X;
                    if (possibleCollision[3].isUsed)
                        position.X = savedPosition.X;
                    else
                        position.X -= leftRightSpeed;
                }
            }  
        }
        //This returns the dominate key pressed comparing it to the newly
        //pressed keys
        public Keys setDominateKey(Keys[] newKeys, Keys domKey){
            
            if (domKey == newKeys[0] || domKey == newKeys[1])
                return domKey;
       
            return newKeys[0];
        }
        //This tests to see if only the arrow keys were pressed
        public bool onlyArrowsPressed(Keys k){
            return !(k != Keys.Up && k != Keys.Down && k != Keys.Left && k != Keys.Right);
        }

        //Connect the map to the player for collision detections
        public void connectMap(MapReader mapDesign){
            this.mapDesign = mapDesign;
        }

        //This will obtain the tils that are around the player and according to that it will
        //return the tiles north, south, east, and west tiles. If the item is null, then 
        //there is no wall tile there
        public WallTile[] tilesNearPlayer(List<WallTile> tiles, Rectangle playerFeet, float upDownSpeed, float leftRightSpeed)
        {
            //Array of tiles North, south, east, west in that order
            
            WallTile[] nsew = { new WallTile(), new WallTile(), new WallTile(), new WallTile() };

            Rectangle collWidth = new Rectangle((int)(playerFeet.X - leftRightSpeed),playerFeet.Y,
                                                (int)(playerFeet.Width + (leftRightSpeed * 2)),playerFeet.Height);
            Rectangle collHeight = new Rectangle(playerFeet.X, (int)(playerFeet.Y - upDownSpeed),
                                                 playerFeet.Width, (int)(playerFeet.Height + (2 * upDownSpeed)));
            //This will set up the array and get the closest
            //tiles
            for (int i = 0; i < tiles.Count; i++)
            {
                WallTile t = tiles[i];
               
                //If we have collision all around us, we don't need to search anymore
                if (nsew[0].isUsed && nsew[1].isUsed && nsew[2].isUsed&& nsew[3].isUsed)
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
            }



            //DEBUG purpose
            jumpVectorString += "\n" + nsew[0].isUsed + " " + nsew[1].isUsed + " " +
                                       nsew[2].isUsed + " " + nsew[3].isUsed;

            //Returns the array with all the collisions around us, besides diagonal
            return nsew;
        }

    }
}
