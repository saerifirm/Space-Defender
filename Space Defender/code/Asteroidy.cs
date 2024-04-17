using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Defender.code;

namespace Space_Defender.code
{
    /// <summary>
    /// создание и управление астероидами
    /// </summary>
    internal class Asteroidy
    {
        public static int Width, Height;
        public static Random rnd = new Random();
        static public SpriteBatch SpriteBatch { get; set; }
        static Star[] stars;
        static public StarShip StarShip { get; set; }

        /// <summary>
        /// получение целого случайного числа в заданном диапазоне
        /// </summary>
        /// <param name="min">минимальное значение</param>
        /// <param name="max">максимальное значение</param>
        /// <returns>случайное целое число</returns>
        static public int GetIntRnd(int min, int max)
        {
            return rnd.Next(min, max);
        }

        /// <summary>
        /// инициализация астероидов
        /// </summary>
        /// <param name="spriteBatch">объект для отрисовки</param>
        /// <param name="Width">ширина объекта</param>
        /// <param name="Height">высота объекта</param>
        static public void Init(SpriteBatch spriteBatch, int Width, int Height)
        {
            Asteroidy.Width = Width;
            Asteroidy.Height = Height;
            Asteroidy.SpriteBatch = spriteBatch;
            stars = new Star[100]; //кол-во звезд
            for (int i = 0; i < stars.Length; i++)
                stars[i] = new Star(new Vector2(-rnd.Next(1, 6), 0)); // направление
            StarShip = new StarShip(new Vector2(0, Height / 2 - 140));
        }

        /// <summary>
        /// отрисовка астероидов
        /// </summary>
        static public void Draw()
        {
            foreach (Star star in stars)
                star.Draw();
            StarShip.Draw();
        }

        /// <summary>
        /// обновление положения астероидов
        /// </summary>
        static public void Update()
        {
            foreach (Star star in stars)
                star.Update();
        }

    }

    /// <summary>
    /// представление звезд
    /// </summary>
    class Star
    {
        Vector2 Pos;
        Vector2 Dir;
        Color color;

        /// <summary>
        /// текстура
        /// </summary>
        public static Texture2D Texture2D { get; set; }

        /// <summary>
        /// конструктор со всеми параметрами
        /// </summary>
        /// <param name="Pos">позиция звезды</param>
        /// <param name="Dir">направление движения </param>
        public Star(Vector2 Pos, Vector2 Dir)
        {
            this.Pos = Pos;
            this.Dir = Dir;
        }

        /// <summary>
        /// конструктор с направлением движения
        /// </summary>
        /// <param name="Dir">направление движения звезды</param>
        public Star(Vector2 Dir)
        { 
            this.Dir = Dir;
            RandomSet();
        }

        /// <summary>
        /// обновление положение звезды
        /// </summary>
        public void Update()
        {
            Pos += Dir;
            if (Pos.X < 0)
            {
                RandomSet();
            }
        }

        /// <summary>
        /// генерация случайных параметров звезды
        /// </summary>
        public void RandomSet()
        {
            Pos = new Vector2(Asteroidy.GetIntRnd(Asteroidy.Width, Asteroidy.Width + 300), Asteroidy.GetIntRnd(0, Asteroidy.Height));
            color = Color.FromNonPremultiplied(Asteroidy.GetIntRnd(0, 256), Asteroidy.GetIntRnd(0, 256), Asteroidy.GetIntRnd(0, 256), 255);
        }

        /// <summary>
        /// отрисовка звезды
        /// </summary>
        public void Draw()
        {
            Asteroidy.SpriteBatch.Draw(Texture2D, Pos, color);
        }
    }
}

/// <summary>
/// движение космического корабля
/// </summary>
class StarShip
{
    Vector2 Pos;
    public int Speed { get; set; } = 5;
    
    Color color = Color.White;

    /// <summary>
    /// текстура
    /// </summary>
    public static Texture2D Texture2D { get; set; }

    /// <summary>
    /// конструктор со всеми параметрами
    /// </summary>
    /// <param name="Pos">позиция rjhf,kz</param>
    public StarShip(Vector2 Pos)
    {
        this.Pos = Pos;
        
    }

    /// <summary>
    /// движение вверх
    /// </summary>
    public void Up()
    {
        if (this.Pos.Y > 0) this.Pos.Y -= Speed;
    }

    /// <summary>
    /// движение вниз
    /// </summary>
    public void Down()
    {
        if (this.Pos.Y < Asteroidy.Height-220) this.Pos.Y += Speed;
    }

    /// <summary>
    /// движение влево
    /// </summary>
    public void Left()
    {
        if (this.Pos.X > 0) this.Pos.X -= Speed;
       
    }

    /// <summary>
    /// движение вправо
    /// </summary>
    public void Right()
    {
        if (this.Pos.X < Asteroidy.Width - 240) this.Pos.X += Speed;
        
    }
    /// <summary>
    /// отрисовка корабля
    /// </summary>
    public void Draw()
    {
        Asteroidy.SpriteBatch.Draw(Texture2D, Pos, color);
    }
}

