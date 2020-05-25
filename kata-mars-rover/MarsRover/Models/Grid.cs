using System;
using System.Collections.Generic;
using MarsRover.Models;

namespace MarsRover.Tests
{
    public class Grid
    {
        public const int NumberOfObstacles = 3;
        public int Length { get; }
        public int Height { get; }
        

        public readonly Square[,] Squares;
        
        public Grid(int length, int height)
        {
            Length = length;
            Height = height;
            
            Squares = new Square[Length,Height];
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Length; x++)
                {
                    var coordinates = new Coordinates(x + 1, y + 1);
                    Squares[x, y] = new Square(coordinates);
                }
            }
        }

        public SquareContents GetContentsAt(Coordinates coordinates)
        {
            return Squares[coordinates.X - 1, coordinates.Y - 1].Contents;
        }
        
        public void PlaceObstaclesAtGeneratedCoords(IObstacleCoordinateGenerator obstacleCoordGenerator)
        {
            var obstacleCoordsList = obstacleCoordGenerator.Generate(Squares, NumberOfObstacles);
            foreach (var coordinates in obstacleCoordsList)
            {
                Squares[coordinates.X - 1, coordinates.Y - 1].Contents = SquareContents.Obstacle;
            }
        }

        // public void PlaceRoverAtAvailableGeneratedCoordinate(IStartingPositionGenerator startingPositionGenerator)
        // {
        //     var roverStartingCoords = startingPositionGenerator.GenerateStartingCoords(Squares);
        //     Squares[roverStartingCoords.X, roverStartingCoords.Y].Contents = SquareContents.Rover;
        // }
    }
}