using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AbstractGame;
using BasicGameInterface;
using GamePlatform.Utilities;

namespace TicTacToe
{
    [GameName("Tic Tac Toe")]
    public class TicTacToeEngine : ConsoleGame
    {
        private string Player1;
        private string Player2;
        private char winner;
        private bool Player1Turn;
        private readonly TicTacToeBoard boardModel;
        private readonly object _Cursorlock = new object();
        private (int l, int t) BoardCursorPosition;

        public TicTacToeEngine()
        {
            boardModel = new TicTacToeBoard();
        }

        public override void InitializeGame()
        {
            GameConsoleUI.CursorVisible = false;
            GameConsoleUI.ClearConsole();
            GameConsoleUI.Title = "Tic Tac Toe";
            boardModel.Init();
            Player1 = PLAYER1_NAME;
            Player2 = PLAYER2_NAME;
            Player1Turn = true;

            PrintBoard();
            PrintValues();
            BoardCursorPosition = BOARD_CURSOR_LOCATIONS.ElementAt(4);
        }

        public override void RunGame()
        {
            do PlayRound(); while (IsGameOver());
            GameOver();
        }

        private void GameOver()
        {
            string winMessage = (winner == 't') ? (DRAW_MESSAGE + SPACE_TO_CONTINUE) : 
                     (winner == PLAYER1_SYMBOL) ? (PLAYER1_WINS + SPACE_TO_CONTINUE) : 
                                                  (PLAYER2_WINS + SPACE_TO_CONTINUE);
            lock (_Cursorlock)
            {
                GameConsoleUI.ClearConsoleLineBuffer(COMMUNICATION_LINE_TOP);
                GameConsoleUI.WriteLine(winMessage, COMMUNICATION_LINE_TOP);                
            }

            while (GameConsoleUI.ReadKeyChar(true) != ' ') ;

        }
        private bool IsGameOver()
        {
            return boardModel.IsGameOver(out winner);
        }
        public override void CleanUp()
        {
            GameConsoleUI.CursorVisible = true;
            lock (_Cursorlock)
            {
                GameConsoleUI.SetConsoleCursorLine(COMMUNICATION_LINE_TOP + 1);
                GameConsoleUI.FlushKeyBuffer();
            }
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
                    MoveBoardPosition(direction);

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
                boardModel[index] = PLAYER1_SYMBOL;
                Player1Turn = false;
            }
            else
            {
                boardModel[index] = PLAYER2_SYMBOL;
                Player1Turn = true;
            }
            PrintValues();
        }
        private char GetMoveDirection()
        {
            lock (_Cursorlock)
            {
                GameConsoleUI.Write(ENGLISH_DIRECTIONS, COMMUNICATION_LINE_TOP);
            }
            char direction;
            while (true)
            {
                ConsoleKeyInfo keyInfo = GameConsoleUI.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    direction = '\r';
                    break;
                }
                direction = keyInfo.KeyChar;

                if (QWERTY_DEFAULT_DIRECTION_KEYS.ToString().ToLower().Contains(direction)) break;
            }

            return direction;
        }
        private void MoveBoardPosition(char direction)
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
                lock (_Cursorlock)
                {
                    GameConsoleUI.SetCursorPosition(cursorPos.l, cursorPos.t);
                    GameConsoleUI.Write(@"{0,2}", FAKE_CURSOR);
                }
                Thread.Sleep(100);
                lock (_Cursorlock)
                {
                    GameConsoleUI.SetCursorPosition(cursorPos.l, cursorPos.t);
                    GameConsoleUI.Write(@"{0,2}", boardChar);
                }
                Thread.Sleep(100);
            }

            _keepFlashing = true;
        }
        private char? GetBoardCharacter((int l, int t) cursorPosition)
        {
            int index = BOARD_CURSOR_LOCATIONS.IndexOf(cursorPosition);

            if (index < 0) return null;
            return boardModel[index];
        }
        private void PrintBoard()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("╔═══╦═══╦═══╗");
            sb.AppendLine("║   ║   ║   ║");
            sb.AppendLine("╠═══╬═══╬═══╣");
            sb.AppendLine("║   ║   ║   ║");
            sb.AppendLine("╠═══╬═══╬═══╣");
            sb.AppendLine("║   ║   ║   ║");
            sb.AppendLine("╚═══╩═══╩═══╝");

            
            lock (_Cursorlock)
            {
                GameConsoleUI.WriteLine(sb.ToString(), BOARD_POSITION.t);
                GameConsoleUI.Write(ENGLISH_DIRECTIONS, COMMUNICATION_LINE_TOP);
            }
        }
        private void PrintValues()
        {
            foreach (var cursor in BOARD_CURSOR_LOCATIONS)
            {
                lock (_Cursorlock)
                {
                    GameConsoleUI.SetCursorPosition(cursor.l, cursor.t);
                    GameConsoleUI.Write(@"{0,2}", boardModel[BOARD_CURSOR_LOCATIONS.IndexOf(cursor)]);
                }
            }
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
        private const int COMMUNICATION_LINE_TOP = 7;


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
