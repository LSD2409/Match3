using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3
{
    public class Match3 : Game
    {
        private GraphicsDeviceManager Graphics { get; set; }
        private SpriteBatch SpriteBatch { get; set; }

        public bool IsBlocked { get; set; }

        public Match3()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = WindowSetting.Width;
            Graphics.PreferredBackBufferHeight = WindowSetting.Height;
        }

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Art.Load(Content);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            if(GameStatus.Level == Level.Menu)
            {
                Input.Update(gameTime);
                Button.Update();   
            }
            else if(GameStatus.Level == Level.Game)
            {
                GameStatus.Update(gameTime);
                EntityManager.Update(gameTime);
            }
            else if(GameStatus.Level == Level.GameOver)
            {
                Input.Update(gameTime);
                Button.Update();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (GameStatus.Level == Level.Menu)
            {
                SpriteBatch.Begin(SpriteSortMode.FrontToBack);
                SpriteBatch.Draw(Art.Background, Vector2.Zero, Color.White);
                Button.Draw(SpriteBatch);
                SpriteBatch.End();
            }
            else if (GameStatus.Level == Level.Game)
            {
                SpriteBatch.Begin(SpriteSortMode.FrontToBack);
                SpriteBatch.Draw(Art.Background, Vector2.Zero, Color.White);
                EntityManager.Draw(SpriteBatch);
                GameStatus.Draw(SpriteBatch);
                SpriteBatch.End();
            }
            else if (GameStatus.Level == Level.GameOver)
            {
                SpriteBatch.Begin(SpriteSortMode.FrontToBack);
                SpriteBatch.Draw(Art.Background, Vector2.Zero, Color.White);
                Button.Draw(SpriteBatch);
                SpriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
