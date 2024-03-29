﻿using System;
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
    class MainPlayer : MainPlayerData
    {
        //Players Stats
        public int maxHealth = 6;
        public int health = 4;
        public int maxMana = 3;
        public int mana = 2;
        public int strength = 1;
        public int intelligence = 0;
        public int defense = 0;
        public int currEXP = 0;
        public int currLvl = 1;
        

        //Error Checking information
        public String posString = "";
        public String groundString = "";
        public String jumpingString = "";
        public String jumpVectorString = "";

        //reset to orginal position Debugging purpose
        public Vector2 resetPosition;

        //direction facing information
        //Down= 0 Right= 1 Left= 2 Up= 3
        public int walkDir = 0;

        //Jumping information
        float jumpYPosition; //Saves Y position when jumping
        float jumpHeight = -9f;
        public float freeFallVelocity = 11f;


        //boolean actions
        public bool jumping;
        public int climbing = -1;
        public bool freeFalling = true;
        public bool hit = false;
        bool dead = false;
        bool done = false;
        bool touchingGround;
        bool jumpFalling = false;
        bool windBlowing = true;
        bool walking;
        bool changed = false;
        public bool talking = false;
        public bool attacking = false;
        

        //Animation times and frame counters
        float windTimer = 0f;
        float walkTimer = 0f;
        float jumpTimer = 0f;
        float climbTimer = 0f;
        float blinkingTimer = 0f;
        float attackingTimer = 0f;
        float hitTimer = 0f;
        float levelChangeTimer = 0f;
        int currAttackingFrame = 0;
        int currJumpFrame = 0;
        int currWindFrame = 0;
        int currWalkFrame = 0;
        int currClimbFrame = 0;
        

        //This holds the frames and the location of the data
        //in for the sprite sheets
        int [] playerData;
        int [] playerAttackingData;

        //The sprite sheet for Will (Main Character)
        public Texture2D spriteSheet;
        public Texture2D attackingSheet;

        //Sprite information
        public Vector2 position;
        public Vector2 center;
        public Vector2 savedPosition;

        //keyboard state from previous update
        KeyboardState oldState = Keyboard.GetState();

        //used for the dominate key
        Keys dominateKey = Keys.End;

        //Collision Rectangles

        public Rectangle footArea;

        //this is used for position the player on the screen
        //and the information on spritesheet
        public Rectangle sourceRect;
        public Rectangle destRect;

        //players,width and height
        public int width;
        public int height;
        public int attackingWidth;
        public int attackingHeight;
        int backX = 0;
        int backY = 0;

        //movement speed
        public float leftRightSpeed = 4f;
        public float upDownSpeed = 3f;

        //MapReader
        MapReader mapDesign;
        
        //This is for the main random generator
        Random rand;

        //The player's ID so if you are a character
        //it is different than the otherone in terms
        //of sprite sheet and data
        public int playerID;

        //Sounds Player
        SoundPlayerGaia soundsPlayer;

        //Collisions that is used for detecting player location
        CollisionFunctions cf = new CollisionFunctions();

        //Hold on to map before transition
        NewMapTile nextMap;

        public MainPlayer(Texture2D will, Texture2D willAttacking,Texture2D freedan, Texture2D freedanAttacking, Vector2 position, int width, int height)
        {
            //Will width, 64,84
            //Will Attacking 140,140
            //Freedan, 120,120
            //Freedan Attacking unknown
            //set everything up
            playerID = 1;
            this.spriteSheet = will;
            this.position = position;
            this.resetPosition = position;
            this.width = width;
            this.height = height;
            this.attackingWidth = 140;
            this.attackingHeight = 140;
            this.playerData = willData;
            this.playerAttackingData = willAttackingData;
            this.attackingSheet = willAttacking;
            this.setTextures(will,willAttacking, freedan,freedanAttacking);

            //This shows where the rectangle is drawn (width = 64 height = 84)
            //twice times normal sprite size
            destRect = new Rectangle((int)position.X, (int)position.Y, width, height);

            //this is the rectangle to get info off the sprite sheet to show
            sourceRect = new Rectangle(0, 0, width, height);

            //seeting the width and the height of the sprite
            width = destRect.Width;
            height = destRect.Height;

            //Center of the sprite
            center = new Vector2(destRect.X + (width/2), destRect.Y + (height/2));



        }

        public void update(KeyboardState newState, GameTime time)
        {

          
            //beginning walking starts off
            walking = false;
            //we will check if we are falling or climbing
            freeFalling = checkFreeFalling();
            climbing = checkClimbing(mapDesign.ladders);

            //Error checking for the sprite
            posString = "Pos:(" + destRect.X + "," + destRect.Y + ") Sav:(" + savedPosition.X + "," + savedPosition.Y + ")";
            groundString = "On ground: " + touchingGround;
            jumpingString = "Jumping: " + jumping + "  JumpHeight: " + jumpHeight;
            jumpVectorString = "Position: " + jumpYPosition + "\nJumpFrame:" + currJumpFrame + "\nClimbing:" + climbing
                               + "\nFeetWidth:" + footArea.Width;
            if (mapDesign.nextMap.Count > 0)
            {
                NewMapTile cd = mapDesign.nextMap[0];
                jumpVectorString += "\nUDLR " + cd.getNextLvlPos() + "\n";
            }



            //Debug button in case we move too far away from the 
            //map or we begin to glitch
            if (newState.IsKeyDown(Keys.R) && oldState.IsKeyUp(Keys.R))
            {
                position = resetPosition;
                jumpYPosition = (int)position.Y;
            }

            //Button to turn the wind off
            if (newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
                if (windBlowing)
                    windBlowing = false;
                else
                    windBlowing = true;



            if (freeFalling && !jumping && climbing < 0)
            {
                fallingAnimation(time, walkDir);
                position.Y += freeFallVelocity;
                jumpVectorString += "\nFALLIIIINNNGGG";
                destRect.X = (int)position.X;
                destRect.Y = (int)position.Y;

                center = new Vector2(destRect.X + (width / 2), destRect.Y + (height / 2));
                oldState = newState;

                footArea = new Rectangle((int)center.X - 14, (int)(position.Y + height) - 14, 26, 14);
                return;

            }
           

            
           
            //check to see if we stepped inside the levelchange area. 
            if(levelChange()){
                changeLevel(time);
                center = new Vector2(destRect.X + (width / 2), destRect.Y + (height / 2));
                oldState = newState;
                position.X = destRect.X;
                position.Y = destRect.Y;
                footArea = new Rectangle((int)center.X - 14, (int)(position.Y + height) - 14, 26, 14);
                return;
            }

            if (talking){
                return;
            }
            //Get the dominate like if left is pressed first left is dominate
            //if there are 2 keys and both are different than before then first
            //in the array is the dominate key
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

            //inform of keys pressed
            jumpVectorString += " Dominate: " + dominateKey;

            if (health > 0)
            {
                done = false;
                dead = false;
            }
            else
            {
                health = 0;
                dead = true;
            }

            if (done)
                return;

            if (dead){
                done = playDeath(time);
                return;
            }



            
            
            if (newState.IsKeyDown(Keys.X) && oldState.IsKeyUp(Keys.X)){
                attacking = true;
            }



            //This plays if the player is hit and shows animation
            if (hit){
                hitTimer += time.ElapsedGameTime.Milliseconds;

                bounceBack(time); 
                if (hitTimer > 1000)
                { 
                    hit = false;
                    hitTimer = 0;
                }
               
                if (backX > 0 || backY > 0)
                {
                    spriteMovement(new KeyboardState());
                    oldState = newState;
                    destRect.X = (int)position.X;
                    destRect.Y = (int)position.Y;
                    center = new Vector2(destRect.X + (width / 2), destRect.Y + (height / 2));
                    footArea = new Rectangle((int)center.X - 14, (int)(position.Y + height) - 14, 26, 14);
                    return;

                }
            }

            //If the player is talking to an enemy don't animate
            if (talking){
                return;

                //we are attacking so we need to change things appropriatly
            }else if (attacking){
                                
                //we will update the action depending on the direction and do collisions with the enemy
                attackUpdate(time, walkDir);
                oldState = newState;


              
                return;
            }
            //if we are climbing and no jumping we will only be able to move up or down
            //and through this we will play the correct climbing animation
            else if (climbing > -1 && !jumping)
            {
   
                position.X = (position.X + (center.X - climbing -16));
                
                if (newState.IsKeyDown(Keys.Up) && !newState.IsKeyDown(Keys.Down)) {
                    position.Y -= upDownSpeed;
                    climbingAnimation(time);
                }else if (newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up)){
                    position.Y += upDownSpeed;
                    climbingAnimation(time);
                }

            }
            else
            {

                currClimbFrame = 0;

                //Now we need move our character upon input
                spriteMovement(newState);
                //Now to add the walking animations
                if (walking && !jumping)
                {
                    walkingAnimation(time, walkDir);

                    //Else the sprite is doing nothing...check what position they are facing
                    //and make corresponding windAnimation;
                }
                else if (!jumping && !walking && windBlowing)
                {
                    windAnimation(time, walkDir);

                    //reset standing position
                }
                else
                {
                    currWalkFrame = 0;
                    sourceRect = new Rectangle(0, height * walkDir, width, height);

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
                    //check to see if we are on the ground and apply smooth landing
                    if (jumpFalling)
                        touchingGround = onGround(jumpYPosition, position.Y);

                }     
                else
                    currJumpFrame = 0;



                //This tests to see if you pressed the space to jump that is if you aren't already jumping
                if (newState.IsKeyDown(Keys.Z) && oldState.IsKeyUp(Keys.Z))
                    if (!jumping)
                    {
                        jumping = true;
                        touchingGround = false;
                        jumpYPosition = position.Y;
                        jumpHeight = -9;

                    }

            }

            //update the sprites position on the map.
            destRect.X = (int)position.X;
            destRect.Y = (int)position.Y;
            center = new Vector2(destRect.X + (width/2), destRect.Y + (height/2));
            oldState = newState;

            footArea = new Rectangle((int)center.X - 14, (int)(position.Y+ height)- 14, 26, 14);
        }


        //this will check if the player is not on a walkable area and 
        //is not jumping. 
        public bool checkFreeFalling()
        {
            bool falling = true;
            for (int i = 0; i < mapDesign.walkable.Count; i++)
            {
               if (mapDesign.walkable[i].onThis(this))
                    falling = false;
            }
            for (int i = 0; i < mapDesign.walls.Count; i++)
            {
                if (mapDesign.walls[i].destRect.Intersects(footArea))
                    return true;
            }

            return falling;
        }

        //This plays the wind animation
        public void windAnimation(GameTime timer, int direction)
        {

            //add the current elapsed time to the animation
            windTimer += (float)timer.ElapsedGameTime.Milliseconds;

            //this is how frequient in milliseconds we will update the animation
            if (windTimer > 120f)
            {
                currWindFrame++;
                if (currWindFrame > playerData[0] - 1)
                    currWindFrame = 0;
                windTimer = 0;
            }
            //now to simulate the animation to the according position on the spritesheet
            sourceRect = new Rectangle(currWindFrame * width, height* direction, width, height);
        }

        //Falling animation
        public void fallingAnimation(GameTime timer, int direction){
            if (currJumpFrame == 0 || currJumpFrame == 1 || currJumpFrame == 2)
                currJumpFrame = 2;
            jumpTimer += (float)timer.ElapsedGameTime.Milliseconds;
            if (jumpTimer > 60f)
            {
                currJumpFrame++;
                if (currJumpFrame > 4)
                    currJumpFrame = 3;
                jumpTimer = 0f;
            }
            sourceRect = new Rectangle((currJumpFrame * width) + (playerData[4] * width), height * direction, width, height);

        }


        //This iss to play the jumping animation
        public void jumpingAnimation(GameTime timer, int direction)
        {
            jumpTimer += (float)timer.ElapsedGameTime.Milliseconds;
            if (jumpTimer > 60f)
            {
                currJumpFrame++;

                if (this.jumpYPosition - this.position.Y < 40 && this.jumpYPosition - this.position.Y > 0)
                {
                    if (this.jumpYPosition - this.position.Y > 28)
                        currJumpFrame = 1;
                    else
                        currJumpFrame = 0;

                }
                else if (currJumpFrame > playerData[3] - 1)
                {
                    currJumpFrame = 2;
                }
             
                jumpTimer = 0;
            }
            sourceRect = new Rectangle((currJumpFrame * width) + (playerData[4] * width), height * direction, width, height);

        }

        public void climbingAnimation(GameTime timer)
        {

            climbTimer += (float)timer.ElapsedGameTime.Milliseconds;
            if (climbTimer > 130f)
            {
                currClimbFrame++;
                if (currClimbFrame > playerData[9] - 1)
                    currClimbFrame = 0;
                climbTimer = 0f;
            }
            // we will go across the sprite sheet x to the climbing start and *4 in y to indicate the position downward
            sourceRect = new Rectangle((width *  currClimbFrame) + (width * playerData[10]), height * playerData[11], width, height);


        }

        //This is for walkingAnimation
        //Same stuff as wind animation
        public void walkingAnimation(GameTime timer, int direction)
        {
            walkTimer += (float)timer.ElapsedGameTime.Milliseconds;
            if (walkTimer > 120f)
            {
                currWalkFrame++;
                if (currWalkFrame > playerData[6] - 1)
                    currWalkFrame = 0;
                walkTimer = 0;
            }
            sourceRect = new Rectangle((currWalkFrame * width) + (width * playerData[7]), height * direction, width, height);
        }

        //blink animation but not understandable how it works yet. 
        public void blinking(GameTime time, int multiplier, int direction)
        {
            if (direction != 3)
            {
                blinkingTimer += time.ElapsedGameTime.Milliseconds;
                if (blinkingTimer > 10f)
                {
                    sourceRect = new Rectangle((direction * 32 * 2), 42 * 2 * 4, 32 * 2, 42 * 2);
                    blinkingTimer = 0;
                }
            }


        }

        //This test if we are falling or jumping that we touch the same spot
        //as we jumped from.
        public bool onGround(float jumpYPosition, float newPosition)
        {

            if (jumpYPosition <= newPosition)
            {
                position.Y = jumpYPosition;
                jumping = false;
                return true;
            }
            return false;
        }

        //We check what the dominate key is and move according to it. 
        //this is where we physically begin to move our sprite to other
        //locations on the screen depending on the input.
        public void spriteMovement(KeyboardState newState)
        {


            //Get the possible colliding blocks nsew
            WallTile[] possibleCollision = tilesNearPlayer(mapDesign.walls, this.footArea, this.upDownSpeed, this.leftRightSpeed);



            //This is moving right. We moving the according X-axis
            //and then check if we moved up and down while right is dom
            if (dominateKey == Keys.Right && newState.IsKeyDown(Keys.Right) && !newState.IsKeyDown(Keys.Left))
            {
                walking = true; walkDir = 1;
                savedPosition.X = position.X;


                //this will check if the player has collided with a wall object
                if (possibleCollision[2].isUsed && !jumping)
                    position.X = savedPosition.X;
                else
                    position.X += leftRightSpeed;


                if (newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up))
                {
                    if (jumping)
                    {
                        jumpYPosition += .6f;
                        jumpHeight += .1f;
                    }
                    else
                    {

                        savedPosition.Y = position.Y;
                        if ((possibleCollision[1].isUsed || (!possibleCollision[2].isUsed && possibleCollision[7].isUsed)) && !jumping)
                            position.Y = savedPosition.Y;
                        else
                            position.Y += upDownSpeed;
                    }

                }
                else if (newState.IsKeyDown(Keys.Up) && !newState.IsKeyDown(Keys.Down))
                {
                    if (jumping)
                    {
                        jumpYPosition -= 2f;
                        jumpHeight -= .1f;
                    }
                    else
                    {

                        savedPosition.Y = position.Y;
                        if ((possibleCollision[0].isUsed || (!possibleCollision[2].isUsed && possibleCollision[5].isUsed)) && !jumping)
                            position.Y = savedPosition.Y;
                        else
                            position.Y -= upDownSpeed;
                    }

                }


            }
            //This is moving left. We moving the according X-axis
            //and then check if we moved up and down while left is dom
            if (dominateKey == Keys.Left && newState.IsKeyDown(Keys.Left) && !newState.IsKeyDown(Keys.Right))
            {
                walking = true; walkDir = 2;

                savedPosition.X = position.X;

                if (possibleCollision[3].isUsed && !jumping)
                    position.X = savedPosition.X;
                else
                    position.X -= leftRightSpeed;

                if (newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up))
                {
                    if (jumping)
                    {
                        jumpYPosition += .6f;
                        jumpHeight += .1f;
                    }
                    else
                    {
                        savedPosition.Y = position.Y;

                        if ((possibleCollision[1].isUsed || (!possibleCollision[3].isUsed && possibleCollision[6].isUsed)) && !jumping)
                            position.Y = savedPosition.Y;
                        else
                            position.Y += upDownSpeed;
                    }

                }
                else if (newState.IsKeyDown(Keys.Up) && !newState.IsKeyDown(Keys.Down))
                {
                    if (jumping)
                    {
                        jumpYPosition -= 2f;
                        jumpHeight -= .1f;
                    }
                    else
                    {
                        savedPosition.Y = position.Y;
                        if ((possibleCollision[0].isUsed || (!possibleCollision[3].isUsed && possibleCollision[4].isUsed)) && !jumping)
                            position.Y = savedPosition.Y;
                        else
                            position.Y -= upDownSpeed;
                    }
                }

            }
            //This is moving up. We moving the according Y-axis
            //and then check if we moved left and right while up is dom
            if (dominateKey == Keys.Up && newState.IsKeyDown(Keys.Up) && !newState.IsKeyDown(Keys.Down))
            {
                walking = true; walkDir = 3;

                if (jumping)
                {
                    jumpYPosition -= 2f;
                    jumpHeight -= .1f;

                }
                else
                {
                    savedPosition.Y = position.Y;
                    if (possibleCollision[0].isUsed && !jumping)
                        position.Y = savedPosition.Y;
                    else
                        position.Y -= upDownSpeed;
                }
                if (newState.IsKeyDown(Keys.Left) && !newState.IsKeyDown(Keys.Right))
                {
                    savedPosition.X = position.X;

                    if ((possibleCollision[3].isUsed || (!possibleCollision[0].isUsed && possibleCollision[4].isUsed)) && !jumping)
                        position.X = savedPosition.X;
                    else
                        position.X -= leftRightSpeed;

                }
                else if (newState.IsKeyDown(Keys.Right) && !newState.IsKeyDown(Keys.Left))
                {
                    savedPosition.X = position.X;
                    if ((possibleCollision[2].isUsed || (!possibleCollision[0].isUsed && possibleCollision[5].isUsed)) && !jumping)
                        position.X = savedPosition.X;
                    else
                        position.X += leftRightSpeed;
                }
            }
            //This is moving down. We moving the according Y-axis
            //and then check if we moved left and right while down is dom
            if (dominateKey == Keys.Down && newState.IsKeyDown(Keys.Down) && !newState.IsKeyDown(Keys.Up))
            {
                walking = true; walkDir = 0;
                if (jumping)
                {
                    jumpYPosition += .6f;
                    jumpHeight += .1f;
                }
                else
                {
                    savedPosition.Y = position.Y;
                    if (possibleCollision[1].isUsed)
                        position.Y = savedPosition.Y;
                    else
                        position.Y += upDownSpeed;
                }
                if (newState.IsKeyDown(Keys.Right) && !newState.IsKeyDown(Keys.Left))
                {
                    savedPosition.X = position.X;
                    if ((possibleCollision[2].isUsed || (!possibleCollision[1].isUsed && possibleCollision[7].isUsed)) && !jumping)
                        position.X = savedPosition.X;
                    else
                        position.X += leftRightSpeed;

                }
                else if (newState.IsKeyDown(Keys.Left) && !newState.IsKeyDown(Keys.Right))
                {
                    savedPosition.X = position.X;
                    if ((possibleCollision[3].isUsed || (!possibleCollision[1].isUsed && possibleCollision[6].isUsed)) && !jumping)
                        position.X = savedPosition.X;
                    else
                        position.X -= leftRightSpeed;
                }
            }
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

        //Connect the map to the player for collision detections
        public void connectMap(MapReader mapDesign, Random rand,SoundPlayerGaia soundPlayer)
        {
            this.rand = rand;
            this.mapDesign = mapDesign;
            this.soundsPlayer = soundPlayer;
        }

        //This will obtain the tils that are around the player and according to that it will
        //return the tiles north, south, east, and west tiles. If the item is null, then 
        //there is no wall tile there
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


            

            //Returns the array with all the collisions around us, besides diagonal
            return nsew;
        }

        public int checkClimbing(List<LadderTile> tiles)
        {
            for (int i = 0; i < tiles.Count; i++)
            {

                if (tiles[i].onLadder(footArea))
                {

                    return (int)tiles[i].position.X;

                }
            }
            return -1;
        }

        //1 - Will
        //2 - Freedan
        //3 - Shadow
        //4 - Nova
        //5 - Devon
        public void loadNewPlayer(int playerID){
            playerData = loadPlayer(playerID);
            spriteSheet = loadSheet(playerID);
            attackingSheet = loadAttackingSheet(playerID);
            playerAttackingData = loadAttackingData(playerID);

            //Save what we are at in the postion on the sprite sheet
            int xSource = sourceRect.X / width;
            int ySource = sourceRect.Y / height;
            if (playerID == 2){
                width = 120;
                height = 120;

            }else{
                width = 64;
                height = 84;
                attackingWidth = 140;
                attackingHeight = 140;
            }
            this.playerID = playerID;

            //We have to load the new data into the new sprites information
            //since the sprite sheets are at different heights and widths
            //and the position will change as well depending on the sprite
            destRect = new Rectangle((int)center.X - (width/2), (int)footArea.Y - height + 14 , width, height);
            sourceRect = new Rectangle(xSource * width, ySource * height, width, height);
            position.X = destRect.X;
            position.Y = destRect.Y;
            update(new KeyboardState(), new GameTime());
        }

        //If the player is attacking we will attacking in the right direction
        //and since the player's sprite size is different for attacking
        //we must accomadate that 
        public void attackUpdate(GameTime timer, int directionFacing){
            
            attackingTimer += timer.ElapsedGameTime.Milliseconds;

            //first frame of attacking so we need to set everything up
            if (currAttackingFrame == 0 && !changed)
            {
                destRect.X = destRect.X + width / 2 - attackingWidth / 2;
                destRect.Y = destRect.Y + height/2 - attackingHeight/2;
                destRect.Width =  attackingWidth;
                spriteSheet = attackingSheet;
                sourceRect = new Rectangle(attackingWidth * currAttackingFrame, attackingHeight * directionFacing, attackingWidth, attackingHeight);
                destRect.Height = attackingHeight;
                changed = true;
                
            }

            //The animation is done change back to regular width and height
            if (currAttackingFrame == playerAttackingData[0]){
                currAttackingFrame = 0;
                destRect.Width = width;
                destRect.Height = height;
                destRect.X = destRect.X  - width/ 2 + attackingWidth / 2;
                destRect.Y = destRect.Y - height/2 + attackingHeight/2;
                changed = false;
                attacking = false;
                sourceRect = new Rectangle(0,0,width,height);
                spriteSheet = loadSheet(playerID);
                return;
            }
         
            //do the animation of attacking and play the sound at the correct sound
            if (attackingTimer > 25){
               sourceRect = new Rectangle(attackingWidth * currAttackingFrame, attackingHeight * directionFacing, attackingWidth, attackingHeight);
               if (currAttackingFrame == 2)
                   soundsPlayer.playSound(3);
                attackingTimer = 0;
                ++currAttackingFrame;
            }
            
        }
        
        //Level up checks to see if the player has the EXP to level up
        //and if it does it uses random to increase the stats
        public void levelUp()
        {
            if (currLvl < 100)
            {
                currEXP += (int)(rand.NextDouble() * 100);
                //we check to see if the player leveled up and if so
                //we level things up randomly
                if (currEXP >= 100)
                {
                    if (maxMana < 52)
                    {
                        int add = (int)(rand.Next(0, 2));
                        maxMana += add;
                        mana += add;
                        if (maxMana > 52)
                            maxMana = 52;
                    }
                    if (maxHealth < 52)
                    {
                        int add = (int)(rand.Next(1, 2));
                        maxHealth += add;
                        health += add;
                        if (maxHealth > 52)
                            maxHealth = 52;
                    }
                    intelligence += (int)(rand.Next(0, 2));
                    strength += (int)(rand.Next(0, 2));
                    defense += (int)(rand.Next(0, 2));
                    currEXP %= 100;
                    currLvl++;
                }
            }
            else
                currEXP = 0;
        }
        //max everything
        public void maxAll()
        {   
            maxHealth = health = mana = maxMana = 52;
            intelligence = 50;
            strength =50;
            defense = 50;
            currEXP = 0;
            currLvl  = 100;
            
                
        }

        //Take damage and bounce back according to the direction
        public void getHit(int dir){
            
               switch(dir){
                   case 0: backY = 100; break;
                   case 1: backX = 100; break;
                   case 2: backX = -100; break;
                   case 3: backY = -100; break;
               }
               hit = true;
        }

        //This is the freature that will make the character bounce back when taken 
        public void bounceBack(GameTime time){
            WallTile[] test = tilesNearPlayer(mapDesign.walls, footArea, 10, 10);
            if (backX > 0)
            {
                if(!test[2].isUsed)
                     position.X += 10;
                backX -= 10;
            }
            else if (backX < 0)
            {
                if (!test[3].isUsed)
                    position.X -= 10;
                backX += 10;
            }else  if (backY > 0)
            {
                if (!test[1].isUsed)
                    position.Y += 10;
                backY -= 10;
            }
            else if (backY < 0)
            {
                if (!test[0].isUsed)
                     position.Y -= 10;
                backY += 10;
            }
          
        }

        public bool playDeath(GameTime time){
            return true;
        }


        //Draw the current sprite sheet the rectangle at which is should be drawn on the screen
        //and then draw the right rectangle from the spritesheet with normal color
        public void Draw(SpriteBatch sp)
        {
            if(hit && hitTimer != 0 && hitTimer%6 == 0)
              sp.Draw(spriteSheet, destRect, sourceRect, Color.DarkRed);
            
            else if(!done)
                sp.Draw(spriteSheet, destRect, sourceRect, Color.White);
           
           

        }

        //We will set the talking true if the player is indeed talking to an NPC
        public bool talkingToNpc(List<Enemy> possibleNPC){
            for(int i = 0; i < possibleNPC.Count;i++){
                if(!possibleNPC[i].isNPC)
                    continue;
                if (NpcInFrontPlayer(possibleNPC[i])){
                    possibleNPC[i].setTalkedTo(true);
                    return true;

                }
            }
            return false;
        }

        //Depending on the direction we will check to see if the npc is ahead of the player
        public bool NpcInFrontPlayer(Enemy npc){

            switch (walkDir)
            {
                case 0: return new Rectangle(footArea.X, footArea.Y + footArea.Height, footArea.Width, footArea.Height).Intersects(npc.footArea);
                case 1: return new Rectangle(footArea.X + footArea.Width, footArea.Y, footArea.Width, footArea.Height).Intersects(npc.footArea);
                case 2: return new Rectangle(footArea.X - footArea.Width, footArea.Y, footArea.Width, footArea.Height).Intersects(npc.footArea);
            }
            return new Rectangle(footArea.X, footArea.Y - footArea.Height, footArea.Width, footArea.Height).Intersects(npc.footArea);

        }

        //check to see if the player is inside of the movementBox
        public bool levelChange()
        {

            foreach(NewMapTile t in mapDesign.nextMap){
                if (cf.insideRect(t.destRect,footArea))
                {
                    nextMap = t;
                    return true;

                }
            }
            return false;
        }

        //Changes to the next Level
        public void changeLevel(GameTime timer)
        {
          

            levelChangeTimer += timer.ElapsedGameTime.Milliseconds;
            if (levelChangeTimer > 50f)
            {
                destRect.X = (int)nextMap.getNextLvlPos().X * 32 - 10;
                destRect.Y = (int)nextMap.getNextLvlPos().Y * 32 - 10;
                mapDesign.loadNext(nextMap.getLevel());
               
                mapDesign.setChange();
                levelChangeTimer = 0f;

            }
        }

    }
    

}
