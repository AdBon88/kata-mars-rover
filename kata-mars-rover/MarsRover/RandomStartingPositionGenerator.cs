using System;
using MarsRover.Tests;

namespace MarsRover
{
    public class RandomStartingPositionGenerator : IRandomStartingPositionGenerator
    {
        private readonly Random _random = new Random();
        public int[] GenerateStartingCoords(int xMax, int yMax)
        {
           
            var x = _random.Next(0, xMax + 1);
            var y = _random.Next(0, yMax + 1);
            
            return new []{x, y};
        }

        public Direction GenerateStartingDirection()
        {
            var numberOfDirections = Direction.GetNames(typeof(Direction)).Length;
            var randomDirection = (Direction) _random.Next(numberOfDirections);
            return randomDirection;
        }
    }
}