using AbstractGame;
using BasicGameInterface;
using GamePlatform.Utilities;
using System;
using System.Text;

namespace Connect4
{
    [GameName("Connect 4")]
    public class Connect4Engine : ConsoleGame
    {
        private Connect4Board board = null;
        private int currentPlayer;
        private int winner;
        private readonly object lockObject = new object();
        private Slot lastPlacedSlot;

        public Connect4Engine(){}
        public override void InitializeGame()
        {
            GameConsoleUI.CursorVisible = false;
            if (board == null) board = new Connect4Board(ROWS, COLUMNS);
            PrintBoard();
            currentPlayer = PLAYER1;
            winner = 0;
        }
        public override void RunGame()
        {
            do PlayRound(); while (!board.InARow(lastPlacedSlot, out winner));
            GameOver();
        }
        public override void CleanUp()
        {
            board = null;
            GameConsoleUI.SetConsoleCursorLine(COMMUNICATION_LINE_TOP + 1);
            GameConsoleUI.FlushKeyBuffer();
        }
        private void GameOver()
        {
            lock (lockObject)
            {
                GameConsoleUI.ClearConsoleLineBuffer(COMMUNICATION_LINE_TOP);
                GameConsoleUI.WriteLine((winner == PLAYER1) ? PLAYER1_WIN : 
                                                              (winner == PLAYER2) ? PLAYER2_WIN : TIE, COMMUNICATION_LINE_TOP);
            }            
            while (GameConsoleUI.ReadKeyChar(true) != ' ');
        }
        private void PlayRound()
        {
            int column;
            bool validMove;
            // repeat until a valid move is made
            do
            {
                column = GetPlayerInput();
                validMove = board.TryPlacePiece(column - 1, currentPlayer, out Slot newPiece);
                if (!validMove)
                {
                    lock (lockObject)
                    {
                        GameConsoleUI.ClearConsoleLineBuffer(COMMUNICATION_LINE_TOP);
                        GameConsoleUI.WriteLine(INVALID_MOVE + INSTRTUCTIONS, COMMUNICATION_LINE_TOP);
                    }
                }
                else
                {
                    PrintBoard();
                    lastPlacedSlot = newPiece;
                    currentPlayer = (currentPlayer == PLAYER1) ? PLAYER2 : PLAYER1;
                }
            }
            while (!validMove);
        }
        private int GetPlayerInput()
        {
            int columnInt;
            while (true)
            {
                char columnChar = GameConsoleUI.ReadKeyChar(true);
                Int32.TryParse(columnChar.ToString(), out columnInt);
                if (1 <= columnInt && columnInt <= board.COLUMNS) break;
                else
                {

                    lock (lockObject)
                    {
                        GameConsoleUI.WriteLine(INVALID_MOVE + INSTRTUCTIONS, COMMUNICATION_LINE_TOP);   
                    }
                }
            }

            return columnInt;
        }
        private void PrintBoard()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("  1   2   3   4   5   6   7");
            sb.AppendLine("┌───┬───┬───┬───┬───┬───┬───┐");
            for (int row = 0; row < 6; row++)
            {
                sb.Append("│");
                for (int col = 0; col < board.COLUMNS; col++)
                {
                    Slot piece = board[row, col];
                    if (piece == Slot.INVALID_SLOT) throw new Exception($"Invalid board location: [{row}]:[{col}]");
                    if (piece.Player == PLAYER1)
                    {
                        sb.Append(" X │");
                    }
                    else if (piece.Player == PLAYER2)
                    {
                        sb.Append(" O │");
                    }
                    else
                    {
                        sb.Append("   │");
                    }
                }
                sb.AppendLine();
                if (row < 5)
                {
                    sb.AppendLine("├───┼───┼───┼───┼───┼───┼───┤");
                }
            }
            sb.AppendLine("└───┴───┴───┴───┴───┴───┴───┘");
            sb.AppendLine(INSTRTUCTIONS);

            lock (lockObject)
            {
                GameConsoleUI.ClearConsole();
                GameConsoleUI.WriteLine(sb.ToString(), BOARD_PRINT.left, BOARD_PRINT.top);
            }
        }



        private readonly (int left, int top) BOARD_PRINT = (0, 0);
        private const int COMMUNICATION_LINE_TOP = (14);

        private const int COLUMNS = 7;
        private const int ROWS = 6;
        private const string INSTRTUCTIONS = "Enter a column number to place your piece. 1-7";
        private const string INVALID_MOVE = "Invalid. Try again. ";
        private const string PLAYER1_WIN = "Player 1 wins! Press space to continue";
        private const string PLAYER2_WIN = "Player 2 wins! Press space to continue";
        private const string TIE = "It's a tie! Press space to continue";
        private const int PLAYER1 = 1;
        private const int PLAYER2 = 2;

    }
}
