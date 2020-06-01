using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Schema;
using MarsRover.Models;
using Moq;
using Xunit;

namespace MarsRover.Tests
{
    public class RoverTests
    {
     
        [Fact]
        public void ShouldGenerateStartingCoordsWithinWorldBounds()
        {
            var world = new World(3,4);
            var mockRandom= new Mock<IRandom>();
            mockRandom.Setup(random =>  random.Next(It.IsAny<int>(),It.IsAny<int>())).Returns(3);
            var generator = new StartingPositionGenerator(mockRandom.Object);
            
            var expectedStartingPoint = world.Coordinates[3];
            var actualStartingPoint = generator.GenerateStartingCoordsIn(world);
            
            Assert.Equal(expectedStartingPoint, actualStartingPoint);
            mockRandom.Verify(v =>v.Next(0,world.Coordinates.Count)); //generated coordinate must be within the available coords

        }
        
        //tests that we use the value returned by the starting point generator
        [Fact]
        public void ShouldHaveRandomInitialStartingPositionAtCoordsFreeOfObstacles()
        {
            const int numberOfObstacles = 8;
            var world = new World(3,3);
            var mockObstacleCoordsGenerator = new Mock<IObstacleCoordinateGenerator>();
            var obstacleCoord1 = world.Coordinates.Find(coord => coord.X == 1 && coord.Y == 1);
            var obstacleCoord2 = world.Coordinates.Find(coord => coord.X == 1 && coord.Y == 2);
            var obstacleCoord3 = world.Coordinates.Find(coord => coord.X == 1 && coord.Y == 3);
            var obstacleCoord4 = world.Coordinates.Find(coord => coord.X == 2 && coord.Y == 1);
            var obstacleCoord5 = world.Coordinates.Find(coord => coord.X == 2 && coord.Y == 2);
            var obstacleCoord6 = world.Coordinates.Find(coord => coord.X == 2 && coord.Y == 3);
            var obstacleCoord7 = world.Coordinates.Find(coord => coord.X == 3 && coord.Y == 1);
            var obstacleCoord8 = world.Coordinates.Find(coord => coord.X == 3 && coord.Y == 2);
            
            mockObstacleCoordsGenerator.Setup(generator => generator
                    .Generate(world.Coordinates, numberOfObstacles))
                .Returns( new List<Coordinates>
                {
                    obstacleCoord1, obstacleCoord2, obstacleCoord3,
                    obstacleCoord4, obstacleCoord5, obstacleCoord6,
                    obstacleCoord7, obstacleCoord8
                } );
            
            world.GenerateObstacleCoordinates(numberOfObstacles, mockObstacleCoordsGenerator.Object);
            var rover = new Rover(new StartingPositionGenerator(new Random()), world);

            var expectedStartingCoords = world.Coordinates.Find(coord => coord.X == 3 && coord.Y == 3);
            var actualStartingCoords = rover.CurrentCoords;

            Assert.Equal(expectedStartingCoords, actualStartingCoords);
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
            var rover = new Rover(mockRandomStartingPositionGenerator.Object, new World(3,3));

            var expectedStartingDirection = mockStartingDirection;
            var actualStartingDirection = rover.CurrentDirection;
            
            Assert.Equal(expectedStartingDirection, actualStartingDirection);
        }
        
        [Theory]
        [InlineData(Direction.North, "f", new[]{1,3})]
        [InlineData(Direction.East, "f", new[]{2,1})]
        [InlineData(Direction.South, "f", new[]{1,2})]
        [InlineData(Direction.West, "f", new[]{3,1})]
        public void ShouldMoveForwardOneSpaceTowardFacingDirection_OnSingleForwardCommand(Direction mockStartingDirection, string forwardCommand, int[] expected)
        {
            var world = new World(3,3);
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoordsIn(world)).Returns(new Coordinates(1,1));
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(mockStartingDirection);
            
            var rover = new Rover(mockRandomStartingPositionGenerator.Object, world);
            rover.TranslateCommands(forwardCommand);
            var actual = new []{rover.CurrentCoords.X, rover.CurrentCoords.Y};
            
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData(Direction.North, "b", new []{1,2})]
        [InlineData(Direction.East, "b", new []{3,1})]
        [InlineData(Direction.South, "b", new []{1,3})]
        [InlineData(Direction.West, "b", new []{2,1})]

        public void ShouldMoveBackwardOneSpaceFromFacingDirection_OnSingleBackwardCommand(Direction startingDirection, string backwardCommand, int[] expected)
        {
            var world = new World(3,3);
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoordsIn(world)).Returns(new Coordinates(1,1));
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(startingDirection);
            
            var rover = new Rover(mockRandomStartingPositionGenerator.Object, world);
            rover.TranslateCommands(backwardCommand);
            var actual = new []{rover.CurrentCoords.X, rover.CurrentCoords.Y};
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

            var rover = new Rover(mockRandomStartingPositionGenerator.Object, new World(3,3));
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
            var rover = new Rover(mockRandomStartingPositionGenerator.Object, new World(3,3));
            
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
            var world = new World(3,3);
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoordsIn(world)).Returns(new Coordinates(1,1));
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(Direction.East);
            var rover = new Rover(mockRandomStartingPositionGenerator.Object, world);
            
            rover.TranslateCommands(commands); ;
            var actualPosition = new []{rover.CurrentCoords.X, rover.CurrentCoords.Y};
            var actualDirection = rover.CurrentDirection;
            
            Assert.Equal(expectedPosition, actualPosition);
            Assert.Equal(expectedDirection, actualDirection);
        }
    }
}