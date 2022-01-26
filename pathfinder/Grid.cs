using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pathfinder
{
    // TODO auto-adjust size of cell depending of size of grid area & cell amount. width / cells && height / cells

    public class Grid
    {
        private readonly Color m_BORDER_COLOR = Color.Gray;
        private readonly int m_BORDER_THICKNESS = 2;
        private readonly Color m_BACKGROUND_COLOR = Color.Gray;
        
        private readonly int[] m_cells;
        private readonly int m_cellSize;
        private readonly Vector2 m_position;
        private readonly int m_sizeX;
        private readonly int m_sizeY;

        private int drawTool;

        public Grid(int x, int y, Vector2 position, int cellSize)
        {
            m_sizeX = x;
            m_sizeY = y;
            m_position = position;
            m_cellSize = cellSize;

            m_cells = new int[m_sizeX * m_sizeY];
            FillArray(0);
        }

        public void FillArray(int value)
        {
            for (var index = 0; index < m_cells.Length; index++) SetCell(index, value);
        }

        private void SetCell(int index, int value)
        {
            m_cells[index] = value;
        }

        // Might be needed at a later date.
        public void SetCell(int x, int y, int value)
        {
            m_cells[GetIndex(x, y)] = value;
        }

        private int GetCellValue(int x, int y)
        {
            return m_cells[GetIndex(x, y)];
        }

        private int GetIndex(int x, int y)
        {
            // Clamps incoming X if out of bounds.
            if (x < 0) x = 0;
            if (x >= m_sizeX) x = m_sizeX - 1;
            // Clamps incoming Y if out of bounds.
            if (y < 0) y = 0;
            if (y >= m_sizeY) y = m_sizeY - 1;

            return x + y * m_sizeX;
        }

        // Only code that should be modifying the grid directly is mouse, but pathfinder will be editing it to
        // so this code will be used then. Remove is this happens.
        public bool WithinArrayBounds(int x, int y)
        {
            // Inclusive Lower Bound
            // Exclusive Upper Bound
            return x >= 0 && x < m_sizeX && y >= 0 && y < m_sizeY;
        }

        private bool WithinGridArea(MouseState ms)
        {
            return ms.X - m_position.X >= 0 && ms.X - m_position.X < m_sizeX * m_cellSize &&
                   ms.Y - m_position.Y >= 0 && ms.Y - m_position.Y < m_sizeY * m_cellSize;
        }

        private int GetMouseOverIndex(int x, int y)
        {
            return GetIndex(x / m_cellSize, y / m_cellSize);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw Grid Background
            spriteBatch.Draw(Game1.PIXEL, new Rectangle((int)m_position.X,(int)m_position.Y,
                m_sizeX * m_cellSize, m_sizeY * m_cellSize), m_BACKGROUND_COLOR);
            
            // Draw Cells
            for (var x = 0; x < m_sizeX; x++)
            for (var y = 0; y < m_sizeY; y++)
            {
                var r = new Rectangle((int) m_position.X + x * m_cellSize, (int) m_position.Y + y * m_cellSize,
                    m_cellSize, m_cellSize);
                var cellColor = GetCellValue(x, y) switch
                {
                    0 => Color.Transparent, // Empty
                    1 => Color.MediumBlue, // Start
                    2 => Color.Gold, // Goal
                    3 => Color.DarkGray, // Wall
                    // Other Non Assignable Colours
                    4 => Color.LimeGreen, // Open
                    5 => Color.Red, // Closed
                    _ => Color.Purple // Error Default 
                };
                spriteBatch.Draw(Game1.PIXEL, r, cellColor);
            }

            // Top Border
            spriteBatch.Draw(Game1.PIXEL,
                new Rectangle((int) m_position.X, (int) m_position.Y,
                    m_sizeX * m_cellSize, m_BORDER_THICKNESS), m_BORDER_COLOR);

            // Left Border
            spriteBatch.Draw(Game1.PIXEL,
                new Rectangle((int) m_position.X, (int) m_position.Y + 0,
                    m_BORDER_THICKNESS, m_sizeY * m_cellSize), m_BORDER_COLOR);

            // Right Border
            spriteBatch.Draw(Game1.PIXEL,
                new Rectangle((int) m_position.X + m_sizeY * m_cellSize - 1, (int) m_position.Y + 0,
                    m_BORDER_THICKNESS, m_sizeY * m_cellSize), m_BORDER_COLOR);

            // Bottom Border BUG non-critical -> borders do not line up is border larger then 1px
            spriteBatch.Draw(Game1.PIXEL,
                new Rectangle((int) m_position.X, (int) m_position.Y + m_sizeX * m_cellSize - 1,
                    m_sizeX * m_cellSize, m_BORDER_THICKNESS), m_BORDER_COLOR);
        }

        public void Update(GameTime gameTime)
        {
            var ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed &&
                WithinGridArea(ms)) // could be made into a function takes in ms, value
                SetCell(GetMouseOverIndex(ms.X - (int) m_position.X, ms.Y - (int) m_position.Y), drawTool);
        }

        public void SetTool(int tool)
        {
            drawTool = tool;
        }
    }
}