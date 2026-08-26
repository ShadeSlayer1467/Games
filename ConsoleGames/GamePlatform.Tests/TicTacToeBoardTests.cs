using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe;

namespace GamePlatform.Tests
{
    [TestClass]
    public class TicTacToeBoardTests
    {
        [TestMethod]
        public void IsGameOver_ReturnsCorrectWinnerForMiddleRowWin()
        {
            TicTacToeBoard board = CreateBoard(
                TicTacToeBoard.EmptyCell, TicTacToeBoard.EmptyCell, TicTacToeBoard.EmptyCell,
                'X', 'X', 'X',
                TicTacToeBoard.EmptyCell, TicTacToeBoard.EmptyCell, TicTacToeBoard.EmptyCell);

            bool isGameOver = board.IsGameOver(out char winner);

            Assert.IsTrue(isGameOver);
            Assert.AreEqual('X', winner);
        }

        [TestMethod]
        public void IsGameOver_ReturnsCorrectWinnerForBottomRowWin()
        {
            TicTacToeBoard board = CreateBoard(
                TicTacToeBoard.EmptyCell, TicTacToeBoard.EmptyCell, TicTacToeBoard.EmptyCell,
                TicTacToeBoard.EmptyCell, TicTacToeBoard.EmptyCell, TicTacToeBoard.EmptyCell,
                'O', 'O', 'O');

            bool isGameOver = board.IsGameOver(out char winner);

            Assert.IsTrue(isGameOver);
            Assert.AreEqual('O', winner);
        }

        [TestMethod]
        public void IsGameOver_PreservesWinnerWhenBoardIsFull()
        {
            TicTacToeBoard board = CreateBoard(
                'X', 'X', 'X',
                'O', 'O', 'X',
                'X', 'O', 'O');

            bool isGameOver = board.IsGameOver(out char winner);

            Assert.IsTrue(isGameOver);
            Assert.AreEqual('X', winner);
        }

        [TestMethod]
        public void IsGameOver_ReturnsTieForFullBoardWithoutWinner()
        {
            TicTacToeBoard board = CreateBoard(
                'X', 'O', 'X',
                'X', 'O', 'O',
                'O', 'X', 'X');

            bool isGameOver = board.IsGameOver(out char winner);

            Assert.IsTrue(isGameOver);
            Assert.AreEqual('t', winner);
        }

        private static TicTacToeBoard CreateBoard(params char[] values)
        {
            TicTacToeBoard board = new TicTacToeBoard();
            board.Init();
            for (int i = 0; i < values.Length; i++)
            {
                board[i] = values[i];
            }

            return board;
        }
    }
}
