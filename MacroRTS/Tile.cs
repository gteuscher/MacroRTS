using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MacroRTS
{
    class Tile
    {
        //use public vs setter/getter on texture?
        public Texture2D tileTexture;
        public Vector2 pos;
        public int size = 128;

        public Tile(Texture2D t)
        {
            tileTexture = t;
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(tileTexture, pos, Color.White);
        }

        public Rectangle GetCoords()
        {
            Rectangle tileLoc = new Rectangle((int)pos.X, (int)pos.Y, size, size);

            return tileLoc;
        }


    }
}
