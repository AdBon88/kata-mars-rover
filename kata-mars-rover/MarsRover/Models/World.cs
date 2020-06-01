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

        public List<Coordinates> Coordinates { get; } = new List<Coordinates>();
        public List<Coordinates> ObstacleCoordinates { get; private set; } = new List<Coordinates>();
        
        public World(int length, int height)
        {
            Length = length;
            Height = height;
            Coordinates = GenerateCoordinates();
        }
        public List<Coordinates> GenerateCoordinates()
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

        public void GenerateObstacleCoordinates(int numberOfObstacles, IObstacleCoordinateGenerator obstacleCoordGenerator)
        {
            ObstacleCoordinates = obstacleCoordGenerator.Generate(Coordinates, numberOfObstacles);
        }
        
        public Coordinates GetNextCoordinateTowardNorth(Coordinates currentCoord)
        {
            var isAtNorthEdge = currentCoord.Y == MinBounds;
            return isAtNorthEdge 
                ? WrapToSouthEdge(currentCoord) 
                : DecreaseYCoord(currentCoord);
        }
        
        private Coordinates DecreaseYCoord(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == currentCoord.X && coord.Y == currentCoord.Y - 1);
        }    

        private Coordinates WrapToSouthEdge(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == currentCoord.X && coord.Y == Height);
        }

        public Coordinates GetNextCoordinateTowardEast(Coordinates currentCoord)
        {
            var isAtEastEdge = currentCoord.X == Length;
            return isAtEastEdge 
                ? WrapToWestEdge(currentCoord) 
                : IncreaseXCoord(currentCoord);
        }

        private Coordinates WrapToWestEdge(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == MinBounds && coord.Y == currentCoord.Y);
        }

        private Coordinates IncreaseXCoord(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == currentCoord.X + 1 && coord.Y == currentCoord.Y);
        }

        public Coordinates GetNextCoordinateTowardSouth(Coordinates currentCoord)
        {
            var isAtSouthEdge = currentCoord.Y == Height;
            return isAtSouthEdge
                ? WrapToNorthEdge(currentCoord)
                : IncreaseYCoord(currentCoord);
        }

        private Coordinates WrapToNorthEdge(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == currentCoord.X && coord.Y == MinBounds);
        }
        
        private Coordinates IncreaseYCoord(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == currentCoord.X && coord.Y == currentCoord.Y + 1);
        }

        public Coordinates GetNextCoordinateTowardWest(Coordinates currentCoord)
        {
            var isAtWestEdge = currentCoord.X == MinBounds;
            return isAtWestEdge 
                ? WrapToEastEdge(currentCoord)
                : DecreaseXCoord(currentCoord);
        }

        private Coordinates WrapToEastEdge(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == Length && coord.Y == currentCoord.Y);
        }

        private Coordinates DecreaseXCoord(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == currentCoord.X - 1 && coord.Y == currentCoord.Y);
        }
        
    }
}