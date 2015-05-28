using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MacroRTS
{
    class Tile
    {
        public Texture2D tileTexture;
        public Vector2 pos;

        public Tile(Texture2D tileTexture)
        {
            this.tileTexture = tileTexture;
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(tileTexture, pos, Color.White);
        }

    }
}
