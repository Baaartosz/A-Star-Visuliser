using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pathfinder
{
    public class ToolSelect
    {
        private Grid _grid;
        private Button left;
        private Button right;
        private String currentToolSelected; // might not be needed if index is switched 
        private int toolIndex = 1;
        
        /*
         * 0 ERASER
         * 1 FINDER
         * 2 GOAL
         * 3 WALL
         */
        
        public ToolSelect(Grid grid)
        {
            _grid = grid;
            left = new Button("<", new Vector2(450, 55), Color.Orange, 25, 25);
            right = new Button(">", new Vector2(480, 55), Color.Orange, 25, 25);
            
            left.onClick += DecreaseOnClick;
            right.onClick += IncreaseOnClick;
        }
        
        private void DecreaseOnClick(object? sender, EventArgs e)
        {
            if(toolIndex > 0)
                toolIndex--;
        }

        private void IncreaseOnClick(object? sender, EventArgs e)
        {
            if(toolIndex < 3)
                toolIndex++;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            left.Draw(spriteBatch);
            right.Draw(spriteBatch);
            currentToolSelected = toolIndex switch
            {
                0 => "ERASER",
                1 => "FINDER",
                2 => "GOAL",
                3 => "WALL",
                _ => currentToolSelected
            };
            spriteBatch.DrawString(Game1.pixelFont, currentToolSelected, new Vector2(515, 58), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            left.Update(gameTime);
            right.Update(gameTime);

            _grid.SetTool(toolIndex);
        }
    }
}