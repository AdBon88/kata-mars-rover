

namespace MarsRover.Models
{
    public class Obstacle
    {
        public Coordinates Coordinates { get; }

        public Obstacle(Coordinates coordinates)
        {
            Coordinates = coordinates;
        }
    }
}