using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pathfinder
{
    public class Game1 : Game
    {
        // Assets
        public static Texture2D PIXEL;
        public static SpriteFont gameplayFont;
        public static SpriteFont pixelFont;

        private GraphicsDeviceManager _graphics;
        private Grid _grid;
        private SpriteBatch _spriteBatch;
        private ToolSelect _toolSelect;
        private Button _clearButton;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _grid = new Grid(40, 40, new Vector2(32, 55), 10);
            _toolSelect = new ToolSelect(_grid);
            _clearButton = new Button("Clear", new Vector2(680, 55), Color.Red, 60, 25);
            _clearButton.onClick += delegate { _grid.FillArray(0); };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            gameplayFont = Content.Load<SpriteFont>("gameplayFont");
            pixelFont = Content.Load<SpriteFont>("silomFont");

            PIXEL = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
            PIXEL.SetData(new[] {Color.White});
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            _grid.Update(gameTime);
            _toolSelect.Update(gameTime);
            _clearButton.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _grid.Draw(_spriteBatch);

            _toolSelect.Draw(_spriteBatch);
            _clearButton.Draw(_spriteBatch);

            _spriteBatch.DrawString(pixelFont, $"Simple A* Pathfinder", new Vector2(32, 25), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}