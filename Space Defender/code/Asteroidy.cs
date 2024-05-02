using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata;
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
        static List<Fire> fires = new List<Fire>();
        static List<Asteroid> asteroids = new List<Asteroid>();
        static List<Nlo> nlos = new List<Nlo>();

        static public List<string> list_test = new List<string>() {
                "asteroid",
                "nlo"
            };


        
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
        /// отвечает за создание нового снаряда (объекта Fire) и его добавление в коллекцию fires при вызове метода ShipFire.
        /// </summary>
        static public void ShipFire()
        {
            fires.Add(new Fire(StarShip.GetPosForFire));
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
            for (int i = 0; i < 10; i++)
                asteroids.Add(new Asteroid());
                nlos.Add(new Nlo());

        }

        /// <summary>
        /// отрисовка астероидов
        /// </summary>
        static public void Draw()
        {
            foreach (Star star in stars)
                star.Draw();
            foreach (Fire fire in fires)
                fire.Draw();
            StarShip.Draw();
            foreach (Asteroid asteroid in asteroids)
                asteroid.Draw();
            foreach(Nlo nlo in nlos)
                nlo.Draw();
        }

        /// <summary>
        /// обновление положения астероидов
        /// </summary>
        static public void Update()
        {
            foreach (Star star  in stars)
                star.Update();
            foreach (Asteroid asteroid in asteroids)
                asteroid.Update();
            for (int i = 0; i < fires.Count; i++)
            {
                fires[i].Update();
                Asteroid asteroidCrash = fires[i].Crash(asteroids);
                Nlo nlosCrash = fires[i].Crash(nlos);
                if (asteroidCrash != null)
                {
                    asteroids.Remove(asteroidCrash);
                    fires.RemoveAt(i);
                    asteroids.Add(new Asteroid());
                    i--;
                    continue;
                }
                if (nlosCrash != null)
                {
                    nlos.Remove(nlosCrash);
                    fires.RemoveAt(i);
                    nlos.Add(new Nlo());
                    i--;
                    continue;
                }
                if (fires[i].Hidden)
                {
                    fires.RemoveAt(i);
                    i--;
                }
                //if (fires[i].)
            }

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



    /// <summary>
    /// отвечает за движение и отображение снаряда на экране в игре
    /// </summary>
    class Fire
    {
        Vector2 Pos;
        Vector2 Dir;
        const int speed = 5; //скрость выстрела 
        Color color = Color.White;

        /// <summary>
        /// текстура
        /// </summary>
        public static Texture2D Texture2D { get; set; }

        /// <summary>
        /// конструктор со всеми параметрами
        /// </summary>
        /// <param name="Pos">позиция снаряда</param>
        public Fire(Vector2 Pos)
        {
            this.Pos = Pos;
            this.Dir = new Vector2(speed, 0); //задаем скорость только по Х
        }

        public Asteroid Crash(List<Asteroid> asteroids)
        {
            foreach (Asteroid asteroid in asteroids)
                if (asteroid.IsIntersect(new Rectangle((int)Pos.X, (int)Pos.Y, Texture2D.Width + 100, Texture2D.Height + 100))) return asteroid;
            return null;
        }
        public Nlo Crash(List<Nlo> nlos)
        {
            foreach (Nlo nlo in nlos)
                if (nlo.IsIntersect(new Rectangle((int)Pos.X, (int)Pos.Y, Texture2D.Width + 100, Texture2D.Height + 100))) return nlo;
            return null;
        }
        /// <summary>
        ///  свойство Hidden, которое возвращает логическое значение (true или false) в зависимости от условия, заданного в блоке get.свойство Hidden 
        ///  будет возвращать true, если объект находится за пределами экрана справа (координата X больше ширины экрана), и false, 
        ///  если объект находится в пределах экрана.
        /// </summary>
        public bool Hidden
        {
            get
            {
                return Pos.X >= Asteroidy.Width;
            }
        }

        /// <summary>
        /// обновление положение снаряда
        /// </summary>
        public void Update()
        {
            if (Pos.X <= Asteroidy.Width)
            {
                Pos += Dir;
            }
        }


        /// <summary>
        /// отрисовка снаряда
        /// </summary>
        public void Draw()
        {
            Asteroidy.SpriteBatch.Draw(Texture2D, Pos, color);
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
        /// <param name="Pos">позиция корабля</param>
        public StarShip(Vector2 Pos)
        {
            this.Pos = Pos;

        }

        /// <summary>
        /// свойство GetPosForFire, которое возвращает новый объект Vector2, который является результатом сложения текущих координат 
        /// </summary>
        public Vector2 GetPosForFire => new Vector2(Pos.X + 260, Pos.Y + 90);
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
            if (this.Pos.Y < Asteroidy.Height - Texture2D.Height) this.Pos.Y += Speed;
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
            if (this.Pos.X < Asteroidy.Width - Texture2D.Width) this.Pos.X += Speed;

        }
        /// <summary>
        /// отрисовка корабля
        /// </summary>
        public void Draw()
        {
            Asteroidy.SpriteBatch.Draw(Texture2D, Pos, color);
        }
    }

    class Asteroid
    {
        Vector2 Pos;

        Vector2 Dir;
        Vector2 center;
        float scale;
        Point size;

        Color color = Color.White;

        float spinSpeed = 1;
        float rotation;

        public static Texture2D Texture2D { get; set; }

        public bool IsIntersect(Rectangle rectangle)
        {
            return rectangle.Intersects(new Rectangle((int)Pos.X, (int)Pos.Y, size.X, size.Y));
        }

        public Asteroid()
        {
            RandomSet();
        }

        public Asteroid(Vector2 Pos, Vector2 Dir, float Scale, float SpinSpeed)
        {
            this.Pos = Pos;
            this.Dir = Dir;
            this.scale = Scale;
            this.spinSpeed = SpinSpeed;
            center = new Vector2(Texture2D.Width / 2, Texture2D.Height / 2);
            rotation = 0;
            size = new Point((int)(Texture2D.Width * scale), (int)(Texture2D.Height * scale));
        }

        public Asteroid(Vector2 Dir)
        {
            this.Dir = Dir;
            RandomSet();
        }

        public void Update()
        {
            Pos += Dir;
            rotation += spinSpeed;
            if (Pos.X < -100)
            {
                RandomSet();
            }
        }

        public void RandomSet()
        {
            Pos = new Vector2(Asteroidy.GetIntRnd(Asteroidy.Width, Asteroidy.Width + 300), Asteroidy.GetIntRnd(0, Asteroidy.Height));
            Dir = new Vector2(-(float)Asteroidy.rnd.NextDouble() * 2 + 0.1f, 0f);
            spinSpeed = (float)(Asteroidy.rnd.NextDouble() - 0.5) / 4;
            scale = (float)Asteroidy.rnd.NextDouble();
            center = new Vector2(Texture2D.Width / 2, Texture2D.Height / 2);
            size = new Point((int)(Texture2D.Width * scale), (int)(Texture2D.Height * scale));
            //Size = new Point(Asteroids.GetIntRnd(10, 20), Asteroids.GetIntRnd(20,40));
            //color = Color.FromNonPremultiplied(Asteroids.GetIntRnd(0, 256), Asteroids.GetIntRnd(0, 256), Asteroids.GetIntRnd(0, 256), Asteroids.GetIntRnd(0, 256));
        }

        public void Draw()
        {
            Asteroidy.SpriteBatch.Draw(Texture2D, Pos, null, color, rotation, center, scale, SpriteEffects.None, 0);
        }
    }


    class Nlo
    {
        Vector2 Pos;

        Vector2 Dir;
        Vector2 center;
        float scale;
        Point size;

        Color color = Color.White;

        float spinSpeed = 1;
        float rotation;

        public static Texture2D Texture2D { get; set; }

        public bool IsIntersect(Rectangle rectangle)
        {
            return rectangle.Intersects(new Rectangle((int)Pos.X, (int)Pos.Y, size.X, size.Y));
        }

        public Nlo()
        {
             RandomSet();
        }

        public Nlo(Vector2 Pos, Vector2 Dir, float Scale, float SpinSpeed)
        {
        this.Pos = Pos;
        this.Dir = Dir;
        this.scale = Scale;
        this.spinSpeed = SpinSpeed;
        center = new Vector2(Texture2D.Width / 2, Texture2D.Height / 2);
        rotation = 0;
        size = new Point((int)(Texture2D.Width * scale), (int)(Texture2D.Height * scale));
        }

         public Nlo(Vector2 Dir)
         {
            this.Dir = Dir;
            RandomSet();
         }

        public void Update()
        {
            Pos += Dir;
            rotation += spinSpeed;
            if (Pos.X < -100)
            {
                RandomSet();
            }
        }

        public void RandomSet()
        {
            Pos = new Vector2(Asteroidy.GetIntRnd(Asteroidy.Width, Asteroidy.Width + 300), Asteroidy.GetIntRnd(0, Asteroidy.Height));
            Dir = new Vector2(-(float)Asteroidy.rnd.NextDouble() * 2 + 0.1f, 0f);
            spinSpeed = (float)(Asteroidy.rnd.NextDouble() - 0.5) / 4;
            scale = (float)Asteroidy.rnd.NextDouble();
            center = new Vector2(Texture2D.Width / 2, Texture2D.Height / 2);
            size = new Point((int)(Texture2D.Width * scale), (int)(Texture2D.Height * scale));
            //Size = new Point(Asteroids.GetIntRnd(10, 20), Asteroids.GetIntRnd(20,40));
            //color = Color.FromNonPremultiplied(Asteroids.GetIntRnd(0, 256), Asteroids.GetIntRnd(0, 256), Asteroids.GetIntRnd(0, 256), Asteroids.GetIntRnd(0, 256));
        }

        public void Draw()
        {
            Asteroidy.SpriteBatch.Draw(Texture2D, Pos, null, color, rotation, center, scale, SpriteEffects.None, 0);
        }
    }
}