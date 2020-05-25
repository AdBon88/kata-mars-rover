using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MarsRover.Tests;

namespace MarsRover.Models
{
    public class ObstacleCoordinateGenerator : IObstacleCoordinateGenerator
    {
        private readonly IRandom _random;

        public ObstacleCoordinateGenerator(IRandom random)
        {
            _random = random;
        }
        
        public List<Coordinates> Generate(Square[,] availableSquares, int numberOfObstacles)
        {
            if (numberOfObstacles < 0 || numberOfObstacles > availableSquares.Length - 1) throw new ArgumentOutOfRangeException();
            
            var obstacleCoords = new List<Coordinates>();
            var availableCoords = (from Square square in availableSquares select square.Coordinates).ToList();

            for (var i = 0; i < numberOfObstacles; i++)
            {
                var randomIndex = _random.Next(0, availableCoords.Count);
                obstacleCoords.Add(availableCoords[randomIndex]);
                availableCoords.Remove(availableCoords[randomIndex]);
            }

            return obstacleCoords;
        }
    }
}