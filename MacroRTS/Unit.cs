using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MacroRTS
{
    class Unit
    {
        //animation
        public Texture2D spriteSheet;
        private float time;
        private float frameTime = 0.2f;
        int frameIndex;
        private int totalFrames = 5;
        private int frameHeight = 30;
        private int frameWidth = 30;
        public int damage = 25;

        double attackSpeed = 1000;

        public Vector2 pos;
        public Vector2 nearestTower;
        public Vector2 ignoredTower;
        private int size = 30;
        bool moving = true;
        bool attacking = false;

        public Tower myTower;

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

        public Vector2 FindNearestTower(List<Vector2> tl)
        {
            Vector2 nearestPos = new Vector2();
            float prevDistance = 0.0f;
            float thisDistance = 0.0f;
            foreach (Vector2 t in tl)
            {
                if (t != ignoredTower)
                { 
                    thisDistance = Vector2.Distance(pos, t);

                    if (thisDistance < prevDistance || prevDistance == 0.0f)
                    {
                        nearestPos = t;
                    }

                    prevDistance = thisDistance;
                }
            }

            nearestTower = nearestPos;
            return nearestPos;
        }


        public void MoveToTower(float speed)
        {
            if (moving == true && nearestTower != new Vector2(0,0))
            {
                Vector2 end = new Vector2(nearestTower.X + 25, nearestTower.Y + 20);
                Vector2 start = pos;
                float elapsed = 0.01f;
                Vector2 direction = Vector2.Normalize(end - start);
                this.pos += direction * speed * elapsed;
                if (Vector2.Distance(start, this.pos) >= Vector2.Distance(start, end))
                {
                    moving = false;
                    attacking = true;
                }
            } 

            if(nearestTower == new Vector2(0, 0))
            {
                myTower = null;
            }
        }

        public void AttackTower(GameTime gt, Texture2D at, Texture2D ot)
        {
            bool isAlive=true;
            attackSpeed -= gt.ElapsedGameTime.TotalMilliseconds;
            if(attacking == true && attackSpeed<= 0 && isAlive == true)
            {
                spriteSheet = at;
                totalFrames = 3;
                frameHeight = 34;
                frameWidth = 40;

                isAlive = myTower.Damage(damage);
                attackSpeed = 1000;
            } 

            if(isAlive == false)
            {
                attacking = false;
                moving = true;
                spriteSheet = ot;
                totalFrames = 5;
                frameHeight = 30;
                frameWidth = 30;
                myTower = null;
            }
        }
    }
}
