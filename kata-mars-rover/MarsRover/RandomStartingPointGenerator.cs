using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;

namespace MarsRover
{
    public class RandomStartingPointGenerator : IRandomStartingPointGenerator
    {
        public int[] Generate(int xMax, int yMax)
        {
            var random = new Random();
            var xCoord = random.Next(0, xMax + 1);
            var yCoord = random.Next(0, yMax + 1);
            
            return new int[]{xCoord, yCoord};
        }
    }
}