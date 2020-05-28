using System;
using System.Collections.Generic;
using MarsRover.Models;

namespace MarsRover.Tests
{
    public class World
    {

        public int Length { get; }
        public int Height { get; }

        public List<Coordinates> AllCoordinates { get; } = new List<Coordinates>();
        public List<Coordinates> ObstacleCoordinates { get; private set; } = new List<Coordinates>();
        
        public World(int length, int height)
        {
            Length = length;
            Height = height;
            AllCoordinates = GenerateAllCoordinates();
        }

        public void GeneratorObstacleCoordinates(int numberOfObstacles, IObstacleCoordinateGenerator obstacleCoordGenerator)
        {
            ObstacleCoordinates = obstacleCoordGenerator.Generate(Length, Height, numberOfObstacles);
        }

        public List<Coordinates> GenerateAllCoordinates()
        {
            var worldCoordinates = new List<Coordinates>();
            for (var y = 1; y <= Height; y++)
            {
                for (var x = 1; x <= Length; x++)
                {
                    worldCoordinates.Add(new Coordinates(x, y));
                }
            }

            return worldCoordinates;
        }
    }
}