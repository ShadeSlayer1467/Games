using System.Linq;

namespace TicTacToe
{
    internal class TicTacToeBoard
    {
        private char[] Board;

        internal void Init()
        {
            Board = new char[9] { ',', ',', ',', ',', ',', ',', ',', ',', ',' };
        }
        internal char this[int index]
        {
            get => Board[index];
            set => Board[index] = value;
        }

        internal bool IsGameOver(out char winner)
        {
            winner = 'f';
            if (Board.Contains(',') == false) winner = 't';
            for (int i = 0; i < 3; i++)
            {
                if (Board[i] == Board[i + 3] && Board[i] == Board[i + 6] && Board[i] != ',') { winner = Board[i]; return true; }
                if (Board[i * 3] == Board[i * 3 + 1] && Board[i * 3] == Board[i * 3 + 2] && Board[i * 3] != ',') { winner = Board[i]; return true; }
            }
            if (Board[0] == Board[4] && Board[0] == Board[8] && Board[0] != ',') winner = Board[0];
            if (Board[2] == Board[4] && Board[2] == Board[6] && Board[2] != ',') winner = Board[2];
            return false;
        }
    }
}
