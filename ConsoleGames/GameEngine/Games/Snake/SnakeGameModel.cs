using GameEngine;
using System;
using System.Collections.Generic;
using System.Numerics;

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
        internal void AddToBody(Vector2 newTail)
        {
            Body.Add(newTail);
        }
        internal void GenerateFood(Vector2 Border)
        {
            Random random = RandomSingleton.Instance;
            List<Vector2> openLocations = new List<Vector2>();

            for (int x = PLAYABLE_MIN_X; x < (int)Border.X - RIGHT_BORDER_WIDTH; x++)
            {
                for (int y = PLAYABLE_MIN_Y; y < (int)Border.Y - BOTTOM_BORDER_HEIGHT; y++)
                {
                    Vector2 location = new Vector2(x, y);
                    if (!Body.Contains(location))
                    {
                        openLocations.Add(location);
                    }
                }
            }

            food = openLocations.Count == 0 ? (Vector2?)null : openLocations[random.Next(openLocations.Count)];
        }

        private const int PLAYABLE_MIN_X = 2;
        private const int PLAYABLE_MIN_Y = 1;
        private const int RIGHT_BORDER_WIDTH = 2;
        private const int BOTTOM_BORDER_HEIGHT = 1;
    }
}
