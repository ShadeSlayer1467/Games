using Connect4;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GamePlatform.Tests
{
    [TestClass]
    public class Connect4BoardTests
    {
        [TestMethod]
        public void Constructor_AssignsEachSlotItsActualColumn()
        {
            Connect4Board board = new Connect4Board();

            for (int row = 0; row < board.ROWS; row++)
            {
                for (int column = 0; column < board.COLUMNS; column++)
                {
                    Assert.AreEqual(column, board[row, column].Column);
                }
            }
        }

        [TestMethod]
        public void TryPlacePiece_StacksPiecesFromTheBottomOfTheColumn()
        {
            Connect4Board board = new Connect4Board();

            Assert.IsTrue(board.TryPlacePiece(6, 1, out Slot firstPiece));
            Assert.IsTrue(board.TryPlacePiece(6, 2, out Slot secondPiece));

            Assert.AreEqual(5, firstPiece.Row);
            Assert.AreEqual(6, firstPiece.Column);
            Assert.AreEqual(4, secondPiece.Row);
            Assert.AreEqual(6, secondPiece.Column);
        }

        [TestMethod]
        public void InARow_DetectsHorizontalWins()
        {
            Connect4Board board = new Connect4Board();
            SetPiece(board, 5, 0, 1);
            SetPiece(board, 5, 1, 1);
            Slot lastPiece = SetPiece(board, 5, 2, 1);
            SetPiece(board, 5, 3, 1);

            bool isGameOver = board.InARow(lastPiece, out int winner);

            Assert.IsTrue(isGameOver);
            Assert.AreEqual(1, winner);
        }

        [TestMethod]
        public void InARow_DetectsVerticalWins()
        {
            Connect4Board board = new Connect4Board();
            SetPiece(board, 5, 0, 2);
            SetPiece(board, 4, 0, 2);
            Slot lastPiece = SetPiece(board, 3, 0, 2);
            SetPiece(board, 2, 0, 2);

            bool isGameOver = board.InARow(lastPiece, out int winner);

            Assert.IsTrue(isGameOver);
            Assert.AreEqual(2, winner);
        }

        [TestMethod]
        public void InARow_DetectsDescendingDiagonalWhenLastPieceIsInTheMiddle()
        {
            Connect4Board board = new Connect4Board();
            SetPiece(board, 1, 1, 1);
            Slot lastPiece = SetPiece(board, 2, 2, 1);
            SetPiece(board, 3, 3, 1);
            SetPiece(board, 4, 4, 1);

            bool isGameOver = board.InARow(lastPiece, out int winner);

            Assert.IsTrue(isGameOver);
            Assert.AreEqual(1, winner);
        }

        [TestMethod]
        public void InARow_DetectsAscendingDiagonalWhenLastPieceIsInTheMiddle()
        {
            Connect4Board board = new Connect4Board();
            SetPiece(board, 4, 1, 2);
            Slot lastPiece = SetPiece(board, 3, 2, 2);
            SetPiece(board, 2, 3, 2);
            SetPiece(board, 1, 4, 2);

            bool isGameOver = board.InARow(lastPiece, out int winner);

            Assert.IsTrue(isGameOver);
            Assert.AreEqual(2, winner);
        }

        private static Slot SetPiece(Connect4Board board, int row, int column, int player)
        {
            Slot slot = board[row, column];
            slot.Player = player;
            return slot;
        }
    }
}
