using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _2048Game;

namespace GamePlatform.Tests
{
    [TestClass]
    public class Board2048Tests
    {
        [TestMethod]
        public void MoveLeft_MergesEachPairOnlyOnce()
        {
            _2048Board board = CreateBoard(
                2, 2, 2, 2,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0);

            bool moved = board.MoveLeft();

            Assert.IsTrue(moved);
            CollectionAssert.AreEqual(new[]
            {
                4, 4, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            }, board.Board);
        }

        [TestMethod]
        public void MoveRight_CompressesAndMergesTowardTheRightEdge()
        {
            _2048Board board = CreateBoard(
                2, 0, 2, 2,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0);

            bool moved = board.MoveRight();

            Assert.IsTrue(moved);
            CollectionAssert.AreEqual(new[]
            {
                0, 0, 2, 4,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            }, board.Board);
        }

        [TestMethod]
        public void MoveLeft_ReturnsFalseWhenBoardDoesNotChange()
        {
            _2048Board board = CreateBoard(
                2, 4, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0);
            int[] originalBoard = board.Board.ToArray();

            bool moved = board.MoveLeft();

            Assert.IsFalse(moved);
            CollectionAssert.AreEqual(originalBoard, board.Board);
        }

        [TestMethod]
        public void GenerateNewNumbers_AddsOneTilePerCall()
        {
            _2048Board board = new _2048Board();

            board.GenerateNewNumbers(new Random(0));
            board.GenerateNewNumbers(new Random(1));

            Assert.AreEqual(2, board.Board.Count(value => value != 0));
            Assert.IsTrue(board.Board.All(value => value == 0 || value == 2 || value == 4));
        }

        [TestMethod]
        public void GenerateNewNumbers_AddsOneTileToAnOpenBoard()
        {
            _2048Board board = new _2048Board();

            board.GenerateNewNumbers(new Random(0));

            Assert.AreEqual(1, board.Board.Count(value => value != 0));
            Assert.IsTrue(board.Board.Any(value => value == 2 || value == 4));
        }

        private static _2048Board CreateBoard(params int[] values)
        {
            _2048Board board = new _2048Board();
            Array.Copy(values, board.Board, values.Length);
            return board;
        }
    }
}
