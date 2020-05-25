namespace MarsRover.Tests
{
    public struct Square
    {
        public Coordinates Coordinates { get; set; }

        public SquareContents Contents { get; set; }

        public Square(Coordinates coordinates)
        {
            Coordinates = coordinates;
            Contents = SquareContents.Empty;
        }
    }
}