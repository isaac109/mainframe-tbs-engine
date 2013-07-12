using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Mainframe.Core
{
    /// <summary>
    /// Keeps track of input, making it accessible for other classes.
    /// </summary>
    public static class Controls
    {
        private const short numControls = 2;
        private static bool[] buttonsPressed, buttonsPressedLast;
        private static Vector2 lastMousePos, currMousePos;

        //Used to keep decent track of button positions
        public enum ButtonNames
        {
            leftMouse,
            rightMouse,
        }

        /// <summary>
        /// Sets up this frame's control input storage structures
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="ks"></param>
        public static void updateControls(MouseState ms, KeyboardState ks)
        {
            for (int i = 0; i < buttonsPressed.Length; i++)
            {
                buttonsPressedLast[i] = buttonsPressed[i];
            }
            buttonsPressed[(int)ButtonNames.leftMouse] = ms.LeftButton == ButtonState.Pressed;
            buttonsPressed[(int)ButtonNames.rightMouse] = ms.RightButton == ButtonState.Pressed;
            lastMousePos = currMousePos;
            currMousePos = new Vector2(ms.X, ms.Y);
        }

        /// <summary>
        /// Creates the necessary objects to hold input data;
        /// </summary>
        public static void Initialize()
        {
            buttonsPressed = new bool[numControls];
            buttonsPressedLast = new bool[numControls];
            lastMousePos = new Vector2(0, 0);
            currMousePos = new Vector2(0, 0);
        }

        //Public accessors for necessary data

        public static bool[] ButtonsDown
        {
            get { return buttonsPressed; }
        }

        public static bool[] ButtonsLastDown
        {
            get { return buttonsPressedLast; }
        }

        public static Vector2 MousePosition
        {
            get { return currMousePos; }
        }

        public static Vector2 MouseLastPosition
        {
            get { return lastMousePos; }
        }
    }
}