using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
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
            return (Direction)_random.Next(3);
        }
    }
}