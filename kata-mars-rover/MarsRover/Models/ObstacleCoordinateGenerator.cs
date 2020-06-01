using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.XPath;
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
        
        public List<Coordinates> Generate(List<Coordinates> worldCoordinates, int numberOfObstacles)
        {
            var maxNumberOfObstacles = worldCoordinates.Count- 1; //need to leave at least 1 empty spot for the rover. 
            if (numberOfObstacles < 0 || numberOfObstacles > maxNumberOfObstacles) throw new ArgumentOutOfRangeException();
            
            var availableCoords = worldCoordinates;
            var obstacleCoords = new List<Coordinates>();
            
            for (var i = 0; i < numberOfObstacles; i++)
            {
                var randomIndex = _random.Next(0, availableCoords.Count);
                obstacleCoords.Add(availableCoords[randomIndex]);
                availableCoords.Remove(availableCoords[randomIndex]);
            }

            return obstacleCoords;
        }

        private static List<Coordinates> GetAllPossibleCoords(int length, int height)
        {
            var availableCoords = new List<Coordinates>();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < length; x++)
                {
                    availableCoords.Add(new Coordinates(x, y));
                }
            }
            return availableCoords;
        }
    }
}