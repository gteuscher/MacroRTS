using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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
        public Texture2D[] animTexture;
        public Texture2D attackTexture;
        private Tower[,] gameboard;
        private Random random = new Random();
        private List<Unit> u;
        private List<Vector2> towerPosition;
        
        private Vector2 mouseLocation;
        private MouseState oldMouseState;

        private Texture2D selectedBuilding;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            animTexture = new Texture2D[2];
            bgTextures = new Texture2D[2];
            gameboard = new Tower[7, 4];
            graphics.PreferredBackBufferWidth = 896;
            graphics.PreferredBackBufferHeight = 512;
            u = new List<Unit>();
            towerPosition = new List<Vector2>();
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

            animTexture[0] = Content.Load<Texture2D>("walking");
            animTexture[1] = Content.Load<Texture2D>("walking-right");
            //TODO: Fix this texture and load into animation array
            attackTexture = Content.Load<Texture2D>("attacking");
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
                            Tower tow = new Tower(selectedBuilding, bgTextures[0]);
                            tow.pos = new Vector2(t.GetCoords().X, t.GetCoords().Y);
                            Vector2 towerPositionForPathfinding = new Vector2(t.GetCoords().X + (tow.size/2), t.GetCoords().Y + (tow.size / 2));
                            tow.Init(100, 50);
                            tow.pathfindingPos = towerPositionForPathfinding;
                            towerPosition.Add(towerPositionForPathfinding);
                            gameboard[i, j] = tow;   
                        }
                    }                        
                }
            }

            if (mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != mouseState.RightButton)
            {
                Unit tempUnit = new Unit(animTexture[0]);
                tempUnit.pos = new Vector2(mouseState.X, mouseState.Y);
                u.Add(tempUnit);
            }

            //unit update management
            foreach (Unit un in u) {
                //find my tower
                if(un.myTower == null)
                {
                    Vector2 nearestLoc = new Vector2();
                    nearestLoc = un.FindNearestTower(towerPosition);
                    un.nearestTower = nearestLoc;
                    if(nearestLoc.X > un.pos.X)
                    {
                        un.spriteSheet = animTexture[1];
                    } else
                    {
                        un.spriteSheet = animTexture[0];
                    }
                    foreach (Tower t in gameboard)
                    {
                        if (un.nearestTower == t.pathfindingPos)
                        {
                            un.myTower = t;
                        }
                    }
                }
                un.MoveToTower(75);
                un.AttackTower(gameTime, attackTexture, animTexture[0]);
            }

            //tower update management
            foreach(Tower t in gameboard)
            {
                if(t.isAlive == false)
                {
                    towerPosition.Remove(new Vector2(t.GetCoords().X + (t.size / 2), t.GetCoords().Y + (t.size / 2)));
                }
                t.spawnSpeed -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (t.spawnSpeed <= 0 && t.isAlive == true)
                {
                    Unit tempUnit= t.spawnUnit(animTexture[0]);
                    u.Add(tempUnit);
                    t.spawnSpeed = 10000;
                    //temp reset unit to spawn in 10 sec
                }
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
