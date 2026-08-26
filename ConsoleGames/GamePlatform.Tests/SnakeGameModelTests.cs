using GamePlatform.Games.Snake;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace GamePlatform.Tests
{
    [TestClass]
    public class SnakeGameModelTests
    {
        [TestMethod]
        public void Direction_IgnoresImmediateReverseDirection()
        {
            SnakeGameModel model = new SnakeGameModel();

            model.Direction = new Vector2(-1, 0);

            Assert.AreEqual(new Vector2(1, 0), model.Direction);
        }

        [TestMethod]
        public void AddToBody_AppendsPreviousTailWithoutMovingAgain()
        {
            SnakeGameModel model = new SnakeGameModel();
            Vector2 previousTail = model.Tail;

            model.Move();
            model.AddToBody(previousTail);

            Assert.AreEqual(2, model.Body.Count);
            Assert.AreEqual(new Vector2(11, 10), model.Head);
            Assert.AreEqual(previousTail, model.Tail);
        }

        [TestMethod]
        public void GenerateFood_OnlyUsesPlayableInteriorLocations()
        {
            SnakeGameModel model = new SnakeGameModel();

            for (int i = 0; i < 100; i++)
            {
                model.GenerateFood(new Vector2(12, 8));

                Assert.IsTrue(model.food.HasValue);
                Assert.IsTrue(model.food.Value.X >= 2);
                Assert.IsTrue(model.food.Value.X < 10);
                Assert.IsTrue(model.food.Value.Y >= 1);
                Assert.IsTrue(model.food.Value.Y < 7);
            }
        }
    }
}
