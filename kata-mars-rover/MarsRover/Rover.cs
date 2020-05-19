namespace MarsRover.Tests
{
    public class Rover
    {
        private const int XMax = 3;
        private const int YMax = 3;
        
        public int[] CurrentCoords { get; set; }
        public Direction CurrentDirection { get; set; }

        public Rover(IRandomStartingPositionGenerator randomStartingPositionGenerator)
        {
            CurrentCoords = randomStartingPositionGenerator.GenerateStartingCoords(XMax, YMax);
            CurrentDirection = randomStartingPositionGenerator.GenerateStartingDirection();
        }
    }
}