using GameEngine;
using GamePlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GamePlatform.Games.Snake
{
    internal class SnakeGameModel
    {
        internal List<Vector2> Body { get; set; }
        private Vector2 direction;
        internal Vector2 Direction
        {
            get { return direction; }
            set
            {
                if (value.X != 0 && value.Y != 0) throw new Exception("Invalid Direction"); ;

                if (value.X != 0 && direction.X == 0) direction = value;
                else if (value.Y != 0 && direction.Y == 0) direction = value;
            }
        }
        internal Vector2 Head => Body[0];
        internal Vector2 Tail => Body[Body.Count - 1];
        internal Vector2? food = null;
        internal SnakeGameModel()
        {
            Body = new List<Vector2> { new Vector2(10, 10) };
            Direction = new Vector2(1, 0);
        }
        internal void Move()
        {
            for (int i = Body.Count - 1; i > 0; i--)
            {
                Body[i] = Body[i - 1];
            }
            Body[0] += Direction;
        }
        internal void AddToBody()
        {
            var tail = Tail;
            Move();
            Body.Add(tail);
        }
        internal void GenerateFood(Vector2 Border)
        {
            Random random = RandomSingleton.Instance;

            while (true)
            {
                food = new Vector2(random.Next(1, (int)Border.X-2), random.Next(1, (int)Border.Y-2));
                if (!Body.Contains(food.Value)) break;
            }
        }
    }
}
