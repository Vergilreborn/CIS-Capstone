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
       
        //Declaring our variables
        MainPlayer player;
        AllGui allGui;
        Enemy skeleton;
        Enemy bat;
        MapReader mapDesigner;
        SpriteFont informFont;
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
        char gameState;
        MediaPlayerGaia mediaPlayer;

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

          
            //Load the main player
            player = new MainPlayer(Content.Load<Texture2D>("Sprites/Will"), new Vector2(160, 120));
            skeleton = new Enemy(10, 2, 3, new Vector2(60, 50), 72, 120, false, Content.Load<Texture2D>("Sprites/Enemy/Skeleton"),true,rand);
            bat = new Enemy(10, 2, 3, new Vector2(70, 70), 80, 80, true, Content.Load<Texture2D>("Sprites/Enemy/Bat"),true,rand);
          
            //startUpMapBuilder 
            mapDesigner = new MapReader(Content.Load<Texture2D>("Maps/GTILES32"));
            
            //This tells the camera to follow the player as well as setting up the camera
            cam = new Camera(graphics.GraphicsDevice.Viewport, Vector2.Zero);
            cam.Follow(player); 
            
            //load font and set position
            informFont = Content.Load<SpriteFont>("Text2");

            
            mediaPlayer.init(Content);

            
            //loads the music and plays it automatically
           
         
            //Load first level
            mapDesigner.init();
            mapDesigner.buildMap("Content/Maps/level",0);

            //Connect the map to the main player and temporary enemies
            player.connectMap(mapDesigner,rand);
            skeleton.connectMap(mapDesigner);
            bat.connectMap(mapDesigner);

            //sets the game state to opening screen
            //stats - 't' for title
            //        'o' for openingScreen
            //        'p' for paused
            //        'm' for menu
            //        'a' for active
            //        'l' for load/save screen
            //        'v' for video playing 
            gameState = 't';

            //sets up the Hud and all the graphics any menu
            allGui = new AllGui(cam,player,Content.Load<Texture2D>("Sprites/Gui/PlayerGui")
                                                                            ,Content.Load<Texture2D>("Sprites/Gui/Transformation2")
                                                                            ,Content.Load<Texture2D>("Sprites/Gui/characters2"),
                                                                            Content.Load<Texture2D>("Sprites/Gui/TitleScreen"),
                                                                            Content.Load<Texture2D>("Sprites/Gui/IntroScreenSetup")
                                                                            ,Content.Load<Texture2D>("Sprites/Gui/life"));
            
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
            if (current.IsKeyDown(Keys.A) && prevState.IsKeyUp(Keys.A))
                if (gameState == 't')
                    gameState = 'o';
                else
                    gameState = 't';
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
                    player.health++;
            if (current.IsKeyDown(Keys.J) && prevState.IsKeyUp(Keys.J))
                if (1 < player.health)
                    player.health--;

            if (current.IsKeyDown(Keys.B) && prevState.IsKeyUp(Keys.B))
                if (player.mana < player.maxMana)
                    player.mana++;
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
                else
                {
                    playerSwitch = 0;
                    cam.Follow(bat);
                
                }

            

            if (gameState != 't')
            {
                mediaPlayer.playNew(1);

                if (!paused)
                {

                    if (playerSwitch == 1)
                    {
                        center = skeleton.center;

                        player.update(new KeyboardState(), gameTime);
                        skeleton.update(current, gameTime, player.position);
                        bat.update(new KeyboardState(), gameTime, player.position);
                    }
                    else if (playerSwitch == 0)
                    {
                        center = player.center;
                        player.update(current, gameTime);
                        skeleton.update(new KeyboardState(), gameTime, player.position);
                        bat.update(new KeyboardState(), gameTime, player.position);
                    }
                    else
                    {
                        center = bat.center;
                        bat.update(current, gameTime, player.position);
                        skeleton.update(new KeyboardState(), gameTime, player.position);

                        player.update(new KeyboardState(), gameTime);
                    }
                    mediaPlayer.resume();




                }
                else
                    mediaPlayer.pause();
            }
            else
                mediaPlayer.playNew(0);



            

                
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

            if (gameState != 't')
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
                if (!stats)
                {
                    spriteBatch.DrawString(informFont, player.simpleGui, new Vector2(fontPosition.X - 400, fontPosition.Y - 80), Color.Gold);
                }
                skeleton.draw(spriteBatch, GraphicsDevice, informFont);
                spriteBatch.Draw(player.spriteSheet, player.destRect, player.sourceRect, Color.White);
                bat.draw(spriteBatch, GraphicsDevice, informFont);
                spriteBatch.DrawString(informFont, "FPS: " + fpsFont, new Vector2(fontPosition.X, fontPosition.Y - 40), Color.DarkRed);
            }
           
           
       
           
            allGui.Draw(spriteBatch);

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
