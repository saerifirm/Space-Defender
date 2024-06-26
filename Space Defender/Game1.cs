﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Space_Defender.code;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Space_Defender
{
    /// <summary>
    /// определение текущего состояния игры
    /// </summary>
    enum Stat
    {
        SplashScreen,
        Game,
        Final,
        Pause
    }

    /// <summary>
    /// основной класс игры
    /// </summary>
    public class Game1 : Game
    {

        /// <summary>
        /// графика
        /// </summary>
        private GraphicsDeviceManager _graphics;
        /// <summary>
        /// спрайты
        /// </summary>
        private SpriteBatch _spriteBatch;
        Stat Stat = Stat.Game;
        KeyboardState keyboardState, oldKeyboardState;

        private Song song;
        /// <summary>
        /// конструктор класса Game1
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// инициализация игры
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            base.Initialize();
        }



        /// <summary>
        /// загрузка игровых ресурсов
        /// </summary>
        protected override async void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            song = Content.Load<Song>("song");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            SplashScreen.Background = Content.Load<Texture2D>("background");
            SplashScreen.Font = Content.Load<SpriteFont>("SplashFont");
            Star.Texture2D = Content.Load<Texture2D>("star");
            StarShip.Texture2D = Content.Load<Texture2D>("starship");
            Fire.Texture2D = Content.Load<Texture2D>("fire");
            Nlo.Texture2D = Content.Load<Texture2D>("nlo");
            Asteroid.Texture2D = Content.Load<Texture2D>(Asteroidy.list_test[Asteroidy.GetIntRnd(0, 2)]);
            Asteroidy.Init(_spriteBatch, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                
            
            // TODO: use this.Content to load your game content here
        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume -= 0.1f;
        }

        protected override void UnloadContent()
        { 

        }

        /// <summary>
        /// обновление игры
        /// </summary>
        /// <param name="gameTime">время игры</param>
        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            switch (Stat)
            {
                case Stat.Game:
                    Asteroidy.Update();
                    if (keyboardState.IsKeyDown(Keys.Escape))
                        Stat = Stat.SplashScreen;
                    if (keyboardState.IsKeyDown(Keys.Up)) Asteroidy.StarShip.Up();
                    if (keyboardState.IsKeyDown(Keys.Left)) Asteroidy.StarShip.Left();
                    if (keyboardState.IsKeyDown(Keys.Right)) Asteroidy.StarShip.Right();
                    if (keyboardState.IsKeyDown(Keys.Down)) Asteroidy.StarShip.Down();
                    if (keyboardState.IsKeyDown(Keys.LeftControl) && oldKeyboardState.IsKeyUp(Keys.LeftControl)) Asteroidy.ShipFire();
                    break;
                case Stat.SplashScreen:
                    SplashScreen.Update();
                    if (keyboardState.IsKeyDown(Keys.Space))
                        Stat = Stat.Game;
                    break;
                
            }
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape)) Exit();

            // TODO: Add your update logic here
            //SplashScreen.Update();
            oldKeyboardState = keyboardState;
            base.Update(gameTime);

        }

        /// <summary>
        /// отрисовка игры
        /// </summary>
        /// <param name="gameTime">время игры</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            switch(Stat)
            {
                case Stat.SplashScreen:
                    SplashScreen.Draw(_spriteBatch);
                    break;
                case Stat.Game:
                    Asteroidy.Draw();
                    break;
            }

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}