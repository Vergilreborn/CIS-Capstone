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

       
        Camera cam;

        

        //Textures for loading Hud and Gui's
        Texture2D playerGui;
        Texture2D characters;
        Texture2D trans;
        Texture2D titleScreen;
        Texture2D menuStuff;
        Texture2D life;
        Rectangle sourceRect;

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
        float flashingText = 0f;
        bool textOn = true;
        int health = 0;
        int maxHealth = 0;
        int mana = 0;
        int maxMana = 0;


        float animationTimer = 0f;
        int x = 0;
        int y = 0;
        char state = 'n';

         public AllGui(Camera cam, MainPlayer will, Texture2D hud,Texture2D trans, Texture2D charactersForHud
                                                                    ,Texture2D titleScreen, Texture2D menuScreen,Texture2D life){
             //initiate the camera and the position, place texture and have title screen and menu 
             this.life = life;
             this.cam = cam;
             this.titleScreen = titleScreen;
             this.trans = trans;
             sourceRect = new Rectangle(0, 0, 71 * 2, 71 * 2);
             this.characters = charactersForHud;
             this.playerGui = hud;
             this.menuStuff = menuScreen;

             pressEnterTextSource = new Rectangle(6, 10, 420, 30);
             
             loadSaveBoxSource = new Rectangle(261, 50, 728, 192);
             loadSaveBox2Source = new Rectangle(261, 255, 728, 192);
             fingerSource = new Rectangle(18, 320, 48, 40);
             newGameSource = new Rectangle(18, 52, 172, 48);
             loadGameSource = new Rectangle(18, 112, 192, 48);
             optionsSource = new Rectangle(18, 241, 172, 48);
             testRoomSource = new Rectangle(18, 176, 192, 48);
        
         

        }

        public char update(GameTime time, char state, char characterChange,MainPlayer player,KeyboardState currentKey,KeyboardState prevKey){

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
            
            if (state == 't')
            {

                if(prevKey.IsKeyDown(Keys.Enter) && currentKey.IsKeyUp(Keys.Enter))
                    state = 'p';

                animationTimer += time.ElapsedGameTime.Milliseconds;
                flashingText += time.ElapsedGameTime.Milliseconds;

                if (animationTimer > 50)
                {
                    animateSprite();
                    animationTimer = 0;
                }

                if (flashingText > 300 && textOn){
                    textOn = false;
                    flashingText = 0f;
                
                }else if (flashingText > 100 && !textOn){
                    textOn = true;
                    flashingText = 0f;
                }

            }
            return state;
        }

        
        public void animateSprite(){

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

        public void Draw(SpriteBatch sp)
        {
            
           
            if (state == 't')
            {
                sp.Draw(titleScreen, new Vector2(-cam.transform.M41, -cam.transform.M42), Color.White);


                if (textOn)
                    sp.Draw(menuStuff, new Vector2(-cam.transform.M41 + (cam.view.Width / 4) + 20, -cam.transform.M42 + (cam.view.Height / 2) + 20), pressEnterTextSource, Color.White);
                sp.Draw(trans, new Vector2(-cam.transform.M41 + (cam.view.Width / 4) - 110, -cam.transform.M42 + (cam.view.Height / 2) - 30), sourceRect, Color.White);
                sp.Draw(trans, new Vector2(-cam.transform.M41 + (cam.view.Width / 2) + 200, -cam.transform.M42 + (cam.view.Height / 2) - 30), sourceRect, Color.White);
            }
            else
            {
                sp.Draw(playerGui, new Vector2(-cam.transform.M41, -cam.transform.M42), Color.White);
                sp.Draw(characters, new Vector2((-cam.transform.M41) + 385, (-cam.transform.M42) + 35), Color.White);
                int j = 0;
                for (int i = 0; i <  maxHealth ; i += 2){
                    
                    if (health - i == 1)
                        sp.Draw(life, new Vector2(-cam.transform.M41 + 30 + (17 * (j%13)), -cam.transform.M42 + 30 + (17*(j/13))), new Rectangle(18, 1, 16, 16), Color.White);
                    else if (i < health)
                    {
                        sp.Draw(life, new Vector2(-cam.transform.M41 + 30 + (17 * (j % 13)), -cam.transform.M42 + 30 + (17 * (j / 13))), new Rectangle(1, 1, 16, 16), Color.White);
                    }else
                        sp.Draw(life, new Vector2(-cam.transform.M41 + 30 + (17 * (j % 13)), -cam.transform.M42 + 30 + (17 * (j / 13))), new Rectangle(35, 1, 16, 16), Color.White);
                    j++;
                }
                j = 0;
                for (int i = 0; i < maxMana ; i+=2)
                {

                    if (mana - i == 1)
                        sp.Draw(life, new Vector2(-cam.transform.M41 + 655 + (17 * (j % 13)), -cam.transform.M42 + 30 + (17 * (j / 13))), new Rectangle(18, 18, 16, 16), Color.White);
                    else if (i < mana)
                    {
                        sp.Draw(life, new Vector2(-cam.transform.M41 + 655 + (17 * (j % 13)), -cam.transform.M42 + 30 + (17 * (j / 13))), new Rectangle(1, 18, 16, 16), Color.White);
                    }
                    else
                        sp.Draw(life, new Vector2(-cam.transform.M41 + 655 + (17 * (j % 13)), -cam.transform.M42 + 30 + (17 * (j / 13))), new Rectangle(35, 18, 16, 16), Color.White);
                    j++;
                }

            }
            
            
            
        }
        


    }
}
