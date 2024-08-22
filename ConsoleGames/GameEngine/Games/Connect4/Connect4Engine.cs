using AbstractGame;
using GamePlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    public class Connect4Engine : ConsoleGame
    {
        private Board board = null;
        private int currentPlayer;
        private int winner;
        private object lockObject = new object();
        private Slot lastPlacedSlot;

        public Connect4Engine(){}
        public override void InitializeGame()
        {
            if (board == null) board = new Board(ROWS, COLUMNS);
            PrintBoard();
            currentPlayer = PLAYER1;
            winner = 0;
        }
        public override void RunGame()
        {
            do PlayRound(); while (!CheckForWinner());
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
            bool validMove = false;
            // repeat until a valid move is made
            do
            {
                column = GetPlayerInput();
                Slot newPiece;
                validMove = board.TryPlacePiece(column - 1, currentPlayer, out newPiece);
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
        private bool CheckForWinner()
        {
            var diagonals = board.GetPieceDiagonal(lastPlacedSlot);

            return (ContainsWinner(board.Row(lastPlacedSlot.Row))) ? true :
                (ContainsWinner(board.Column(lastPlacedSlot.Column))) ? true :
                (ContainsWinner(diagonals.asc)) ? true :
                (ContainsWinner(diagonals.desc)) ? true : 
                (board.slots.Where(s => s.Player == Slot.DEFAULT_PLAYER).Count() == 0) ? true : false;
        }
        private bool ContainsWinner(Slot[] slots)
        {
            if (slots.Length < Board.WIN_CONDITION) return false;
            int inARow = 1;
            for (int i = 0; i < slots.Length - 1; i++)
            {
                if (!slots[i].IsOpenSlot && slots[i].Player == slots[i + 1].Player)
                {
                    inARow++;
                    if (inARow == Board.WIN_CONDITION)
                    {
                        winner = slots[i].Player;
                        return true;
                    }
                } 
                else inARow = 1;
            }
            return false;
        }
        private void ClearConsoleBuffer(int top)
        {
            GameConsoleUI.ClearConsoleLineBuffer(top);
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
