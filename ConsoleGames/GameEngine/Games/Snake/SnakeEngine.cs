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
using System.Xml.Linq;

namespace GamePlatform.Games.Snake
{
    [GameName("Snake")]
    internal class SnakeEngine : ConsoleGame
    {
        SnakeGameModel snakeModel;
        Vector2 Border => new Vector2(GameConsoleUI.WindowWidth, GameConsoleUI.WindowHeight);
        bool gameOver = false;
        char lastKeyPressed;
        int sleepMS = 100;


        public SnakeEngine()
        {
            snakeModel = new SnakeGameModel();
        }

        public override void InitializeGame()
        {
            GameConsoleUI.ClearConsole();
            snakeModel = new SnakeGameModel();
            snakeModel.food = new Vector2(5, 5);
            gameOver = false;
        }
        public override void RunGame()
        {
            GameConsoleUI.ClearConsole();
            GameConsoleUI.WriteLine(ENGLISH_DIRECTIONS, COMMUNICATION_LINE_TOP);
            GameConsoleUI.ReadKeyChar(true);
            GameConsoleUI.ClearConsole();
            PrintBorder();

            snakeModel.GenerateFood(Border);
            PrintFood();

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
            GameConsoleUI.WriteLine($"Game Over... Size: {snakeModel.Body.Count}... Press space to continue");
            Thread.Sleep(1000);
            GameConsoleUI.ReadKeyChar(true);
        }
        private void UpdateFrame()
        {
            if (GameConsoleUI.KeyAvailable)
            {
                lastKeyPressed = GetMoveDirection();
                MovePieces(lastKeyPressed);
            }
            Thread.Sleep(sleepMS);
            Vector2 tail = snakeModel.Body.Last();
            if (CheckForCollision())
            {
                gameOver = true;
                return;
            }
            if (CheckFoodCollision())
            {
                snakeModel.AddToBody();
                PrintSnake();

                snakeModel.food = null;
                snakeModel.GenerateFood(Border);
                PrintFood();
            }
            else
            {
                snakeModel.Move();
                PrintSnake();
                GameConsoleUI.SetCursorPosition((int)tail.X, (int)tail.Y);
                GameConsoleUI.Write(" ");
            }
        }
        private void PrintSnake()
        {
            foreach (Vector2 piece in snakeModel.Body)
            {
                GameConsoleUI.SetCursorPosition((int)piece.X, (int)piece.Y);
                GameConsoleUI.Write("O");
            }
        }
        private void PrintFood()
        {
            GameConsoleUI.SetCursorPosition((int)snakeModel.food.Value.X, (int)snakeModel.food.Value.Y);
            GameConsoleUI.Write("X");
        }
        private void PrintBorder()
        {
            (int width, int height) = ((int)Border.X, (int)Border.Y);
            for (int i = 0; i < width; i++)
            {
                GameConsoleUI.SetCursorPosition(i, 0);
                GameConsoleUI.Write("█");
                GameConsoleUI.SetCursorPosition(i, height - 1);
                GameConsoleUI.Write("█");
            }
            for (int i = 0; i < height; i++)
            {
                GameConsoleUI.SetCursorPosition(0, i);
                GameConsoleUI.Write("██");
                GameConsoleUI.SetCursorPosition(width - 2, i);
                GameConsoleUI.Write("██");
            }
        }
        private bool CheckForCollision()
        {
            if (snakeModel.Head.X == Border.X - 2 || snakeModel.Head.Y == Border.Y - 1 || snakeModel.Head.X == 1 || snakeModel.Head.Y == 1) return true;
            if (snakeModel.Body.Count > 1 && snakeModel.Body.Skip(1).Any(b => b == snakeModel.Head)) return true;
            return false;
        }
        private bool CheckFoodCollision()
        {
            return (snakeModel.Head == snakeModel.food);
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
                    snakeModel.Direction = new Vector2(0, -1);
                    if (snakeModel.Direction.Y == -1) sleepMS = FRAME_WAIT;
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.l:
                    snakeModel.Direction = new Vector2(-1, 0);
                    if (snakeModel.Direction.X == -1) sleepMS = FRAME_WAIT / 2;
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.d:
                    snakeModel.Direction = new Vector2(0, 1);
                    if (snakeModel.Direction.Y == 1) sleepMS = FRAME_WAIT;
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.r:
                    snakeModel.Direction = new Vector2(1, 0);
                    if (snakeModel.Direction.X == 1) sleepMS = FRAME_WAIT / 2;
                    break;
            }
        }

        const int COMMUNICATION_LINE_TOP = 0;
        const int FRAME_WAIT = 100;
        const string ENGLISH_DIRECTIONS = "Use WASD to move the snake.... press space to continue";

        private readonly (char t, char l, char d, char r) QWERTY_DEFAULT_DIRECTION_KEYS = ('w', 'a', 's', 'd');
    }
}
