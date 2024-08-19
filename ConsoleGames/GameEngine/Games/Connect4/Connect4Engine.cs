using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    public class Connect4Engine : ConsoleGame
    {
        private Board board;
        private int currentPlayer = 1;
        private int winner = 0;
        private object lockObject = new object();

        public Connect4Engine()
        {
        }
        public override void InitializeGame()
        {
            board = new Board();
            PrintBoard();
            currentPlayer = 1;
            winner = 0;
        }
        public override void RunGame()
        {
            while (true) PlayRound();
        }
        public override void CleanUp()
        {
            throw new NotImplementedException();
        }
        private void PlayRound()
        {
            int column;
            bool validMove = false;
            // repeat until a valid move is made
            do
            {
                column = GetPlayerInput();
                validMove = board.PlacePiece(column - 1, currentPlayer);
                if (!validMove)
                {
                    lock (lockObject)
                    {
                        Console.SetCursorPosition(COMMUNICATION_LINE.left, COMMUNICATION_LINE.top);
                        Console.WriteLine(INVALID_MOVE + INSTRTUCTIONS);
                    }
                }
                else
                {
                    PrintBoard();
                    currentPlayer = currentPlayer == 1 ? 2 : 1;
                }
            }
            while (!validMove);
        }

        private int GetPlayerInput()
        {
            int columnInt;
            while (true)
            {
                char columnChar = Console.ReadKey(true).KeyChar;
                Int32.TryParse(columnChar.ToString(), out columnInt);
                if (1 <= columnInt && columnInt <= 7) break;
                else
                {

                    lock (lockObject)
                    {
                        Console.SetCursorPosition(COMMUNICATION_LINE.left, COMMUNICATION_LINE.top);
                        Console.WriteLine(INVALID_MOVE + INSTRTUCTIONS);
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
                for (int col = 0; col < 7; col++)
                {
                    Slot piece = board[row,col];
                    if (piece.Player == -1) throw new Exception($"Invalid board location: [{row}]:[{col}]");
                    if (piece.Player == 1)
                    {
                        sb.Append(" X │");
                    }
                    else if (piece.Player == 2)
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
                Console.Clear();
                Console.SetCursorPosition(BOARD_PRINT.left, BOARD_PRINT.top);
                Console.WriteLine(sb.ToString());
            }
        }



        private readonly (int left, int top) BOARD_PRINT = (0, 0);
        private readonly (int left, int top) COMMUNICATION_LINE = (0, 14);

        private const string INSTRTUCTIONS = "Enter a column number to place your piece. 1-7";
        private const string INVALID_MOVE = "Invalid. Try again. ";

    }
}
