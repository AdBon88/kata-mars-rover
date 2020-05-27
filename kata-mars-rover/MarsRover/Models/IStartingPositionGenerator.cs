using MarsRover.Tests;

namespace MarsRover
{
    public interface IStartingPositionGenerator
    {
        public Coordinates GenerateStartingCoordsIn(World world);
        public Direction GenerateStartingDirection();
    }
}