using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        public void ShouldHaveInitialStartingPositionAtCoordsFreeOfObstacles()
        {
            const int numberOfObstacles = 8;
            var world = new World(3,3);
            world.GenerateObstacleCoordinates(numberOfObstacles, new ObstacleGenerator(new Random()));

            var obstacleCoordinates = world.Obstacles.Select(obstacle => obstacle.Coordinates).ToList();
            var rover = new Rover(new StartingPositionGenerator(new Random()), world);

            Assert.False(obstacleCoordinates.Contains(rover.CurrentCoords));
        }     
        
        [Theory]
        [InlineData(Orientation.North)]
        [InlineData(Orientation.East)]
        [InlineData(Orientation.South)]
        [InlineData(Orientation.West)]

        public void ShouldHaveRandomInitialStartingDirection(Orientation mockStartingOrientation)
        {
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(mockStartingOrientation);
            var rover = new Rover(mockRandomStartingPositionGenerator.Object, new World(3,3));

            var expectedStartingDirection = mockStartingOrientation;
            var actualStartingDirection = rover.CurrentOrientation;
            
            Assert.Equal(expectedStartingDirection, actualStartingDirection);
        }
        
        [Theory]
        [InlineData(Orientation.North, "f", "1,3")]
        [InlineData(Orientation.East, "f", "2,1")]
        [InlineData(Orientation.South, "f", "1,2")]
        [InlineData(Orientation.West, "f", "3,1")]
        public void ShouldMoveForwardOneSpaceTowardFacingDirection_OnSingleForwardCommand(Orientation mockStartingOrientation, string forwardCommand, string expectedCoord)
        {
            var world = new World(3,3);
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoordsIn(world)).Returns(new Coordinates(1,1));
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(mockStartingOrientation);
            
            var rover = new Rover(mockRandomStartingPositionGenerator.Object, world);
            var expectedOutput = $"Moved forward to {expectedCoord}\n";
            var actualOutput = rover.TranslateCommands(forwardCommand);
            
            Assert.Equal(expectedOutput, actualOutput );
        }
        
        [Theory]
        [InlineData(Orientation.North, "b", "1,2")]
        [InlineData(Orientation.East, "b", "3,1")]
        [InlineData(Orientation.South, "b", "1,3")]
            [InlineData(Orientation.West, "b", "2,1")]

        public void ShouldMoveBackwardOneSpaceFromFacingDirection_OnSingleBackwardCommand(Orientation startingOrientation, string backwardCommand, string expectedCoord)
        {
            var world = new World(3,3);
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoordsIn(world)).Returns(new Coordinates(1,1));
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(startingOrientation);
            
            var rover = new Rover(mockRandomStartingPositionGenerator.Object, world);
            var expectedOutput = $"Moved backward to {expectedCoord}\n";
            var actualOutput = rover.TranslateCommands(backwardCommand);
            
            Assert.Equal(expectedOutput, actualOutput);
        }

        [Theory]
        [InlineData(Orientation.North, "l", "Turned left towards West\n")]
        [InlineData(Orientation.East, "l", "Turned left towards North\n")]
        [InlineData(Orientation.South, "l", "Turned left towards East\n")]
        [InlineData(Orientation.West, "l", "Turned left towards South\n")]

        public void ShouldTurnLeft_OnSingleTurnLeftCommand(Orientation startingOrientation, string turnLeftCommand,
            string expectedOutput)
        {
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(startingOrientation);

            var rover = new Rover(mockRandomStartingPositionGenerator.Object, new World(3,3));
            var actualOutput = rover.TranslateCommands(turnLeftCommand);
            
            Assert.Equal(expectedOutput, actualOutput);
        }
        
        [Theory]
        [InlineData(Orientation.North, "r", "Turned right towards East\n")]
        [InlineData(Orientation.East, "r", "Turned right towards South\n")]
        [InlineData(Orientation.South, "r", "Turned right towards West\n")]
        [InlineData(Orientation.West, "r", "Turned right towards North\n")]

        public void ShouldTurnRight_OnSingleTurnRightCommand(Orientation startingOrientation, string turnLeftCommand,
            string expectedOutput)
        {
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(startingOrientation);
            var rover = new Rover(mockRandomStartingPositionGenerator.Object, new World(3,3));

            var actualOutput = rover.TranslateCommands(turnLeftCommand);
            
            Assert.Equal(expectedOutput, actualOutput);
        }

        [Theory]
        [InlineData("frflfrrb", new[]{1,2}, Orientation.West)]
        [InlineData("rFFlf", new[]{2,3}, Orientation.East)]
        [InlineData("fffblfrr", new[]{3,3}, Orientation.South)]
        public void ShouldCompleteSequenceOfCommands_OnMultipleCommands(string commands, int[] expectedPosition, Orientation expectedOrientation)
        {
            var world = new World(3,3);
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoordsIn(world)).Returns(new Coordinates(1,1));
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(Orientation.East);
            var rover = new Rover(mockRandomStartingPositionGenerator.Object, world);
            
            rover.TranslateCommands(commands); ;
            var actualPosition = new []{rover.CurrentCoords.X, rover.CurrentCoords.Y};
            var actualDirection = rover.CurrentOrientation;
            
            Assert.Equal(expectedPosition, actualPosition);
            Assert.Equal(expectedOrientation, actualDirection);
        }
        
        [Fact]
        public void shouldNotMoveAndReportObstacleIfAttemptingToMoveToCoordsWithObstacle()
        {

            const int numberOfObstacles = 1;
            var world = new World(3,3);
            var mockObstacleCoordsGenerator = new Mock<IObstacleGenerator>();
            var obstacleCoord = world.Coordinates.Find(coord => coord.X == 1 && coord.Y == 1);
            mockObstacleCoordsGenerator.Setup(generator => generator
                    .Generate(world.Coordinates, numberOfObstacles))
                .Returns( new List<Obstacle> {new Obstacle(obstacleCoord)} );
            
            world.GenerateObstacleCoordinates(numberOfObstacles, mockObstacleCoordsGenerator.Object);
            
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoordsIn(world)).Returns(new Coordinates(1,2));
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(Orientation.North);
            var rover = new Rover(mockRandomStartingPositionGenerator.Object, world);
            
            
            const string expected = "Obstacle detected! Cannot move to coord 1,1\n";
            var actual = rover.TranslateCommands("f");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void shouldMoveToLastPossiblePointandReportObstacleForSequenceOfCommands()
        {
            const int numberOfObstacles = 1;
            var world = new World(3,3);
            var mockObstacleCoordsGenerator = new Mock<IObstacleGenerator>();
            var obstacleCoord = world.Coordinates.Find(coord => coord.X == 1 && coord.Y == 1);
            mockObstacleCoordsGenerator.Setup(generator => generator
                    .Generate(world.Coordinates, numberOfObstacles))
                .Returns( new List<Obstacle> {new Obstacle(obstacleCoord)} );
            
            world.GenerateObstacleCoordinates(numberOfObstacles, mockObstacleCoordsGenerator.Object);
            
            var mockRandomStartingPositionGenerator = new Mock<IStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoordsIn(world)).Returns(new Coordinates(1,2));
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(Orientation.East);
            var rover = new Rover(mockRandomStartingPositionGenerator.Object, world);

            const string expected =
                "Moved forward to 2,2\nTurned left towards North\nMoved forward to 2,1\nTurned left towards West\nObstacle detected! Cannot move to coord 1,1\n";
            var actual = rover.TranslateCommands("flflfffffffff"); //should encounter obstacle at third forward command, and ignore any subsequent commands 
            
            Assert.Equal(expected, actual);
        }
    }
    
    
    
}