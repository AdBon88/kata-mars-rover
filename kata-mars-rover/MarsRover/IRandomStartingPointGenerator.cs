using System.Collections.Generic;

namespace MarsRover
{
    public interface IRandomStartingPointGenerator
    {
        public int[] Generate(int xMax, int yMax);
    }
}