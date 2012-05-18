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
    class AllGui
    {

        //The camera 
        Camera cam;



        //Textures for loading Hud and Gui's
        Texture2D playerGui;
        Texture2D characters;
        Texture2D trans;
        Texture2D titleScreen;
        Texture2D menuStuff;
        Texture2D life;
        Texture2D playerMenu;
        Texture2D selectionScreen;
        Texture2D transBack;
        Rectangle sourceRect;

        //Transition color
        Color transColor;

        //TitleScreen
        Rectangle pressEnterTextSource;

        //TitleScreen Options
        Rectangle loadSaveBoxSource;
        Rectangle loadSaveBox2Source;
        Rectangle fingerSource;
        Rectangle newGameSource;
        Rectangle loadGameSource;
        Rectangle optionsSource;
        Rectangle testRoomSource;
        Rectangle playerRectangle = new Rectangle(0, 0, 0, 0);
      
        float flashingText = 0f;
        float transitionTimer = 0f;
        SpriteFont font;
        Texture2D currentPlayer;
    
        bool textOn = true;
        bool transDone = false;
        bool transDark = true;
        int health = 0;
        int maxHealth = 0;
        int mana = 0;
        int maxMana = 0;
        int playerID;

        //used to tell the player's information in the playerstatus
        String menuData = "";
        int fingerPositionY = 0;
        int fingerPosition2Y = 0;
        int fingerPosition2X = 0;

        float animationTimer = 0f;
        int x = 0;
        int y = 0;
        int state = -1;

        SoundPlayerGaia sounds;
        MapReader map;

        public AllGui(Camera cam, MainPlayer will, Texture2D hud, Texture2D trans, Texture2D charactersForHud
                                                                   , Texture2D titleScreen, Texture2D menuScreen, Texture2D life
                                                                   , Texture2D selectionScreen, Texture2D playerMenu,Texture2D transBack)
        {
            //initiate the camera and the position, place texture and have title screen and menu 
            this.life = life;
            this.cam = cam;
            this.titleScreen = titleScreen;
            this.trans = trans;
            this.characters = charactersForHud;
            this.playerGui = hud;
            this.menuStuff = menuScreen;
            this.selectionScreen = selectionScreen;
            this.playerMenu = playerMenu;
            this.transBack = transBack;
          

            //This is the source rectangle to do the transformation animations
            sourceRect = new Rectangle(0, 0, 71 * 2, 71 * 2);

            //These lines set up the images that are to be drawn on the selection screen
            pressEnterTextSource = new Rectangle(6, 10, 420, 30);
            loadSaveBoxSource = new Rectangle(261, 50, 728, 192);
            loadSaveBox2Source = new Rectangle(261, 255, 728, 192);
            fingerSource = new Rectangle(18, 320, 48, 40);
            newGameSource = new Rectangle(18, 52, 172, 48);
            loadGameSource = new Rectangle(18, 112, 192, 48);
            optionsSource = new Rectangle(18, 241, 172, 48);
            testRoomSource = new Rectangle(18, 176, 192, 48);
        }
        //we connect the sounds of menues and the font to this class file
        public void connect(SoundPlayerGaia sounds, SpriteFont font, MapReader map){
            this.sounds = sounds;
            this.font = font;
            this.map = map;
            font.LineSpacing = 39;
        }

        //The update loop that gets called constantly
        public int update(GameTime time, int state, char characterChange, MainPlayer player, KeyboardState currentKey, KeyboardState prevKey)
        {

            this.state = state;
            //check to see if the character has different hp
            //add additional items if the exist,
            //see if we are in the titlescreen
            //update to the according states
            //if game playing update
            this.maxHealth = player.maxHealth;
            this.maxMana = player.maxMana;
            this.health = player.health;
            this.mana = player.mana;
            this.playerID = player.playerID;

            //This updates the corresponding GUI/HUD depending on the right
            //state
            switch (state)
            {

                //This is for working on the title screen
                case 0:
                    if (prevKey.IsKeyDown(Keys.Enter) && currentKey.IsKeyUp(Keys.Enter))
                        state = 1;

                    animationTimer += time.ElapsedGameTime.Milliseconds;
                    flashingText += time.ElapsedGameTime.Milliseconds;

                    if (animationTimer > 50)
                    {
                        animateSprite();
                        animationTimer = 0;
                    }

                    if (flashingText > 300 && textOn)
                    {
                        textOn = false;
                        flashingText = 0f;

                    }
                    else if (flashingText > 100 && !textOn)
                    {
                        textOn = true;
                        flashingText = 0f;
                    }
                    break;

                //This is for loading,starting a new game or going to the test room
                case 1:

                    if (prevKey.IsKeyUp(Keys.Up) && currentKey.IsKeyDown(Keys.Up))
                    {
                        fingerPositionY -= 60;
                        if (fingerPositionY < 0)
                            fingerPositionY = 180;
                        fingerPositionY = fingerPositionY % 240;
                        sounds.playSound(0);
                    }

                    if (prevKey.IsKeyUp(Keys.Down) && currentKey.IsKeyDown(Keys.Down))
                    {

                        fingerPositionY += 60;
                        fingerPositionY = fingerPositionY % 240;
                        sounds.playSound(0);
                    }
                    if (prevKey.IsKeyDown(Keys.Enter) && currentKey.IsKeyUp(Keys.Enter))
                        switch (fingerPositionY)
                        {
                            case 0: state = 3; sounds.playSound(1); break;
                            case 60: state = 4; sounds.playSound(1); break;
                            case 120: state = 5; sounds.playSound(1); break;
                            case 180: state = 6; sounds.playSound(1); break;

                        }
                    break;
                
                //This is for the menu for the player
                case 2:
                    //Displays the players stats according to the player
                    menuData = player.currLvl + "\n" + player.maxHealth + "\n"
                               + player.maxMana + "\n0\n" + player.strength
                               + "\n" + player.defense + "\n" + player.intelligence +
                               "\n" + player.currEXP;
                    
                    //Check to see what keys were pressed and move the finger accordingly
                    if (prevKey.IsKeyUp(Keys.Up) && currentKey.IsKeyDown(Keys.Up))
                    {
                        fingerPosition2Y -= 47;
                        if (fingerPosition2Y < 0)
                            fingerPosition2Y = 282;
                        fingerPosition2Y = fingerPosition2Y % 329;
                        sounds.playSound(0);
                    }

                    if (prevKey.IsKeyUp(Keys.Down) && currentKey.IsKeyDown(Keys.Down))
                    {

                        fingerPosition2Y += 47;
                        fingerPosition2Y = fingerPosition2Y % 329;
                        sounds.playSound(0);
                    }
                        if (prevKey.IsKeyUp(Keys.Left) && currentKey.IsKeyDown(Keys.Left))
                    {
                        fingerPosition2X -= 63;
                        if (fingerPosition2X < 0)
                            fingerPosition2X = 378;
                        fingerPosition2X = fingerPosition2X % 441 ;
                        sounds.playSound(0);
                    }

                    if (prevKey.IsKeyUp(Keys.Right) && currentKey.IsKeyDown(Keys.Right))
                    {

                        fingerPosition2X += 63;
                        fingerPosition2X = fingerPosition2X % 441;
                        sounds.playSound(0);
                    }
                    //We unpause the game and move to the next available gamestate
                    if (prevKey.IsKeyUp(Keys.RightShift) && currentKey.IsKeyDown(Keys.RightShift))
                        state = 3;

                    //Change what the character looks like on the sprite sheet making sure its the standing still image
                    if(!player.attacking)
                        currentPlayer = player.spriteSheet;
                    playerRectangle = new Rectangle(0, 0, player.width, player.height);
                    break;

                    //This is when the game is running and if this button is pressed, the 
                    //menu screen pops up
                case 3:
                case 4:
                case 5:
                case 6:
                    //these keys are pressed means that the game is paused into the players menu
                  if (prevKey.IsKeyUp(Keys.RightShift) && currentKey.IsKeyDown(Keys.RightShift))
                    state = 2;
                    break;
                case 7:
                    //do transitions
                    if (!transitionDone()){
                        transition(time);
                    }else{
                        transDone = false;
                        map.setChange();
                        state = 3;
                    }
                    break;
            }
           
            return state;
        }

        public void transition(GameTime time){
           //add to time and check to see if we are transition up or down
            //change the alpha accordingly to transition
            transitionTimer += time.ElapsedGameTime.Milliseconds;
            if (transitionTimer > 30f){
                if (transDark)
                {
                    transColor.A = (byte)(transColor.A + 20);
                    if (transColor.A == 240)
                        transDark = false;
                }else{
                    transColor.A = (byte)(transColor.A - 20);
                    if (transColor.A < 20)
                    {
                        transDark = true;
                        transDone = true;
                    }

                }
                transitionTimer = 0f;
            }

        }

        public bool transitionDone(){
            return transDone;

        }

        //The animation timer and changing of the sprite occurs here
        public void animateSprite()
        {

            sourceRect = new Rectangle(x * 142, y * 142, 142, 142);
            x++;
            if (x % 10 == 0)
            {
                x = 0;
                y++;
            }
            if (y % 15 == 0)
                y = 0;
        }
      

        //Updates the screen according to the right state with the correct images and location
        public void Draw(SpriteBatch sp)
        {


            switch (state)
            {

                    //Drawing the title screen information
                case 0:
                    sp.Draw(titleScreen, new Vector2(-cam.transform.M41, -cam.transform.M42), Color.White);


                    if (textOn)
                        sp.Draw(menuStuff, new Vector2(-cam.transform.M41 + (cam.view.Width / 4) + 20, -cam.transform.M42 + (cam.view.Height / 2) + 20), pressEnterTextSource, Color.White);
                    sp.Draw(trans, new Vector2(-cam.transform.M41 + (cam.view.Width / 4) - 110, -cam.transform.M42 + (cam.view.Height / 2) - 30), sourceRect, Color.White);
                    sp.Draw(trans, new Vector2(-cam.transform.M41 + (cam.view.Width / 2) + 200, -cam.transform.M42 + (cam.view.Height / 2) - 30), sourceRect, Color.White);
                    return;

                    //drawing the menuscreen after the title menu
                case 1:
                    sp.Draw(selectionScreen, new Vector2(-cam.transform.M41, -cam.transform.M42), Color.White);
                    sp.Draw(menuStuff, new Vector2(-cam.transform.M41 + (cam.view.Width / 4) - 80, -cam.transform.M42 + (cam.view.Height / 2) - 120), newGameSource, Color.LightGray);
                    sp.Draw(menuStuff, new Vector2(-cam.transform.M41 + (cam.view.Width / 4) - 80, -cam.transform.M42 + (cam.view.Height / 2) - 60), loadGameSource, Color.LightGray);
                    sp.Draw(menuStuff, new Vector2(-cam.transform.M41 + (cam.view.Width / 4) - 80, -cam.transform.M42 + (cam.view.Height / 2)), optionsSource, Color.LightGray);
                    sp.Draw(menuStuff, new Vector2(-cam.transform.M41 + (cam.view.Width / 4) - 80, -cam.transform.M42 + (cam.view.Height / 2) + 60), testRoomSource, Color.LightGray);
                    sp.Draw(menuStuff, new Vector2(-cam.transform.M41 + (cam.view.Width / 4) - 120, -cam.transform.M42 + (cam.view.Height / 2) - 120 + fingerPositionY), fingerSource, Color.White);
                   
                    return;
                case 7:
                        sp.Draw(transBack,new Vector2(-cam.transform.M41, -cam.transform.M42), transColor);
                        break;
            }


            //This is when playing the game and the game is active
                sp.Draw(playerGui, new Vector2(-cam.transform.M41, -cam.transform.M42), Color.White);
                sp.Draw(characters, new Vector2((-cam.transform.M41) + 385, (-cam.transform.M42) + 35), Color.White);
                int j = 0;
                Rectangle src;
                for (int i = 0; i < maxHealth; i += 2)
                {

                    if (health - i == 1)
                        src = new Rectangle(18 + ((playerID - 1) * 51), 1, 16, 16);
                    else if (i < health)
                        src = new Rectangle(1 + ((playerID - 1) * 51), 1, 16, 16);
                    else
                        src = new Rectangle(35 + ((playerID - 1) * 51), 1, 16, 16);
                    sp.Draw(life, new Vector2(-cam.transform.M41 + 30 + (17 * (j % 13)), -cam.transform.M42 + 30 + (17 * (j / 13))), src, Color.White);

                    j++;
                }
                j = 0;
                for (int i = 0; i < maxMana; i += 2)
                {

                    if (mana - i == 1)
                        src = new Rectangle(18 + ((playerID - 1) * 51), 18, 16, 16);
                    else if (i < mana)
                        src = new Rectangle(1 + ((playerID - 1) * 51), 18, 16, 16);
                    else
                        src = new Rectangle(35 + ((playerID - 1) * 51), 18, 16, 16);
                    sp.Draw(life, new Vector2(-cam.transform.M41 + 655 + (17 * (j % 13)), -cam.transform.M42 + 30 + (17 * (j / 13))), src, Color.White);
                    j++;
                }
                //draws the menu
                if (state == 2){

                    sp.Draw(playerMenu, new Vector2(-cam.transform.M41, -cam.transform.M42), Color.White);
                    sp.Draw(menuStuff, new Vector2(-cam.transform.M41 + 400 + fingerPosition2X, -cam.transform.M42 +20+ fingerPosition2Y),fingerSource,Color.White);
                    sp.Draw(currentPlayer, new Vector2(-cam.transform.M41 + 110 - playerRectangle.Width/2, -cam.transform.M42+ 150 -playerRectangle.Height),playerRectangle,Color.White);
                    sp.DrawString(font, menuData, new Vector2(-cam.transform.M41 + 80, -cam.transform.M42 + 215), Color.White);

                }
                
        }
    }
    
}
