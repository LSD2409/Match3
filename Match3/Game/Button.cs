using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace Match3
{
    static class Button
    {
        private static string Text { get; set; }
        private static Vector2 Position { get; set; }
        private static Rectangle Bounds { get; set; }
        private static SpriteFont Font { get; set; }

        static Button()
        {
            Font = Art.Font;
            
            Bounds = new Rectangle();
        }


        public static void Update()
        {
            if (GameStatus.Level == Level.Menu)
            {
                Text = "Play";
                Position = new Vector2(WindowSetting.Width / 2 - Text.Length * 10, WindowSetting.Height / 2 - Text.Length * 10);
                Bounds = new Rectangle((int)Position.X, (int)Position.Y, Text.Length * 15, Text.Length * 15);
            }
            else
            {
                Text = $"Game Over\nScores: {GameStatus.Scores}\n      Ok";
                Position = new Vector2(WindowSetting.Width / 2 - Text.Length * 3, WindowSetting.Height / 2 - Text.Length * 3);
                Bounds = new Rectangle((int)Position.X, (int)Position.Y, Text.Length * 10, Text.Length * 10);
            }
            if(Input.WasClick())
            {
                if (Bounds.Contains(Input.GetMouseState()))
                {
                    if (GameStatus.Level == Level.GameOver)
                        GameStatus.Level = Level.Menu;
                    else
                    {
                        GameStatus.Level = Level.Game;
                        GameStatus.NewGame();
                    }
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position, Color.Black, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0.5f);
        } 
    }
}
