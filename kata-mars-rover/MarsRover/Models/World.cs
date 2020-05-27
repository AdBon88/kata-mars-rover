using System;
using System.Collections.Generic;
using MarsRover.Models;

namespace MarsRover.Tests
{
    public class World
    {

        public int Length { get; }
        public int Height { get; }

        public List<Coordinates> ObstacleCoordinates;
        
        public World(int length, int height)
        {
            Length = length;
            Height = height;
        }

        public void GeneratorObstacleCoordinates(int numberOfObstacles, IObstacleCoordinateGenerator obstacleCoordGenerator)
        {
            ObstacleCoordinates = obstacleCoordGenerator.Generate(Length, Height, numberOfObstacles);
        }
        
    }
}