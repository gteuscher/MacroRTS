﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MacroRTS
{
    //TODO: Create unit with its own draw method and genericize later.
    class Unit
    {
        //animation
        public Texture2D spriteSheet;
        private float time;
        private float frameTime = 0.2f;
        int frameIndex;
        const int totalFrames = 5;
        private int frameHeight = 30;
        private int frameWidth = 30;

        public Vector2 pos;
        private int size = 30;

        public Unit(Texture2D u)
        {
            spriteSheet = u;
            pos = new Vector2(-100, -100);
        }

        public void Draw(SpriteBatch s, GameTime g)
        {
            time += (float)g.ElapsedGameTime.TotalSeconds;
            while (time > frameTime)
            {
                frameIndex++;

                time = 0f;
            }
            if (frameIndex > totalFrames) frameIndex = 1;
            Rectangle source = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);
            Vector2 origin = new Vector2(frameWidth / 2.0f, frameHeight);

            s.Draw(spriteSheet, pos, source, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }

        public Rectangle GetCoords()
        {
            Rectangle tileLoc = new Rectangle((int)pos.X, (int)pos.Y, size, size);

            return tileLoc;
        }

        public Vector2 FindNearestTower(List<Vector2> tl) {
            Vector2 nearestPos = new Vector2();
            float prevDistance = 0.0f;
            float thisDistance = 0.0f;
            foreach(Vector2 t in tl)
            {
                thisDistance = Vector2.Distance(pos,t);

                if (thisDistance < prevDistance || prevDistance == 0.0f)
                {
                    nearestPos = t;
                }

                prevDistance = thisDistance;
            }

            return nearestPos;
        }
    }
}
