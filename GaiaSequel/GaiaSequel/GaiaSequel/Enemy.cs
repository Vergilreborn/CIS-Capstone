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

        //The stats of the enemy
        public int health;
        public int str;
        public int def;
        int width;
        int height;
        int backX = 0;
        int backY = 0;
        
        //The enemy data as top location/center
        Vector2 position;
        public Vector2 center;
        public Rectangle destRect;
        public Rectangle sourceRect;
        //the sprites spritesheet
        Texture2D sprite;
        

        //Debugging String
        String enemyString = "";

        //used for the dominate key
        Keys dominateKey = Keys.End;
        Keys secKey = Keys.End;
        Camera cam;

        float walkTimer = 0f;
        
        //TEMP MAX WALK FRAMES
        int walkFrames = 4;
        int currWalkFrame = 0;
        MapReader mapDesign;
      
        //The boolean expressions
        bool walking = false;
        bool simpleAI;
        bool flying;
        public bool isNPC;
        public bool done = false;
        public bool dead = false;
        public bool hit = false;
        bool talked = false;
        public int walkDir;
        float leftRightSpeed = 3f;
        float upDownSpeed = 2f;
        Vector2 savedPosition;
        public Rectangle footArea;

        //AI movement
        float aiTimer = 0f;
        float hitTimer = 0f;
        double percentage = 0;
        Random rand;
        public Color color;
        List<String> texts = new List<String>();
        SoundPlayerGaia sound;
        public Texture2D textBox;

        //the constuctor as to setting up the spritesheet
        //the information of the enemy.
        public Enemy(int health, int str, int def, Vector2 position, int width, int height, bool flying,Texture2D sprite,bool simpleAI,Random rand,bool isNPC){
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
            this.isNPC = isNPC;
            sourceRect = new Rectangle(0, 0, width, height);
            destRect = new Rectangle((int)position.X,(int)position.Y, width, height);
            center = new Vector2(destRect.X + (width / 2), destRect.Y + (height / 2));
            footArea = new Rectangle((int)(center.X - 13), (int)(destRect.Y + height - 20), 26, 18);
        }

        //Updating the enemy with the keyboard state the timer and the location of the player
        public void update(KeyboardState newState, GameTime timer, Vector2 playerPosition){
            walking = false;

            if (isNPC && simpleAI){
                updateNPC(timer,playerPosition);
                return;
            }


            //If the health is less than 0 then the enemy
            //is considered to be dead
            if (health > 0)
            {
                done = false;
                dead = false;
            }
            else
                dead = true;

            //if the enemy is fully dead then we move it off screen for the meantime
            if (done)
            {
                
                center = new Vector2(destRect.X + (width / 2), destRect.Y + (height / 2));
                footArea = new Rectangle((int)(center.X - 13), (int)(destRect.Y + height - 20), 26, 18);
                return;
            }
            //play the dead animation
            if (dead == true)
            {
              
                done = playDeath(timer);
                return;
            }

            //This is when the enemy is hit. When so we will
            //give it slight invincibility and some time to 
            //for it to get "knockedback"
            if (hit)
            {
                hitTimer += timer.ElapsedGameTime.Milliseconds;
                bounceBack(timer);
                if (hitTimer > 500)
                {
                    hit = false;
                    hitTimer = 0;
                }

                if (backX > 0 || backY > 0)
                {
                    movement(new Keys(),new Keys());
                    
                    destRect.X = (int)position.X;
                    destRect.Y = (int)position.Y;
                    center = new Vector2(destRect.X + (width / 2), destRect.Y + (height / 2));
                    footArea = new Rectangle((int)center.X - 14, (int)(position.Y + height) - 14, 26, 14);
                    return;
                }
            }

            //Simple AI where the enemy moves towards the player for the meantime
            //if the AI is off then we can control the enemies if we connect to them
            if (simpleAI)
                aiMovement(timer, playerPosition);
            else
            {
                  //We will get the dominate key and the secondary key if one is pressed   
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
                

                //if the enemy is flying then we don't have to worry about any collision and 
                //we can just update, otherwise the enemy is on the ground and walls will affect
                //them
                if (flying)
                    movementNoColl(newState);
                else
                    movement(newState);
            }
                
            //if the enemy is walking or flying we will update it
            //otherwise the enemy is standing facing a certain direction
            if(walking || flying)
                movingAnimation(timer,walkDir);
            else 
                sourceRect = new Rectangle(0, height * walkDir, width, height);

            
            //update locations and centers
            destRect.X = (int)position.X;
            destRect.Y = (int)position.Y;
            center = new Vector2(destRect.X + (width/2), destRect.Y + (height/2));
            footArea = new Rectangle((int)(center.X -13), (int)(destRect.Y + height -20), 26, 18);
        
          }
        
        //have the enemy be able to access the map
        public void connectMap(MapReader map,Camera cam,SoundPlayerGaia sound){
            this.mapDesign = map;
            this.cam = cam;
            this.sound = sound;
        }
        //Turn of the AI so the enemy doesn't move
        public void turnOffAi(){
            if (simpleAI)
                simpleAI = false;
            else
                simpleAI = true;
        }
        //This generates random movement time towards the enemy.
        //We generate dominate keys to follow the player
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
                //This is the chance that we will have to go and attack the player
                percentage = ((rand.NextDouble() * 100));
                if (percentage < 4)
                    aiTimer = rand.Next(1000, 2000);
                enemyString = percentage + " TIMER " + aiTimer;
            }
           
        }
        //This checks to see if the player is within range of the enemy.
        //If the player is within 200 pixels from the player then the enemy
        //will go towards the player. This will also set up the dominate key
        public bool withinRange(Vector2 playerPosition){
            dominateKey = Keys.Delete;
            secKey = Keys.Delete;
            if ((Math.Abs(playerPosition.X - footArea.X) > Math.Abs(playerPosition.Y - footArea.Y)))
            {

                if (this.footArea.X - 200 < playerPosition.X && this.footArea.X + 200 > playerPosition.X)
                {
                    if (playerPosition.X+15 < footArea.X)
                        dominateKey = Keys.Left;
                    else if (playerPosition.X-5 > footArea.X)
                        dominateKey = Keys.Right;
                }
            
            }
            else
            {
                if (this.footArea.Y - 150 < playerPosition.Y && this.footArea.Y + 150 > playerPosition.Y)
                {
                    if (playerPosition.Y+15< footArea.Y)
                        dominateKey = Keys.Up;
                    else if (playerPosition.Y-5 > footArea.Y)
                        dominateKey = Keys.Down;
                }

            }
            return dominateKey != Keys.Delete;
        }

        //This is the same movement as the player but we include the keys pressed
        //and if we won't bother checking to see if anything is in the way of 
        //movement
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

        //Movement with walls in mind/objects
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
        
        //Movement with no collision. This is where the player is controlling the 
        //creature
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
        
        //The player controls this creature to test them out
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
        

        //Generate the movement animation depening on direction
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
        //Draw the enemy according state, either dead or alive
        public void draw(SpriteBatch sp,GraphicsDevice g, SpriteFont informFont){
            

            //We draw a box around the foot area
            var t = new Texture2D(g, 1, 1);
            t.SetData(new[] { Color.White });
            sp.DrawString(informFont, enemyString, new Vector2(center.X + 50, center.Y - 40), Color.Red);
            sp.Draw(t, footArea, Color.White);
        
            if (hit && hitTimer != 0 && hitTimer % 6 == 0)
                sp.Draw(sprite, destRect, sourceRect, Color.DarkRed);

            else if (!done)
                sp.Draw(sprite, destRect, sourceRect, Color.White);
           
           
           
        }
        //Drawi with a color
        public void draw(SpriteBatch sp, GraphicsDevice g, SpriteFont informFont, Color color)
        {

            var t = new Texture2D(g, 1, 1);
            t.SetData(new[] { Color.White });
            sp.DrawString(informFont, enemyString, new Vector2(center.X + 50, center.Y - 40), Color.Red);
            sp.Draw(t, footArea, Color.White);
            sp.Draw(sprite, destRect, sourceRect, Color.White);


            //Since this is an NPC, when the NPC is talked to, currently hardcoded but will be genericized
            //depending on the amount of strings in the mapreader
            //--------------------------To be implemented------------------//
            if (talked){
                sp.Draw(textBox, new Vector2(-cam.transform.M41,-cam.transform.M42 + 375), Color.White);
                sp.DrawString(informFont, texts[0], new Vector2(-cam.transform.M41 + 10, -cam.transform.M42 + 385), color);
                sp.DrawString(informFont, texts[1], new Vector2(-cam.transform.M41 + 10, -cam.transform.M42 + 415), color);
                sp.DrawString(informFont, texts[2], new Vector2(-cam.transform.M41 + 10, -cam.transform.M42 + 445), color);
                sp.DrawString(informFont, texts[3], new Vector2(-cam.transform.M41 + 10, -cam.transform.M42 + 475), color);
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

        //This will bounce the player back checking if there are walls.
        //if there are walls we don't bounce back but rather stay put
        public void bounceBack(GameTime time)
        {
            WallTile[] test = tilesNearPlayer(mapDesign.walls, footArea, 10, 10);
            if (backX > 0)
            {
                if (!test[2].isUsed)
                    position.X += 10;
                backX -= 10;
            }
            else if (backX < 0)
            {
                if (!test[3].isUsed)
                    position.X -= 10;
                backX += 10;
            }
            else if (backY > 0)
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
        //playe the death animation AKA EXPLOSION
        public bool playDeath(GameTime time)
        {
            return true;
        }
        //If the enemy gets hit we must set the bounce back
        public void getHit(int dir)
        {
            //depending on the player's direction the enemy will bounce
            //back in a certian direction
            switch (dir)
            {
                case 0: backY = 100; break;
                case 1: backX = 100; break;
                case 2: backX = -100; break;
                case 3: backY = -100; break;
            }
            sound.playSound(4);
            hit = true;
        }

        //This sets the talking expression depending on if this NPC was talked to
        public void setTalkedTo(bool talked){
            this.talked = talked;
        }

        //We update the NPC if and only if the enemy is an NPC
        //This functionality will be different for the NPC depending on the movement
        //-----------------------MOVEMENT OPTIONS NEED TO BE IMPLEMENTED---------------------------//
        public void updateNPC(GameTime timer,Vector2 playerPosition){
            //If not being talked to, we don't do anything yet
            //Movement will be implemented when ! being talked to
            //-------------IMPLEMENTATION NEEDED------------------//
            if (!talked)
                return;
            
            //Depending ont he players location, if he is closer in the change of Y 
            //or the change in X the NPC will face the player.
            Vector2 thisCenter = new Vector2(footArea.X + footArea.Width / 2, footArea.Y + footArea.Height / 2);
            bool isX = Math.Abs(playerPosition.Y - thisCenter.Y) == Math.Min(Math.Abs(playerPosition.X - thisCenter.X), Math.Abs(playerPosition.Y - thisCenter.Y));
            if (!isX)
            {
                if (playerPosition.Y < thisCenter.Y)
                {
                    sourceRect.Y = 3 * sourceRect.Height;
                }
                else
                    sourceRect.Y = 0;
            }
            else
            {
                if (playerPosition.X < thisCenter.X)
                {
                    sourceRect.Y = 2 * sourceRect.Height;
                }
                else
                    sourceRect.Y = sourceRect.Height;
            }
        }

        //Add any text to the List of strings for this character
        public void addText(String text) {
            texts.Add(text);
        }


    }
}
