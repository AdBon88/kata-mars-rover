using MarsRover;

namespace MarsRover
{
    public interface IStartingPositionGenerator
    {
        public Coordinates GenerateStartingCoordsIn(World world);
        public Orientation GenerateStartingDirection();
    }
}