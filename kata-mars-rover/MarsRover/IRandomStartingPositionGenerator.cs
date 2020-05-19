using MarsRover.Tests;

namespace MarsRover
{
    public interface IRandomStartingPositionGenerator
    {
        public int[] GenerateStartingCoords(int xMax, int yMax);
        public Direction GenerateStartingDirection();
    }
}