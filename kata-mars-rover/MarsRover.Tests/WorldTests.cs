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
        // [Fact]
        // public void ShouldUseGeneratedCoordsForObstacles()
        // {
        //     const int worldLength = 3;
        //     const int worldHeight = 3;
        //     const int numberOfObstacles = 3;
        //     
        //     var mockObstacleCoordinateGenerator = new Mock<IObstacleCoordinateGenerator>();
        //     mockObstacleCoordinateGenerator.Setup(generator =>  generator.Generate(worldLength, worldLength, numberOfObstacles))
        //         .Returns(new List<Coordinates>{ new Coordinates(1,1), new Coordinates(2,2), new Coordinates(3,3) });
        //     
        //     var world = new World(worldLength, worldHeight);
        //     world.GeneratorObstacleCoordinates(numberOfObstacles, mockObstacleCoordinateGenerator.Object);
        //     var expectedObstacleCoords = new List<Coordinates>{ new Coordinates(1,1), new Coordinates(2,2), new Coordinates(3,3) };
        //     var actualObstacleCoords = world.ObstacleCoordinates;
        //     
        //     Assert.Equal(expectedObstacleCoords, actualObstacleCoords);
        // }
        
        [Fact]
        public void ShouldGenerateAllCoords()
        {
            var world = new World(2,2);
            world.GenerateCoordinates();

            var expectedCoord1 = new[] {1, 1};
            var expectedCoord2 = new[] {2, 1};
            var expectedCoord3 = new[] {1, 2};
            var expectedCoord4 = new[] {2, 2};

            var actualCoord1 = new[] {world.Coordinates[0].X, world.Coordinates[0].Y};
            var actualCoord2 = new[] {world.Coordinates[1].X, world.Coordinates[1].Y};
            var actualCoord3 = new[] {world.Coordinates[2].X, world.Coordinates[2].Y};
            var actualCoord4 = new[] {world.Coordinates[3].X, world.Coordinates[3].Y};
            
            Assert.Equal(expectedCoord1, actualCoord1);
            Assert.Equal(expectedCoord2, actualCoord2);
            Assert.Equal(expectedCoord3, actualCoord3);
            Assert.Equal(expectedCoord4, actualCoord4);
        }
    }
}