using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Defender.code
{
    internal class Asteroidy
    {
        public static int Width, Height;
        public static Random rnd = new Random();
        static public SpriteBatch SpriteBatch { get; set; }
        static Star[] stars;

        static public int GetIntRnd(int min, int max)
        {
            return rnd.Next(min, max);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        static public void Init(SpriteBatch spriteBatch, int Width, int Height)
        {
            Asteroidy.Width = Width;
            Asteroidy.Height = Height;
            Asteroidy.SpriteBatch = spriteBatch;
            stars = new Star[50]; //кол-во звезд
            for (int i = 0; i < stars.Length; i++)
                stars[i] = new Star(new Vector2(-rnd.Next(1, 10), 0)); // направление
        }

        static public void Draw()
        {
            foreach (Star star in stars)
                star.Draw();
        }

        static public void Update()
        {
            foreach (Star star in stars)
                star.Update();
        }

    }

    class Star
    {
        Vector2 Pos;
        Vector2 Dir;
        Color color;

        public static Texture2D Texture2D { get; set; }

        public Star(Vector2 Pos, Vector2 Dir)
        {
            this.Pos = Pos;
            this.Dir = Dir;
        }

        public Star(Vector2 Dir)
        { 
            this.Dir = Dir;
            RandomSet();
        }

        public void Update()
        {
            Pos += Dir;
            if (Pos.X < 0)
            {
                RandomSet();
            }
        }

        public void RandomSet()
        {
            Pos = new Vector2(Asteroidy.GetIntRnd(Asteroidy.Width, Asteroidy.Width + 300), Asteroidy.GetIntRnd(0, Asteroidy.Height));
            color = Color.FromNonPremultiplied(Asteroidy.GetIntRnd(0, 256), Asteroidy.GetIntRnd(0, 256), Asteroidy.GetIntRnd(0, 256), 255);
        }

        public void Draw()
        {
            Asteroidy.SpriteBatch.Draw(Texture2D, Pos, color);
        }
    }
}
