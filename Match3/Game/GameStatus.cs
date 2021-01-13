using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3
{
    static class GameStatus
    {
        private static SpriteFont Font = Art.Font;

        public static bool IsGameOver { get; private set; }
        public static double RemainingTime { get; private set; }
        public static int Scores { get; private set; }
        public static Level Level { get; set; }

        private static Vector2 ScorePosition { get; set; }
        private static Vector2 TimePosition { get; set; }

        static GameStatus()
        {
            IsGameOver = false;
            RemainingTime = 60;
            Scores = 0;
            Level = Level.Menu;
            ScorePosition = new Vector2(WindowSetting.Width / 10, WindowSetting.Height / 3);
            TimePosition = new Vector2(WindowSetting.Width -  WindowSetting.Width / 6, WindowSetting.Height / 3);

        }

        public static void Update(GameTime gameTime)
        {
            RemainingTime -= gameTime.ElapsedGameTime.TotalSeconds;

            if (RemainingTime < 0)
                IsGameOver = true;

            if (IsGameOver)
                Level = Level.GameOver;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, $"Scores: {Scores}", ScorePosition, Color.Black, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font, $"Time: {(int)RemainingTime}", TimePosition, Color.Black, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0.5f);
        }

        public static void NewGame()
        {
            IsGameOver = false;
            RemainingTime = 60;
            Scores = 0;
            Level = Level.Game;
            EntityManager.Entities.Clear();
        }

        public static void IncreaseScores(int expiredElements)
        {
            Scores += expiredElements * 10;
        }
    }

    public enum Level
    {
        Menu,
        Game,
        GameOver
    }
}
