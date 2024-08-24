using System;

namespace GamePlatform.Utilities
{
    internal class GameConsoleUI
    {
        public static ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; }
        }
        public static bool CursorVisible
        {
            get { return Console.CursorVisible; }
            set { Console.CursorVisible = value; }
        }
        public static string Title
        {
            get { return Console.Title; }
            set { Console.Title = value; }
        }
        public static bool KeyAvailable
        {
            get { return Console.KeyAvailable; }
            internal set { }
        }

        public static int CursorTop
        {
            get { return Console.CursorTop;  }
            set { Console.CursorTop = value;  }
        }

        public static float WindowWidth 
        {
            get { return Console.WindowWidth; }
            internal set { }
        }
        public static float WindowHeight
        {
            get { return Console.WindowHeight; }
            internal set { }
        }

        public static void ClearConsole()
        {
            Console.Clear();
        }
        public static void ClearConsoleToLine(int top)
        {
            Console.SetCursorPosition(0, top);
            for (int i = 0; i < Console.WindowHeight - top; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }
        public static void ClearConsoleLineBuffer(int top)
        {
            Console.SetCursorPosition(0, top);
            Console.Write(new string(' ', Console.WindowWidth));
        }
        public static void Write(string message)
        {
            Console.Write(message);
        }
        public static void Write(string format, object arg0)
        {
            Console.Write(format, arg0);
        }
        public static void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
        public static void WriteLine(string message, int top)
        {
            ClearConsoleToLine(top);
            Console.SetCursorPosition(0, top);
            Console.WriteLine(message);
        }
        public static void WriteLine(string message, int left, int top)
        {
            ClearConsoleToLine(top);
            Console.SetCursorPosition(left, top);
            Console.WriteLine(message);
        }

        public static (int left, int top) GetConsoleCursorPosition()
        {
            return (Console.CursorLeft, Console.CursorTop);
        }
        internal static ConsoleKeyInfo ReadKey(bool hideKeypress = false)
        {
            return Console.ReadKey(hideKeypress);
        }
        public static char ReadKeyChar(bool hideKeypress = false)
        {
            return Console.ReadKey(hideKeypress).KeyChar;
        }
        public static void FlushKeyBuffer()
        {
            while (Console.KeyAvailable) Console.ReadKey(true);
        }


        public static void SetCursorPosition(int left, int top)
        {
            var w = Console.BufferWidth;
            var h = Console.BufferHeight;
            Console.SetCursorPosition(left, top);
        }
        public static void SetConsoleCursorLine(int top)
        {
            Console.SetCursorPosition(0, top);
        }

        public static void ResetColor()
        {
            Console.ResetColor();
        }

    }
}
