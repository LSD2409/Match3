using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Match3
{
    static class Art
    {
        public static Texture2D Background { get; private set; }
        public static Texture2D Bomb { get; private set; }
        public static Texture2D BombSelected { get; private set; }
        public static SpriteFont Font { get; private set; }

        public static List<Texture2D> Type1 = new List<Texture2D>();
        public static List<Texture2D> Type2 = new List<Texture2D>();
        public static List<Texture2D> Type3 = new List<Texture2D>();
        public static List<Texture2D> Type4 = new List<Texture2D>();
        public static List<Texture2D> Type5 = new List<Texture2D>();


        public static void Load(ContentManager content)
        {
            Background = content.Load<Texture2D>("Art/background1");
            Bomb = content.Load<Texture2D>("Art/Bonuse/Bomb/Bomb");
            BombSelected = content.Load<Texture2D>("Art/Bonuse/Bomb/BombSelected");
            Font = content.Load<SpriteFont>("Art/Font");

            for(int j = 1; j < 61; j++)
            {
                Type1.Add(content.Load<Texture2D>($"Art/GameElement/Type1/{j}"));
                Type2.Add(content.Load<Texture2D>($"Art/GameElement/Type2/{j}"));
                Type3.Add(content.Load<Texture2D>($"Art/GameElement/Type3/{j}"));
                Type4.Add(content.Load<Texture2D>($"Art/GameElement/Type4/{j}"));
                Type5.Add(content.Load<Texture2D>($"Art/GameElement/Type5/{j}"));
            }
        }
    }
}
