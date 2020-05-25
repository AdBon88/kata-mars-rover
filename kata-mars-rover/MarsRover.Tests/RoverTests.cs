using System.Runtime.InteropServices.ComTypes;
using System.Xml.Schema;
using Moq;
using Xunit;

namespace MarsRover.Tests
{
    public class RoverTests
    {
        //TODO Research the mock Verify method
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
        public void ShouldMoveForwardOneSpaceTowardFacingDirection_OnSingleForwardCommand(Direction mockStartingDirection, string forwardCommand, int[] expected)
        {
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoords(3,3)).Returns(new []{1,1});
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(mockStartingDirection);
            
            var rover = new Rover(mockRandomStartingPositionGenerator.Object);
            rover.TranslateCommands(forwardCommand);
            var actual = rover.CurrentCoords;
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData(Direction.North, "b", new []{1,2})]
        [InlineData(Direction.East, "b", new []{3,1})]
        [InlineData(Direction.South, "b", new []{1,3})]
        [InlineData(Direction.West, "b", new []{2,1})]

        public void ShouldMoveBackwardOneSpaceFromFacingDirection_OnSingleBackwardCommand(Direction startingDirection, string backwardCommand, int[] expected)
        {
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoords(3,3)).Returns(new []{1,1});
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(startingDirection);
            
            var rover = new Rover(mockRandomStartingPositionGenerator.Object);
            rover.TranslateCommands(backwardCommand);
            var actual = rover.CurrentCoords;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(Direction.North, "l", Direction.West)]
        [InlineData(Direction.East, "l", Direction.North)]
        [InlineData(Direction.South, "l", Direction.East)]
        [InlineData(Direction.West, "l", Direction.South)]

        public void ShouldTurnLeft_OnSingleTurnLeftCommand(Direction startingDirection, string turnLeftCommand,
            Direction expected)
        {
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(startingDirection);

            var rover = new Rover(mockRandomStartingPositionGenerator.Object);
            rover.TranslateCommands(turnLeftCommand);
            
            var actual = rover.CurrentDirection;
            
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData(Direction.North, "r", Direction.East)]
        [InlineData(Direction.East, "r", Direction.South)]
        [InlineData(Direction.South, "r", Direction.West)]
        [InlineData(Direction.West, "r", Direction.North)]

        public void ShouldTurnRight_OnSingleTurnRightCommand(Direction startingDirection, string turnLeftCommand,
            Direction expected)
        {
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(startingDirection);
            var rover = new Rover(mockRandomStartingPositionGenerator.Object);
            
            rover.TranslateCommands(turnLeftCommand);
            var actual = rover.CurrentDirection;
            
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("frflfrrb", new[]{1,2}, Direction.West)]
        [InlineData("rFFlf", new[]{2,3}, Direction.East)]
        [InlineData("fffblfrr", new[]{3,3}, Direction.South)]
        public void ShouldCompleteSequenceOfCommands_OnMultipleCommands(string commands, int[] expectedPosition, Direction expectedDirection)
        {
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoords(3,3)).Returns(new []{1,1});
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(Direction.East);
            var rover = new Rover(mockRandomStartingPositionGenerator.Object);
            
            rover.TranslateCommands(commands); ;
            var actualPosition = rover.CurrentCoords;
            var actualDirection = rover.CurrentDirection;
            
            Assert.Equal(expectedPosition, actualPosition);
            Assert.Equal(expectedDirection, actualDirection);
        }
    }
}