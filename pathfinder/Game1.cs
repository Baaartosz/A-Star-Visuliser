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
        private readonly Grid _grid;
        private SpriteBatch _spriteBatch;
        private readonly ToolSelect _toolSelect;
        private int selectedItem;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            _grid = new Grid(40, 40, new Vector2(32, 55), 10);
            _toolSelect = new ToolSelect(_grid);
            // _testBtn = new Button("<", new Vector2(450, 55), Color.Orange, 25, 25);
            // _testBtn.onClick += TestBtnOnonClick;
            // _testBtn2 = new Button(">", new Vector2(480, 55), Color.Orange, 25, 25);
            // _testBtn2.onClick += TestBtn2OnonClick;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private void TestBtnOnonClick(object? sender, EventArgs e)
        {
            Console.WriteLine(selectedItem);
            selectedItem--;
        }
        
        private void TestBtn2OnonClick(object? sender, EventArgs e)
        {
            Console.WriteLine(selectedItem);
            selectedItem++;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _grid.Draw(_spriteBatch);

            _toolSelect.Draw(_spriteBatch);
            _spriteBatch.DrawString(pixelFont, $"Simple A* Pathfinder {selectedItem}", new Vector2(32, 25), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}