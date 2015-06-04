using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MacroRTS
{
    class Tower : Tile
    {
        private Texture2D destroyedTower;
        public int health;
        public int shield;
        public int healthTotal;
        public int shieldTotal;
        public Vector2 pathfindingPos;
        public bool isAlive = true;

        public Tower(Texture2D tileTexture, Texture2D destroyedTower): base (tileTexture)
        {
            this.tileTexture = tileTexture;
            this.destroyedTower = destroyedTower;
        }

        public void Init(int h,int s)
        {
            healthTotal = h;
            shieldTotal = s;
            health = h;
            shield = s;
        }

        public bool Damage(int d)
        {
            int rd = DamageShield(d);
            DamageHealth(rd);
            if (health <= 0)
            {
                isAlive = false;
            }

            Debug.WriteLine("Damage: " + d.ToString() + " Shield: " + shield.ToString() + " Health: " + health.ToString() + " Is Alive: " + isAlive);
            return isAlive;

        }

        private int DamageShield(int d)
        {
            int remainingDamage = d;
            if(shield>0)
            {
                shield -= d;
                if (shield < 0)
                {
                    //calculate non-absorbed damage
                    remainingDamage = Math.Abs(shield);
                    shield = 0;
                } else
                {
                    //shield absorbed all damage
                    remainingDamage = 0;
                }

            }
            return remainingDamage;   
        }

        private void DamageHealth(int d)
        {
            health -= d;
            if (health <= 0)
            {
                Destroy();
            }
        }

        private void Destroy()
        {
            //hardcode destroyed tower
            this.tileTexture = destroyedTower;
        }
    }
}
