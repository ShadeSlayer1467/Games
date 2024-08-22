using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AbstractGame;
using BasicGameInterface;
using GamePlatform.Utilities;

namespace TikTacToe
{
    [GameName("Tic Tac Toe")]
    public class TikTacToeEngine : ConsoleGame
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public bool Player1Turn { get; set; }
        public char[] Board { get; set; }
        public object _Cursorlock = new object();
        public (int l, int t) CurrentCursorPosition => (GameConsoleUI.GetConsoleCursorPosition());
        public (int l, int t) BoardCursorPosition;


        public override void InitializeGame()
        {
            GameConsoleUI.CursorVisible = false;
            GameConsoleUI.ClearConsole();
            GameConsoleUI.Title = "Tik Tac Toe";
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
            char winner;
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
            lock (_Cursorlock)
            {
                GameConsoleUI.ClearConsoleLineBuffer(COMMUNICATION_LINE_TOP);
                if (winner == 't') 
                    GameConsoleUI.WriteLine(DRAW_MESSAGE + SPACE_TO_CONTINUE, COMMUNICATION_LINE_TOP);
                else if (winner == PLAYER1_SYMBOL) 
                    GameConsoleUI.WriteLine(PLAYER1_WINS + SPACE_TO_CONTINUE, COMMUNICATION_LINE_TOP);
                else 
                    GameConsoleUI.WriteLine(PLAYER2_WINS + SPACE_TO_CONTINUE, COMMUNICATION_LINE_TOP);
                
            }

            while (GameConsoleUI.ReadKeyChar(true) != ' ') ;

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
            lock (_Cursorlock)
            {
                GameConsoleUI.CursorVisible = true;
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
            return Board[index];
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
                    GameConsoleUI.Write(@"{0,2}", Board[BOARD_CURSOR_LOCATIONS.IndexOf(cursor)]);
                }
            }
        }
        private void ClearConsoleBuffer(int top)
        {
            GameConsoleUI.ClearConsoleLineBuffer(top);
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
