using AbstractGame;
using BasicGameInterface;
using GameEngine;
using GamePlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GamePlatform.Games.Snake
{
    [GameName("Snake")]
    internal class SnakeEngine : ConsoleGame
    {
        SnakeBody snake;
        Vector2? food = null;
        bool gameOver = false;
        char lastKeyPressed;
        int sleepMS = 100;


        public SnakeEngine()
        {
            snake = new SnakeBody();
        }

        public override void InitializeGame()
        {
            GameConsoleUI.ClearConsole();
            snake = new SnakeBody();
            food = new Vector2(5, 5);
            gameOver = false;
        }
        public override void RunGame()
        {
            GameConsoleUI.ClearConsole();
            GameConsoleUI.WriteLine(ENGLISH_DIRECTIONS, COMMUNICATION_LINE_TOP);
            GameConsoleUI.ReadKeyChar(true);
            GameConsoleUI.ClearConsole();
            GenerateFood();

            do UpdateFrame(); while (!gameOver); GameOver();
        }
        public override void CleanUp()
        {
            GameConsoleUI.ClearConsole();
            gameOver = false;
        }

        private void GameOver()
        {
            GameConsoleUI.ClearConsole();
            GameConsoleUI.WriteLine($"Game Over... Size: {snake.Body.Count}... Press space to continue");
            GameConsoleUI.ReadKeyChar(true);
        }
        private void UpdateFrame()
        {
            Thread.Sleep(sleepMS);
            if (GameConsoleUI.KeyAvailable)
            {
                lastKeyPressed = GetMoveDirection();
                MovePieces(lastKeyPressed);
            }
            Vector2 tail = snake.Body.Last();
            if (CheckForCollision())
            {
                gameOver = true;
                return;
            }
            if (CheckFoodCollision())
            {
                food = null;
                GenerateFood();
                snake.AddToBody();
                PrintSnake();
            }
            else
            {
                snake.Move();
                PrintSnake();
                GameConsoleUI.SetCursorPosition((int)tail.X, (int)tail.Y);
                GameConsoleUI.Write(" ");
            }
        }
        private void PrintSnake()
        {
            foreach (Vector2 piece in snake.Body)
            {
                GameConsoleUI.SetCursorPosition((int)piece.X, (int)piece.Y);
                GameConsoleUI.Write("O");
            }
        }
        private void GenerateFood()
        {
            Random random = RandomSingleton.Instance;

            Vector2 windowSize = new Vector2(GameConsoleUI.WindowWidth, GameConsoleUI.WindowHeight);
            while (true)
            {
                food = new Vector2(random.Next(1, (int)windowSize.X), random.Next(1, (int)windowSize.Y));
                if (!snake.Body.Contains(food.Value)) break;
            }
            GameConsoleUI.SetCursorPosition((int)food.Value.X, (int)food.Value.Y);
            GameConsoleUI.Write("X");
        }
        private bool CheckForCollision()
        {
            Vector2 windowSize = new Vector2(GameConsoleUI.WindowWidth, GameConsoleUI.WindowHeight);
            if (snake.Head.X == windowSize.X - 1 || snake.Head.Y == windowSize.Y - 1 || snake.Head.X == 0 || snake.Head.Y == 0) return true;
            if (snake.Body.Count > 1 && snake.Body.Skip(1).Any(b => b == snake.Head)) return true;
            return false;
        }
        private bool CheckFoodCollision()
        {
            if (snake.Head == food)
            {
                return true;
            }
            return false;
        }
        private char GetMoveDirection()
        {
            char direction;
            while (true)
            {
                direction = GameConsoleUI.ReadKeyChar(true);
                if (QWERTY_DEFAULT_DIRECTION_KEYS.ToString().ToLower().Contains(direction)) break;
                else GameConsoleUI.SetCursorPosition(0, 0);
            }

            return direction;
        }
        private void MovePieces(char direction)
        {
            switch (direction)
            {
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.t:
                    snake.Direction = new Vector2(0, -1);
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.l:
                    snake.Direction = new Vector2(-1, 0);
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.d:
                    snake.Direction = new Vector2(0, 1);
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.r:
                    snake.Direction = new Vector2(1, 0);
                    break;
            }
        }

        const int COMMUNICATION_LINE_TOP = 0;
        const string ENGLISH_DIRECTIONS = "Use WASD to move the snake.... press space to continue";

        private readonly (char t, char l, char d, char r) QWERTY_DEFAULT_DIRECTION_KEYS = ('w', 'a', 's', 'd');
    }
    internal class SnakeBody
    {
        public List<Vector2> Body { get; set; }
        private Vector2 direction;
        public Vector2 Direction
        {
            get { return direction; }
            set
            {
                if (value.X != 0 && value.Y != 0) throw new Exception("Invalid Direction"); ;

                if (value.X != 0 && direction.X == 0) direction = value;
                else if (value.Y != 0 && direction.Y == 0) direction = value;
            }
        }
        public Vector2 Head => Body[0];
        public Vector2 Tail => Body[Body.Count - 1];
        public SnakeBody()
        {
            Body = new List<Vector2> { new Vector2(10, 10) };
            Direction = new Vector2(1, 0);
        }
        public void Move()
        {
            for (int i = Body.Count - 1; i > 0; i--)
            {
                Body[i] = Body[i - 1];
            }
            Body[0] += Direction;
        }
        public void AddToBody()
        {
            var tail = Tail;
            Move();
            Body.Add(tail);
        }
    }
}
