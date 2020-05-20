using System.Runtime.InteropServices.ComTypes;
using System.Xml.Schema;
using Moq;
using Xunit;

namespace MarsRover.Tests
{
    public class RoverTests
    {
        [Fact]
        public void ShouldGenerateRandomStartingCoordsWithinBounds()
        {
            var mockRandom= new Mock<IRandom>();
            mockRandom.Setup(random =>  random.Next(It.IsAny<int>(),It.IsAny<int>())).Returns(3);
            var generator = new StartingPositionGenerator(mockRandom.Object);

            var expectedStartingPoint = new[] {3, 3};
            var actualStartingPoint = generator.GenerateStartingCoords(3,4);
            
            Assert.Equal(expectedStartingPoint, actualStartingPoint);
            mockRandom.Verify(v =>v.Next(1,4)); //random number must be between 1 and xMax + 1
            mockRandom.Verify(v =>v.Next(1,5)); //random number must be between 1 and yMax + 1
        }
        
        //tests that we use the value returned by the starting point generator
        [Fact]
        public void ShouldHaveRandomInitialStartingPosition()
        {
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoords(3,3)).Returns(new []{2,2});
            var rover = new Rover(mockRandomStartingPositionGenerator.Object);

            var expectedStartingPoint = new[] {2, 2};
            var actualStartingPoint = rover.CurrentCoords;
            
            Assert.Equal(expectedStartingPoint, actualStartingPoint);
        }     
        
        [Theory]
        [InlineData(Direction.North)]
        [InlineData(Direction.East)]
        [InlineData(Direction.South)]
        [InlineData(Direction.West)]

        public void ShouldHaveRandomInitialStartingDirection(Direction mockStartingDirection)
        {
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(mockStartingDirection);
            var rover = new Rover(mockRandomStartingPositionGenerator.Object);

            var expectedStartingDirection = mockStartingDirection;
            var actualStartingDirection = rover.CurrentDirection;
            
            Assert.Equal(expectedStartingDirection, actualStartingDirection);
        }
        
        [Theory]
        [InlineData(Direction.North, "f", new []{1,3})]
        [InlineData(Direction.East, "f", new []{2,1})]
        [InlineData(Direction.South, "f", new []{1,2})]
        [InlineData(Direction.West, "f", new []{3,1})]

        public void ShouldMoveForwardOneSpaceOnSingleMoveCommand(Direction mockStartingDirection, string command, int[] expected)
        {
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoords(3,3)).Returns(new []{1,1});
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(mockStartingDirection);
            
            var rover = new Rover(mockRandomStartingPositionGenerator.Object);
            rover.ProcessCommand(command);
            var actual = rover.CurrentCoords;
            Assert.Equal(expected, actual);
        }
        
        // [Fact]
        // public void ShouldChangeDirectionOnCommand()
        // {
        //     var mockRandomStartingPositionGenerator = new Mock<IRandomStartingPositionGenerator>();
        //     mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(Direction.East);
        //     
        //     var rover = new Rover(mockRandomStartingPositionGenerator.Object);
        //     rover.ProcessInput("l")
        //
        // }
    }
}