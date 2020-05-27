using System;
using System.Collections.Generic;
using System.Security.Principal;
using MarsRover.Models;
using Moq;
using Xunit;
using Random = MarsRover.Models.Random;

namespace MarsRover.Tests
{
    public class GridTests
    {
        [Fact]
        public void ShouldGenerateCoordsForObstacles()
        {
            const int worldLength = 3;
            const int worldHeight = 3;
            const int numberOfObstacles = 3;
            
            var mockObstacleCoordinateGenerator = new Mock<IObstacleCoordinateGenerator>();
            mockObstacleCoordinateGenerator.Setup(generator =>  generator.Generate(worldLength, worldLength, numberOfObstacles))
                .Returns(new List<Coordinates>{ new Coordinates(1,1), new Coordinates(2,2), new Coordinates(3,3) });
            
            var world = new World(worldLength, worldHeight);
            world.GeneratorObstacleCoordinates(numberOfObstacles, mockObstacleCoordinateGenerator.Object);
            var expectedObstacleCoords = new List<Coordinates>{ new Coordinates(1,1), new Coordinates(2,2), new Coordinates(3,3) };
            var actualObstacleCoords = world.ObstacleCoordinates;
            
            Assert.Equal(expectedObstacleCoords, actualObstacleCoords);
        }
        
        // [Fact]
        // public void ShouldGenerateStartingCoordsWithoutObstacleForRover()
        // {
        //     var grid = new Grid(2,2);
        //     
        //     var mockObstacleCoordinateGenerator = new Mock<IObstacleCoordinateGenerator>();
        //     mockObstacleCoordinateGenerator.Setup(generator =>  generator.Generate(grid.Squares, 3))
        //         .Returns(new List<Coordinates>(){new Coordinates(1,1), new Coordinates(1,2), new Coordinates(2,1)});
        //     
        //     grid.PlaceRoverAtAvailableGeneratedCoordinate(new StartingPositionGenerator(new Random()));
        //     const SquareContents expected = SquareContents.Rover;
        //     
        //     Assert.Equal(expected, grid.GetContentsAt( new Coordinates(3,3) ));
        // }
    }
}