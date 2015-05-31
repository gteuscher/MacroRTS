using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MacroRTS
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D[] bgTextures;
        public Texture2D animTexture;
        private Tower[,] gameboard;
        private Random random = new Random();
        private List<Unit> u;
        
        private Vector2 mouseLocation;
        private MouseState oldMouseState;

        private Texture2D selectedBuilding;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            bgTextures = new Texture2D[2];
            gameboard = new Tower[7, 4];
            graphics.PreferredBackBufferWidth = 896;
            graphics.PreferredBackBufferHeight = 512;
            u = new List<Unit>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();
            IsMouseVisible = true;
            for (int i = 0; i < gameboard.GetLength(0); i++)
            {
                for (int j = 0; j< gameboard.GetLength(1); j++)
                {
                    //TODO: Default loading should use tile, not tower
                    gameboard[i, j] = new Tower(bgTextures[0], bgTextures[0]);
                    gameboard[i, j].pos = new Vector2(i * gameboard[i,j].size, j * gameboard[i, j].size);
                }
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            animTexture = Content.Load<Texture2D>("walking");
            bgTextures[0] = Content.Load<Texture2D>("blank");
            bgTextures[1] = Content.Load<Texture2D>("tower");

            //TODO: Make this based on some selectable options
            selectedBuilding = bgTextures[1];


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouseState = Mouse.GetState();
            mouseLocation.X = mouseState.X;
            mouseLocation.Y = mouseState.Y;

            if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != mouseState.LeftButton)
            {

                for (int i = 0; i < gameboard.GetLength(0); i++)
                {
                    for (int j = 0; j < gameboard.GetLength(1); j++)
                    {
                        Tower t = gameboard[i, j];
                        if (t.GetCoords().Contains(mouseState.X, mouseState.Y))
                        {
                            //test condition for damage
                            if (t.IsAlive())
                            {
                                t.Damage(8);
                            } else
                            {
                                Tower tow = new Tower(selectedBuilding, bgTextures[0]);
                                tow.pos = new Vector2(t.GetCoords().X, t.GetCoords().Y);
                                tow.Init(100, 50);
                                gameboard[i, j] = tow;
                            }
                            
                        }
                    }                        
                }
            }

            if (mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != mouseState.RightButton)
            {
                Unit tempUnit = new Unit(animTexture);
                tempUnit.pos = new Vector2(mouseState.X, mouseState.Y);
                u.Add(tempUnit);
            }

            oldMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            for (int i = 0; i < gameboard.GetLength(0); i++)
            {
                for (int j = 0; j < gameboard.GetLength(1); j++)
                {
                    gameboard[i, j].Draw(spriteBatch);
                }
            }

            for(int i = 0; i < u.Count; i++)
            {
                if(u[i] != null) { 
                    u[i].Draw(spriteBatch, gameTime);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
