using System.Linq;

namespace TicTacToe
{
    internal class TicTacToeBoard
    {
        private char[] Board;

        internal void Init()
        {
            Board = new char[9] { EmptyCell, EmptyCell, EmptyCell, EmptyCell, EmptyCell, EmptyCell, EmptyCell, EmptyCell, EmptyCell };
        }
        internal char this[int index]
        {
            get => Board[index];
            set => Board[index] = value;
        }

        internal bool IsGameOver(out char winner)
        {
            winner = 'f';
            for (int i = 0; i < 3; i++)
            {
                if (Board[i] == Board[i + 3] && Board[i] == Board[i + 6] && Board[i] != EmptyCell) { winner = Board[i]; return true; }
                if (Board[i * 3] == Board[i * 3 + 1] && Board[i * 3] == Board[i * 3 + 2] && Board[i * 3] != EmptyCell) { winner = Board[i * 3]; return true; }
            }
            if (Board[0] == Board[4] && Board[0] == Board[8] && Board[0] != EmptyCell) { winner = Board[0]; return true; }
            if (Board[2] == Board[4] && Board[2] == Board[6] && Board[2] != EmptyCell) { winner = Board[2]; return true; }
            if (!Board.Contains(EmptyCell)) { winner = 't'; return true; }
            return false;
        }

        internal const char EmptyCell = ',';
    }
}
