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
            do PlayRound(); while (!CheckForWinner());
            GameOver();
        }

        private void GameOver()
        {
            lock (lockObject)
            {
                Console.SetCursorPosition(COMMUNICATION_LINE.left, COMMUNICATION_LINE.top);
                Console.WriteLine((winner == 1) ? PLAYER1_WIN : 
                                  (winner == 2) ? PLAYER2_WIN : TIE);
            }
            while (Console.ReadKey(true).KeyChar != ' ');
        }
        public override void CleanUp()
        {
            Console.SetCursorPosition(0, COMMUNICATION_LINE.top + 1);
            while (Console.KeyAvailable) Console.ReadKey(true);
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
                    currentPlayer = (currentPlayer == 1) ? 2 : 1;
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
        private bool CheckForWinner()
        {
            Slot[] slots;

            // Horizontal
            for (int row = 0; row < 6; row++)
            {
                slots = board.Row(row);
                if (ContainsWinner(slots)) return true;
            }

            // Vertical
            for (int col = 0; col < 7; col++)
            {
                slots = board.Column(col);
                if (ContainsWinner(slots)) return true;
            }

            // Diagonal
            foreach (Slot[] list in board.Diagonals())
            {
                if (ContainsWinner(list)) return true;
            }

            return false;
        }
        private bool ContainsWinner(Slot[] slots)
        {
            if (slots.Length < 4) return false;
            // sliding window of 4
            for (int i = 0; i < slots.Length - 3; i++)
            {
                if (slots[i].Player != 0 &&
                    slots[i].Player == slots[i + 1].Player &&
                    slots[i + 1].Player == slots[i + 2].Player &&
                    slots[i + 2].Player == slots[i + 3].Player)
                {
                    winner = slots[i].Player;
                    return true;
                }
            }
            return false;
        }



        private readonly (int left, int top) BOARD_PRINT = (0, 0);
        private readonly (int left, int top) COMMUNICATION_LINE = (0, 14);

        private const string INSTRTUCTIONS = "Enter a column number to place your piece. 1-7";
        private const string INVALID_MOVE = "Invalid. Try again. ";
        private const string PLAYER1_WIN = "Player 1 wins! Press space to continue";
        private const string PLAYER2_WIN = "Player 2 wins! Press space to continue";
        private const string TIE = "It's a tie! Press space to continue";

    }
}
