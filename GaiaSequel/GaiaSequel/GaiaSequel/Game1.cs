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
        MapReader mapDesigner;
        SpriteFont informFont;
        Vector2 fontPosition;
        Camera cam;
        
        
        KeyboardState prevState = Keyboard.GetState();

        bool hidePrint = false;
        bool hideMap = false;
        bool normal = false;
        bool stats = false;
        bool needDarkBack = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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

          
            //Load the main player
            player = new MainPlayer(Content.Load<Texture2D>("Sprites/Will"), new Vector2(160, 120));

            //startUpMapBuilder
           //mapDesigner = new MapReader(Content.Load<Texture2D>("Maps/GTILES"));
            mapDesigner = new MapReader(Content.Load<Texture2D>("Maps/GTILES32"));
            
            //This tells the camera to follow the player
            cam = new Camera(graphics.GraphicsDevice.Viewport, Vector2.Zero);
            cam.Follow(player); 
            
            //load font and set position
            informFont = Content.Load<SpriteFont>("Text2");
            
            //Load first level
            mapDesigner.init();
            mapDesigner.buildMap("Content/Maps/level",0);

            //Connect the map to the main player
            player.connectMap(mapDesigner);
            
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
            KeyboardState current = Keyboard.GetState();
            // Allows the game to exit
            if (current.IsKeyDown(Keys.Back) && prevState.IsKeyUp(Keys.Back))
                this.Exit();

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

            //update the previous keystate
            prevState = current;

            //update the player
            player.update(current, gameTime);
            
            //update the camera
            cam.Update(gameTime);
                
            //also update the fonts position
            fontPosition = new Vector2(player.position.X + 100, player.position.Y - 100);

            // TODO: Add your update logic here

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

            
            //draws debugger information
            if (!hideMap)
            {
                mapDesigner.Draw(spriteBatch,normal);
            }
            if (!hidePrint){
                var t = new Texture2D(GraphicsDevice, 1, 1);
                t.SetData(new[] { Color.White });
           
                spriteBatch.DrawString(informFont, player.posString, fontPosition, Color.DarkRed);
                spriteBatch.DrawString(informFont, player.groundString, new Vector2(fontPosition.X, fontPosition.Y + 20), Color.DarkRed);
                spriteBatch.DrawString(informFont, player.jumpingString, new Vector2(fontPosition.X, fontPosition.Y + 40), Color.DarkRed);
                spriteBatch.DrawString(informFont, player.jumpVectorString, new Vector2(fontPosition.X, fontPosition.Y + 60), Color.DarkRed);
                spriteBatch.Draw(t, player.footArea, Color.White);    
            }
            if (!stats){
                spriteBatch.DrawString(informFont, player.simpleGui, new Vector2(fontPosition.X - 400, fontPosition.Y - 80), Color.Gold);
            }

            spriteBatch.Draw(player.spriteSheet, player.destRect, player.sourceRect, Color.White);
            


            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
