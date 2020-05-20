using MarsRover.Tests;

namespace MarsRover
{
    public interface IStartingPositionGenerator
    {
        public int[] GenerateStartingCoords(int xMax, int yMax);
        public Direction GenerateStartingDirection();
    }
}