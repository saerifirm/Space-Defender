﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Defender.code
{
    /// <summary>
    /// отображение заставки
    /// </summary>
    static class SplashScreen
    {
        public static Texture2D Background { get; set; }
        static int TimeCounter = 0;
        static Color color;
        public static SpriteFont Font { get; set; }
        static Vector2 TextPosition = new Vector2(100, 100);

        /// <summary>
        /// отрисовка засавки
        /// </summary>
        /// <param name="spriteBatch">объект для отрисовки</param>
        static public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, Vector2.Zero, Color.White);
            spriteBatch.DrawString(Font, "Космический Защитник!", TextPosition, color);
        }

        /// <summary>
        /// обновление заставки
        /// </summary>
        static public void Update()
        {
            color = Color.FromNonPremultiplied(255, 255, 255, TimeCounter % 256);
            TimeCounter++;
        }
    }
}
