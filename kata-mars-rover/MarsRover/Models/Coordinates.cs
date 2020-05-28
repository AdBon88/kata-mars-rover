namespace MarsRover.Tests
{
    public class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool HasObstacle { get; set; } = false;
        

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool isEqualTo(Coordinates coords)
        {
            return X == coords.X && Y == coords.Y;
        }
    }
}