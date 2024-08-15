﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AbstractGame;

namespace TikTacToe
{
    public class TikTacToeEngine : ConsoleGame
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public bool Player1Turn { get; set; }
        public char[] Board { get; set; }
        public (int l, int t) CurrentCursorPosition => (Console.CursorLeft, Console.CursorTop);
        public (int l, int t) BoardCursorPosition;


        public override void InitializeGame()
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.Title = "Tik Tac Toe";
            Board = new char[9] { ',', ',', ',', ',', ',', ',', ',', ',', ',' };
            Player1 = PLAYER1_NAME;
            Player2 = PLAYER2_NAME;
            Player1Turn = true;

            PrintBoard();
            PrintValues();
            BoardCursorPosition = BOARD_CURSOR_LOCATIONS.ElementAt(4);
        }

        public override void RunGame()
        {
            char winner = 'f';
            do
            {
                PlayRound();
                PrintValues();
                winner = IsGameOver();
            } while (winner == 'f');
            GameOver(winner);
        }

        private void GameOver(char winner)
        {
            if (winner == 't')
            {
                ClearConsoleBuffer(COMMUNICATION_LINE.t);
                Console.SetCursorPosition(COMMUNICATION_LINE.l, COMMUNICATION_LINE.t);
                Console.WriteLine(DRAW_MESSAGE + SPACE_TO_CONTINUE);
            }
            else if (winner == PLAYER1_SYMBOL)
            {
                ClearConsoleBuffer(COMMUNICATION_LINE.t);
                Console.SetCursorPosition(COMMUNICATION_LINE.l, COMMUNICATION_LINE.t);
                Console.WriteLine(PLAYER1_WINS + SPACE_TO_CONTINUE);
            }
            else
            {
                ClearConsoleBuffer(COMMUNICATION_LINE.t);
                Console.SetCursorPosition(COMMUNICATION_LINE.l, COMMUNICATION_LINE.t);
                Console.WriteLine(PLAYER2_WINS + SPACE_TO_CONTINUE);
            }
            while (Console.ReadKey(true).KeyChar != ' ') ;

        }
        private char IsGameOver()
        {
            if (Board.Contains(',') == false) return 't';
            for (int i = 0; i < 3; i++)
            {
                if (Board[i] == Board[i + 3] && Board[i] == Board[i + 6] && Board[i] != ',') return Board[i];
                if (Board[i * 3] == Board[i * 3 + 1] && Board[i * 3] == Board[i * 3 + 2] && Board[i * 3] != ',') return Board[i];
            }
            if (Board[0] == Board[4] && Board[0] == Board[8] && Board[0] != ',') return Board[0];
            if (Board[2] == Board[4] && Board[2] == Board[6] && Board[2] != ',') return Board[2];
            return 'f';
        }
        public override void CleanUp()
        {
            Console.CursorVisible = true;
            Console.SetCursorPosition(0, COMMUNICATION_LINE.t + 1);
            while (Console.KeyAvailable) Console.ReadKey(true);
        }
        private void PlayRound()
        {
            Thread flashThread = new Thread(FlashCursor);
            flashThread.Start();

            while (true)
            {
                // move cursor around board
                while (true)
                {
                    char direction = GetMoveDirection();
                    _keepFlashing = false;
                    flashThread.Join();

                    if (direction == ENTER_KEY) break;
                    MoveCursor(direction);

                    _keepFlashing = true;
                    flashThread = new Thread(FlashCursor);
                    flashThread.Start();
                }
                if (GetBoardCharacter(BoardCursorPosition) != ',') continue;
                else break;
            }

            int index = BOARD_CURSOR_LOCATIONS.IndexOf(BoardCursorPosition);
            if (Player1Turn)
            {
                Board[index] = PLAYER1_SYMBOL;
                Player1Turn = false;
            }
            else
            {
                Board[index] = PLAYER2_SYMBOL;
                Player1Turn = true;
            }
        }
        private char GetMoveDirection()
        {
            char direction = ' ';

            Console.SetCursorPosition(COMMUNICATION_LINE.l, COMMUNICATION_LINE.t);
            Console.Write(ENGLISH_DIRECTIONS);
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    direction = '\r';
                    break;
                }
                direction = keyInfo.KeyChar;

                if (QWERTY_DEFAULT_DIRECTION_KEYS.ToString().ToLower().Contains(direction)) break;
                else Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }

            return direction;
        }
        private void MoveCursor(char direction)
        {
            switch (char.ToUpper(direction))
            {
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.t:
                    MoveUp();
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.l:
                    MoveLeft();
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.d:
                    MoveDown();
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.r:
                    MoveRight();
                    break;
            }
        }
        private void MoveLeft()
        {
            bool notLeftColumn = (BoardCursorPosition != BOARD_CURSOR_LOCATIONS.ElementAt(0) && BoardCursorPosition != BOARD_CURSOR_LOCATIONS.ElementAt(3) && BoardCursorPosition != BOARD_CURSOR_LOCATIONS.ElementAt(6));
            bool isOnBoard = (BOARD_CURSOR_LOCATIONS.IndexOf(BoardCursorPosition) != -1);
            if (notLeftColumn && isOnBoard)
            {
                BoardCursorPosition = (BoardCursorPosition.l - 4, BoardCursorPosition.t);
            }
        }
        private void MoveRight()
        {
            bool notRightColumn = (BoardCursorPosition != BOARD_CURSOR_LOCATIONS.ElementAt(2) && BoardCursorPosition != BOARD_CURSOR_LOCATIONS.ElementAt(5) && BoardCursorPosition != BOARD_CURSOR_LOCATIONS.ElementAt(8));
            bool isOnBoard = (BOARD_CURSOR_LOCATIONS.IndexOf(BoardCursorPosition) != -1);
            if (notRightColumn && isOnBoard)
            {
                BoardCursorPosition = (BoardCursorPosition.l + 4, BoardCursorPosition.t);
            }
        }
        private void MoveUp()
        {
            bool notFirstRow = (BoardCursorPosition != BOARD_CURSOR_LOCATIONS.ElementAt(0) && BoardCursorPosition != BOARD_CURSOR_LOCATIONS.ElementAt(1) && BoardCursorPosition != BOARD_CURSOR_LOCATIONS.ElementAt(2));
            bool isOnBoard = (BOARD_CURSOR_LOCATIONS.IndexOf(BoardCursorPosition) != -1);
            if (notFirstRow && isOnBoard)
            {
                BoardCursorPosition = (BoardCursorPosition.l, BoardCursorPosition.t - 2);
            }
        }
        private void MoveDown()
        {
            bool notBottomRow = (BoardCursorPosition != BOARD_CURSOR_LOCATIONS.ElementAt(6) && BoardCursorPosition != BOARD_CURSOR_LOCATIONS.ElementAt(7) && BoardCursorPosition != BOARD_CURSOR_LOCATIONS.ElementAt(8));
            bool isOnBoard = (BOARD_CURSOR_LOCATIONS.IndexOf(BoardCursorPosition) != -1);
            if (notBottomRow && isOnBoard)
            {
                BoardCursorPosition = (BoardCursorPosition.l, BoardCursorPosition.t + 2);
            }
        }
        private void FlashCursor()
        {
            (int l, int t) cursorPos = BoardCursorPosition;
            char? boardChar = GetBoardCharacter(cursorPos);

            if (boardChar == null) return;

            while (_keepFlashing)
            {
                Console.SetCursorPosition(cursorPos.l, cursorPos.t);
                Console.Write(@"{0,2}", FAKE_CURSOR);
                Thread.Sleep(100);
                Console.SetCursorPosition(cursorPos.l, cursorPos.t);
                Console.Write(@"{0,2}",boardChar);
                Thread.Sleep(100);
            }

            _keepFlashing = true;
        }
        private char? GetBoardCharacter((int l, int t) cursorPosition)
        {
            int index = BOARD_CURSOR_LOCATIONS.IndexOf(cursorPosition);

            if (index < 0) return null;
            return Board[index];
        }
        private void PrintBoard()
        {
            Console.SetCursorPosition(BOARD_POSITION.l, BOARD_POSITION.t);
            Console.WriteLine("╔═══╦═══╦═══╗");
            Console.WriteLine("║   ║   ║   ║");
            Console.WriteLine("╠═══╬═══╬═══╣");
            Console.WriteLine("║   ║   ║   ║");
            Console.WriteLine("╠═══╬═══╬═══╣");
            Console.WriteLine("║   ║   ║   ║");
            Console.WriteLine("╚═══╩═══╩═══╝");
            Console.SetCursorPosition(0, COMMUNICATION_LINE.t);
            Console.WriteLine(ENGLISH_DIRECTIONS);
        }
        private void PrintValues()
        {
            foreach (var cursor in BOARD_CURSOR_LOCATIONS)
            {
                Console.SetCursorPosition(cursor.l, cursor.t);
                Console.Write(@"{0,2}", Board[BOARD_CURSOR_LOCATIONS.IndexOf(cursor)]);
            }
        }
        private void ClearConsoleBuffer(int top)
        {
            Console.SetCursorPosition(0, top);
            Console.Write(new String(' ', Console.BufferWidth));
        }

        private readonly (int l, int t) BOARD_POSITION = (0, 0);
        private readonly List<(int l, int t)> BOARD_CURSOR_LOCATIONS = new List<(int l, int r)>
        {
            (1, 1), (5, 1), (9, 1),
            (1, 3), (5, 3), (9, 3),
            (1, 5), (5, 5), (9, 5)
        };
        private volatile bool _keepFlashing = true;

        private readonly (char t, char l, char d, char r) QWERTY_DEFAULT_DIRECTION_KEYS = ('W', 'A', 'S', 'D');
        private readonly char ENTER_KEY = '\r';
        private readonly (int l, int t) COMMUNICATION_LINE = (0, 7);


        private const char PLAYER1_SYMBOL = 'X';
        private const char PLAYER2_SYMBOL = 'O';
        private const string PLAYER1_NAME = "Player 1";
        private const string PLAYER2_NAME = "Player 2";
        private const string ENGLISH_DIRECTIONS = "Enter a direction (W, A, S, D) to move the cursor and press Enter to place your symbol. ";
        private const string DRAW_MESSAGE = "DRAW! GAME OVER! "; 
        private const string PLAYER1_WINS = "PLAYER 1 WINS! GAME OVER! ";
        private const string PLAYER2_WINS = "PLAYER 2 WINS! GAME OVER! ";
        private const string SPACE_TO_CONTINUE = "Press space to continue...";
        private const string FAKE_CURSOR = "█";

    }
}
