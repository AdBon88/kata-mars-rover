namespace MarsRover.Tests
{
    public class Rover
    {
        private const int XMax = 3;
        private const int YMax = 3;
        
        public int[] CurrentCoords { get; set; }
        public Rover(IRandomStartingPointGenerator randomStartingPointGenerator)
        {
            CurrentCoords = randomStartingPointGenerator.Generate(XMax, YMax);
        }
    }
}