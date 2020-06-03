using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Models;
using Random = System.Random;

namespace MarsRover
{
    public class World
    {
        private const int MinBounds = 1;
        private int Length { get; }
        private int Height { get; }

        public List<Coordinates> Coordinates { get; } = new List<Coordinates>();
        public List<Obstacle> Obstacles { get; private set; } = new List<Obstacle>();
        
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
        
        public void GenerateObstacleCoordinates(int numberOfObstacles, IObstacleGenerator obstacleCoordGenerator)
        {
            Obstacles = obstacleCoordGenerator.Generate(Coordinates, numberOfObstacles);
        }
        
        public Coordinates GetNextCoordinateTowardNorth(Coordinates currentCoord)
        {
            var isAtNorthEdge = currentCoord.Y == MinBounds;
            return isAtNorthEdge 
                ? WrapToSouthEdge(currentCoord) 
                : DecreaseYPosition(currentCoord);
        }
        
        private Coordinates DecreaseYPosition(Coordinates currentCoord)
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
                : IncreaseXPosition(currentCoord);
        }

        private Coordinates WrapToWestEdge(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == MinBounds && coord.Y == currentCoord.Y);
        }

        private Coordinates IncreaseXPosition(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == currentCoord.X + 1 && coord.Y == currentCoord.Y);
        }

        public Coordinates GetNextCoordinateTowardSouth(Coordinates currentCoord)
        {
            var isAtSouthEdge = currentCoord.Y == Height;
            return isAtSouthEdge
                ? WrapToNorthEdge(currentCoord)
                : IncreaseYPosition(currentCoord);
        }

        private Coordinates WrapToNorthEdge(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == currentCoord.X && coord.Y == MinBounds);
        }
        
        private Coordinates IncreaseYPosition(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == currentCoord.X && coord.Y == currentCoord.Y + 1);
        }

        public Coordinates GetNextCoordinateTowardWest(Coordinates currentCoord)
        {
            var isAtWestEdge = currentCoord.X == MinBounds;
            return isAtWestEdge 
                ? WrapToEastEdge(currentCoord)
                : DecreaseXPosition(currentCoord);
        }

        private Coordinates WrapToEastEdge(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == Length && coord.Y == currentCoord.Y);
        }

        private Coordinates DecreaseXPosition(Coordinates currentCoord)
        {
            return Coordinates.Find(coord => coord.X == currentCoord.X - 1 && coord.Y == currentCoord.Y);
        }
        
    }
}