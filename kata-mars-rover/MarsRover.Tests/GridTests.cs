using System;
using System.Collections.Generic;
using System.Security.Principal;
using MarsRover.Models;
using Moq;
using Xunit;

namespace MarsRover.Tests
{
    public class GridTests
    {
        [Fact]
        public void ShouldGenerateGridOfSpecifiedLengthAndWidth()
        {
            var grid = new Grid(4,3);

            const int expectedLength = 4;
            const int expectedHeight = 3;
            var actualLength = grid.Squares.GetLength(0);
            var actualWidth = grid.Squares.GetLength(1);
            
            Assert.Equal(expectedLength, actualLength);
            Assert.Equal(expectedHeight, actualWidth);
        }

        [Fact]
        public void ShouldGetSquareContentsOfGivenCoords()
        {
            var grid = new Grid(4,3);

            var squareContents = grid.GetContentsAt(new Coordinates(2,2));
            
            Assert.Equal(squareContents, SquareContents.Empty);
        }

        [Fact]
        public void ShouldPlaceObstaclesOnGridAtGeneratedCoords()
        {
            var grid = new Grid(3,3);
            
            var mockObstacleCoordinateGenerator = new Mock<IObstacleCoordinateGenerator>();
            mockObstacleCoordinateGenerator.Setup(generator =>  generator.Generate(grid.Squares, Grid.NumberOfObstacles))
                .Returns(new List<Coordinates>(){new Coordinates(1,1), new Coordinates(2,2), new Coordinates(3,3)});
            
            grid.PlaceObstaclesAtGeneratedCoords(mockObstacleCoordinateGenerator.Object);
            const SquareContents expected = SquareContents.Obstacle;
            Assert.Equal(expected, grid.GetContentsAt( new Coordinates(1,1) ));
            Assert.Equal(expected, grid.GetContentsAt( new Coordinates(2,2) ));
            Assert.Equal(expected, grid.GetContentsAt( new Coordinates(3,3) ));
        }
        
        // [Fact]
        // public void ShouldPlaceRoverAtRandomPositionWithoutObstacle()
        // {
        //     var grid = new Grid(2,2);
        //     
        //     var mockObstacleCoordinateGenerator = new Mock<IObstacleCoordinateGenerator>();
        //     mockObstacleCoordinateGenerator.Setup(generator =>  generator.Generate(grid.Squares, 3))
        //         .Returns(new List<Coordinates>(){new Coordinates(1,1), new Coordinates(1,2), new Coordinates(2,1)});
        //     
        //     grid.PlaceRoverAtAvailableGeneratedCoordinate(new StartingPositionGenerator());
        //     const SquareContents expected = SquareContents.Rover;
        //     
        //     Assert.Equal(expected, grid.GetContentsAt( new Coordinates(3,3) ));
        // }
    }
}