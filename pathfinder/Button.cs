using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// TODO add bool for on hover vv
// FIXME change current colour hover method.


// TODO eventListener for Button
// TODO Auto-adjust code to text on button.

namespace pathfinder
{
    public class Button
    {
        private MouseState m_currentState;
        private MouseState m_previousState;

        private bool isHovering;
        private Color m_color;
        private readonly Color m_colorOnHover = Color.Gray;

        private bool showTooltip;
        
        private readonly Vector2 m_position;
        
        private readonly string m_text;
        private readonly int m_width;
        private readonly int m_height;

        public event EventHandler onClick;

        public Button(string text, Vector2 position, Color color, int width, int height)
        {
            m_text = text;
            m_color = color;
            m_width = width;
            m_height = height;
            m_position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color btnColor = isHovering ? m_colorOnHover : m_color;
            spriteBatch.Draw(Game1.PIXEL, new Rectangle((int) m_position.X, (int) m_position.Y,
                m_width, m_height), btnColor);

            spriteBatch.DrawString(Game1.pixelFont, m_text, new Vector2( m_position.X + 8,m_position.Y + 4),
                Color.White);

            if (showTooltip)
            {
                spriteBatch.DrawString(Game1.pixelFont, "Tooltip Text Test", new Vector2( m_currentState.X + 10 ,m_currentState.Y), Color.White);
            }
        }

        private bool WithinButton(MouseState ms)
        {
            return ms.X - m_position.X >= 0 && ms.X - m_position.X < m_width &&
                   ms.Y - m_position.Y >= 0 && ms.Y - m_position.Y < m_height;
        }

        public void Update(GameTime gameTime)
        {
            m_previousState = m_currentState;
            m_currentState = Mouse.GetState();

            isHovering = false;
            showTooltip = false;
            
            if (WithinButton(m_currentState))
            {
                isHovering = true;
                if (m_currentState.LeftButton == ButtonState.Released &&
                    m_previousState.LeftButton == ButtonState.Pressed)
                {
                    onClick?.Invoke(this, new EventArgs());
                }
                
                if (Keyboard.GetState().IsKeyDown(Keys.I))
                {
                    showTooltip = true;
                }
            }
            // if (WithinButton(ms))
            // {
            //     m_color = m_colorOnHover;
            //     if (ms.LeftButton == ButtonState.Pressed)
            //     {
            //         Console.WriteLine("Event Triggered");
            //         
            //     }
            // }
            // else m_color = m_colorNormal;
        }
    }
}