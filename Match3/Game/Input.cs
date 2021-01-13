using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Match3
{
    static class Input
    {
        private static MouseState mouseState, lastMouseState;
        private static ButtonState buttonState, lastButtonState;
        private static GameTime clickTime, lastClickTime;

        public static void Update(GameTime gameTime)
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();

            lastButtonState = buttonState;
            buttonState = mouseState.LeftButton;

            lastClickTime = clickTime;
            clickTime = gameTime;
        }

        public static bool WasDoubleClick()
        {
            return (clickTime.TotalGameTime - lastClickTime.TotalGameTime).TotalMilliseconds > 15 ; 
        }

        public static bool WasClick()
        {
            if (lastButtonState == ButtonState.Pressed)
                return false;
            else if(lastButtonState == ButtonState.Released && buttonState == ButtonState.Pressed)
                return true;

            return false;
        }

        public static Point GetMouseState()
        {
            return new Point(mouseState.X, mouseState.Y);
        }
    }
}
