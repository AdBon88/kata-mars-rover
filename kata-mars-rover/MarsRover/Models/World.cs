using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Models;
using Random = System.Random;

namespace MarsRover.Tests
{
    public class World
    {
        private const int MinBounds = 1;
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

        public Coordinates GetNextCoordinateTowardNorth(Coordinates currentCoord)
        {
            var atTopOfGrid = currentCoord.Y == MinBounds;
            return atTopOfGrid ? 
                WrapToYMax(currentCoord) : 
                DecreaseYCoord(currentCoord);
        }
        
        private Coordinates DecreaseYCoord(Coordinates currentCoord)
        {
            return AllCoordinates.Find(coord => coord.X == currentCoord.X && coord.Y == currentCoord.Y - 1);
        }    

        private Coordinates WrapToYMax(Coordinates currentCoord)
        {
            return AllCoordinates.Find(coord => coord.X == currentCoord.X && coord.Y == Height);
        }

        public Coordinates GetNextCoordinateTowardEast(Coordinates currentCoord)
        {
            return currentCoord.X < Length ? AllCoordinates.Find(coord => coord.X == currentCoord.X + 1 && coord.Y == currentCoord.Y) : AllCoordinates.Find(coord => coord.X == 1 && coord.Y == currentCoord.Y);
        }
        
        public Coordinates GetNextCoordinateTowardSouth(Coordinates currentCoord)
        {
            return currentCoord.Y < Height ? AllCoordinates.Find(coord => coord.X == currentCoord.X && coord.Y == currentCoord.Y + 1) : AllCoordinates.Find(coord => coord.X == currentCoord.X && coord.Y == 1);
        }

        public Coordinates GetNextCoordinateTowardWest(Coordinates currentCoord)
        {
            return currentCoord.X > 1 ? AllCoordinates.Find(coord => coord.X == currentCoord.X - 1 && coord.Y == currentCoord.Y) : AllCoordinates.Find(coord => coord.X == Length && coord.Y == currentCoord.Y);
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