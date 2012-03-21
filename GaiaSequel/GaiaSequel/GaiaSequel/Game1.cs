using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GaiaSequel
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        CollisionFunctions colFunctions = new CollisionFunctions();
        //Declaring our variables
        MainPlayer player;
        AllGui allGui;
        Enemy skeleton;
        Enemy bat;
        Enemy powerHitter;
        Enemy lance;
        Enemy[] enemyHolder = new Enemy[3];
        MapReader mapDesigner;
        SpriteFont informFont;
        SpriteFont informFont2;
        Vector2 fontPosition;
        Vector2 center;
        Camera cam;
        Random rand;
        String fpsFont = "";
        float fpsTimer = 0f;
        double fps = 0;
        
        
        KeyboardState prevState = Keyboard.GetState();

        bool hidePrint = false;
        bool hideMap = false;
        bool normal = false;
        bool stats = false;
        bool paused = false;
        bool needDarkBack = false;
        int playerSwitch = 0;
        int gameState;
        MediaPlayerGaia mediaPlayer;
        SoundPlayerGaia soundPlayer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 550;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            rand = new Random();
            mediaPlayer = new MediaPlayerGaia();
            soundPlayer = new SoundPlayerGaia();

          
            //Load the main player
            player = new MainPlayer(Content.Load<Texture2D>("Sprites/Will"),Content.Load<Texture2D>("Sprites/WillAttackingSheet")
                                     ,Content.Load<Texture2D>("Sprites/freedan"),null,new Vector2(160, 120),64,84);
            skeleton = new Enemy(20, 5, 4, new Vector2(60, 50), 72, 120, false, Content.Load<Texture2D>("Sprites/Enemy/Skeleton"),true,rand,false);
            bat = new Enemy(14,9, 3, new Vector2(70, 70), 80, 80, true, Content.Load<Texture2D>("Sprites/Enemy/Bat"),true,rand,false);
            powerHitter= new Enemy(100, 20, 25, new Vector2(700, 300), 72, 120, false, Content.Load<Texture2D>("Sprites/Enemy/Skeleton"), true, rand,false);
            lance = new Enemy(10, 10, 10, new Vector2(600, 200), 64, 64, false, Content.Load<Texture2D>("Sprites/NPC/NPCSheet"), true, rand,true);
            //startUpMapBuilder 
            mapDesigner = new MapReader(Content.Load<Texture2D>("Maps/GTILES32"));
            
            //This tells the camera to follow the player as well as setting up the camera
            cam = new Camera(graphics.GraphicsDevice.Viewport, Vector2.Zero);
            cam.Follow(player); 
            
            //load font and set position
            informFont = Content.Load<SpriteFont>("Text2");
            informFont2 = Content.Load<SpriteFont>("Text");

            
            mediaPlayer.init(Content);
            soundPlayer.init(Content);
           
           
         
            //Load first level
            mapDesigner.init();
            mapDesigner.buildMap("Content/Maps/level",0);

            //Connect the map to the main player and temporary enemies
            player.connectMap(mapDesigner,rand,soundPlayer);
            skeleton.connectMap(mapDesigner,cam,soundPlayer);
            bat.connectMap(mapDesigner,cam,soundPlayer);
            powerHitter.connectMap(mapDesigner,cam,soundPlayer);
            lance.connectMap(mapDesigner, cam,soundPlayer);
            enemyHolder[0] = skeleton;
            enemyHolder[1] = bat;
            enemyHolder[2] = powerHitter;

            //sets the game state to opening screen
            //stats - '0' for title
            //        '1' for openingScreen
            //        '2' for player menu
            gameState = 0;
            lance.addText("Hello there, this is just a test to make sure that NPC is ");
            lance.addText("working correctly. This is another test run because ");
            lance.addText("we will be reading line by line or max characters. Not ");
            lance.addText("100% sure what will be done with this yet");
            lance.color = Color.LightGreen;
            lance.textBox = Content.Load<Texture2D>("Sprites/Gui/TextBox");
            //sets up the Hud and all the graphics any menu
            allGui = new AllGui(cam, player, Content.Load<Texture2D>("Sprites/Gui/PlayerGui")
                                                  , Content.Load<Texture2D>("Sprites/Gui/Transformation2")
                                                  , Content.Load<Texture2D>("Sprites/Gui/characters2"),
                                                  Content.Load<Texture2D>("Sprites/Gui/TitleScreen"),
                                                  Content.Load<Texture2D>("Sprites/Gui/IntroScreenSetup")
                                                  , Content.Load<Texture2D>("Sprites/Gui/life")
                                                  , Content.Load<Texture2D>("Sprites/Gui/SelectionScreen"), 
                                                  Content.Load<Texture2D>("Sprites/Gui/PlayerMenu"));

            allGui.connect(soundPlayer,Content.Load<SpriteFont>("Text"));
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            //Calculates the Frame Rate
            fps++;
            fpsTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (fpsTimer > 1000f)
            {
                fpsFont = ((fps/fpsTimer)*1000) + "";
                fpsTimer = 0f;
                fps = 0;
            }

            //Grabs the keyboard state
            KeyboardState current = Keyboard.GetState();

            // Allows the game to exit
            if (current.IsKeyDown(Keys.Back) && prevState.IsKeyUp(Keys.Back))
                this.Exit();
           

            //All keys for action are debugging
            if (current.IsKeyDown(Keys.A) && prevState.IsKeyUp(Keys.A))
                if (gameState == 0)
                    gameState = 1;
                else
                    gameState = 0;
            //allows the game to be paused
            if (current.IsKeyDown(Keys.P) && prevState.IsKeyUp(Keys.P))
                if (paused)
                    paused = false;
                else
                    paused = true;

            //this will let use hide the print statements
            if (current.IsKeyDown(Keys.OemTilde) && prevState.IsKeyUp(Keys.OemTilde))
                if (hidePrint)
                    hidePrint = false;
                else
                    hidePrint = true;

            //allows use to toggle the map on and off
            if (current.IsKeyDown(Keys.D1) && prevState.IsKeyUp(Keys.D1))
                if (hideMap)
                    hideMap = false;
                else
                    hideMap = true;

            //allows use to toggle the map on and off
            if (current.IsKeyDown(Keys.H) && prevState.IsKeyUp(Keys.H))
                if (player.health < player.maxHealth)
                {
                    player.health++;
                    soundPlayer.playSound(2);
                }
            if (current.IsKeyDown(Keys.S) && prevState.IsKeyUp(Keys.S))
                if (player.playerID == 1)
                    player.loadNewPlayer(2);
                else
                    player.loadNewPlayer(1);
            if (current.IsKeyDown(Keys.J) && prevState.IsKeyUp(Keys.J))
                if (1 < player.health)
                    player.health--;

            if (current.IsKeyDown(Keys.B) && prevState.IsKeyUp(Keys.B))
                if (player.mana < player.maxMana)
                {
                    player.mana++;
                    soundPlayer.playSound(2);
                }
            if (current.IsKeyDown(Keys.N) && prevState.IsKeyUp(Keys.N))
                if (1 < player.mana)
                    player.mana--;

            //sees the map normally and or collision type
            if (current.IsKeyDown(Keys.D2) && prevState.IsKeyUp(Keys.D2))
                if (normal)
                    normal = false;
                else
                    normal = true;

            //sees the player stats
            if (current.IsKeyDown(Keys.D3) && prevState.IsKeyUp(Keys.D3))
                if (stats)
                    stats = false;
                else
                    stats = true;
            //the background on/off
            if (current.IsKeyDown(Keys.D4) && prevState.IsKeyUp(Keys.D4))
                if (needDarkBack)
                     needDarkBack = false;
                else
                    needDarkBack = true;
            //Tests the leveling up for the player
            if (current.IsKeyDown(Keys.Q) && prevState.IsKeyUp(Keys.Q))
                player.levelUp();

            //turns of the enemies simple AI
            if (current.IsKeyDown(Keys.W) && prevState.IsKeyUp(Keys.W)){
                skeleton.turnOffAi();
                bat.turnOffAi();
                powerHitter.turnOffAi();
                lance.turnOffAi();
            }

          
            //Debugging purpose of controlling another enemy and player
            if (current.IsKeyDown(Keys.Tab) && prevState.IsKeyUp(Keys.Tab))
                if (playerSwitch == 0){
                    playerSwitch = 1;
                    cam.Follow(player);
                }
                else if (playerSwitch == 1)
                {
                    playerSwitch = 2;
                    cam.Follow(skeleton);
                }
                else if (playerSwitch == 2)
                {
                    playerSwitch = 3;
                    cam.Follow(bat);

                }
                else if (playerSwitch == 3)
                {
                    playerSwitch = 4;
                    cam.Follow(powerHitter);
                }
                else
                {
                    playerSwitch = 0;
                    cam.Follow(lance);
                
                }


            //this loop runs if the game is active, play the hardcoded song for the mean time
            if (gameState != 0 && gameState != 1 && gameState !=2)
            {
                mediaPlayer.playNew(1);

                //If the game is not paused we continue the music and update all the characters on the screen
                if (!paused)
                {
                    mediaPlayer.resume();


                    if (current.IsKeyDown(Keys.D) && prevState.IsKeyUp(Keys.D))
                    {
                        if (player.NpcInFrontPlayer(lance) && !player.talking)
                        {
                            player.talking = true;
                            lance.setTalkedTo(true);
                            soundPlayer.playSound(5);
                        }
                        else{
                            player.talking = false;
                            lance.setTalkedTo(false);
                        }
                            
                        
                    }


                    if (playerSwitch == 1)
                    {
                        center = skeleton.center;

                        player.update(new KeyboardState(), gameTime);
                        skeleton.update(current, gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        bat.update(new KeyboardState(), gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        powerHitter.update(new KeyboardState(), gameTime,  new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                    }
                    else if (playerSwitch == 0)
                    {
                        center = player.center;
                        player.update(current, gameTime);


                        
                        //We want to worry about footBoundingBoxForMovement and NOT the player position
                        skeleton.update(new KeyboardState(), gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        bat.update(new KeyboardState(), gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        powerHitter.update(new KeyboardState(), gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                    }
                    else if (playerSwitch == 2)
                    {
                        center = bat.center;
                        bat.update(current, gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        skeleton.update(new KeyboardState(), gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        player.update(new KeyboardState(), gameTime);

                    }
                    else if (playerSwitch == 3)
                    {
                        center = powerHitter.center;
                        powerHitter.update(current, gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        bat.update(new KeyboardState(), gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        skeleton.update(new KeyboardState(), gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        player.update(new KeyboardState(), gameTime);

                    }
                    else
                    {
                        center = lance.center;
                        lance.update(current, gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        powerHitter.update(new KeyboardState(), gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        bat.update(new KeyboardState(), gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        skeleton.update(new KeyboardState(), gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                        player.update(new KeyboardState(), gameTime);
                    }
                    if (player.talking)
                    {
                        lance.update(new KeyboardState(), gameTime, new Vector2(player.footArea.X + player.footArea.Width / 2, player.footArea.Y + player.footArea.Height / 2));
                    }
                    if (!player.hit)
                        for (int i = 0; i < enemyHolder.Length; i++){
                            if (colFunctions.attackingOtherHit(enemyHolder[i].footArea, 10, enemyHolder[i].walkDir, player.footArea))
                            {
                                if (player.defense < enemyHolder[i].str)
                                    player.health = player.health - enemyHolder[i].str + player.defense;
                                else
                                    player.health--;
                                
                                player.getHit(enemyHolder[i].walkDir);
                            }
                        }
                    if(player.attacking) 
                        for(int i = 0; i < enemyHolder.Length; i++)
                           
                                if (!enemyHolder[i].hit)
                                    if (colFunctions.attackingOtherHit(player.footArea, 30, player.walkDir, enemyHolder[i].footArea))
                                    {
                                        if (enemyHolder[i].def < player.strength)
                                            enemyHolder[i].health = enemyHolder[i].health - player.strength + enemyHolder[i].def;
                                        else
                                            enemyHolder[i].health--;
                                        enemyHolder[i].getHit(player.walkDir);
                                    
                            }
                    for (int i = 0; i < enemyHolder.Length; i++)
                        if (enemyHolder[i].done)
                        {
                            enemyHolder[i].destRect.X = -200;
                            enemyHolder[i].destRect.Y = -200;
                        }



                }
                else
                    mediaPlayer.pause();
            }
            else if (gameState == 0)
                mediaPlayer.playNew(0);
            else if (gameState == 1)
                mediaPlayer.playNew(2);



            

                
            //also update the fonts position
            fontPosition = new Vector2(player.position.X + 100, player.position.Y - 100);

            // TODO: Add your update logic here
            cam.Update(gameTime, center);
            gameState = allGui.update(gameTime, gameState, 'n',player,current,prevState);
            //update the previous keystate
            prevState = current;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if(!needDarkBack)
                GraphicsDevice.Clear(Color.Black);
            else
                GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cam.transform);

            if (gameState != 0 && gameState != 1)
            {
                //draws debugger information
                if (!hideMap)
                {
                    mapDesigner.Draw(spriteBatch, normal);
                }
              
                if (!hidePrint)
                {
                    var t = new Texture2D(GraphicsDevice, 1, 1);
                    t.SetData(new[] { Color.White });

                    spriteBatch.DrawString(informFont, player.posString, fontPosition, Color.DarkRed);
                    spriteBatch.DrawString(informFont, player.groundString, new Vector2(fontPosition.X, fontPosition.Y + 20), Color.DarkRed);
                    spriteBatch.DrawString(informFont, player.jumpingString, new Vector2(fontPosition.X, fontPosition.Y + 40), Color.DarkRed);
                    spriteBatch.DrawString(informFont, player.jumpVectorString, new Vector2(fontPosition.X, fontPosition.Y + 60), Color.DarkRed);

                    spriteBatch.Draw(t, player.footArea, Color.White);

                }
                powerHitter.draw(spriteBatch, GraphicsDevice, informFont);
                 skeleton.draw(spriteBatch, GraphicsDevice, informFont);
                player.Draw(spriteBatch);
                bat.draw(spriteBatch, GraphicsDevice, informFont);
                lance.draw(spriteBatch, GraphicsDevice, informFont2, lance.color);
             
                spriteBatch.DrawString(informFont, "FPS: " + fpsFont, new Vector2(fontPosition.X, fontPosition.Y - 40), Color.DarkRed);
            }
           
           
       
           
            allGui.Draw(spriteBatch);

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
