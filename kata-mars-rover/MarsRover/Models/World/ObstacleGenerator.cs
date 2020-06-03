using System;
using System.Collections.Generic;

namespace MarsRover.Models
{
    public class ObstacleGenerator : IObstacleGenerator
    {
        private readonly IRandom _random;

        public ObstacleGenerator(IRandom random)
        {
            _random = random;
        }
        
        public List<Obstacle> Generate(List<Coordinates> worldCoordinates, int numberOfObstacles)
        {
            var maxNumberOfObstacles = worldCoordinates.Count- 1; //need to leave at least 1 empty spot for the rover. 
            if (numberOfObstacles < 0 || numberOfObstacles > maxNumberOfObstacles) throw new ArgumentOutOfRangeException();
            
            var availableCoords = new List<Coordinates>(worldCoordinates);
            var obstacles = new List<Obstacle>();
            
            for (var i = 0; i < numberOfObstacles; i++)
            {
                var randomIndex = _random.Next(0, availableCoords.Count);
                obstacles.Add(new Obstacle(availableCoords[randomIndex]));
                availableCoords.Remove(availableCoords[randomIndex]);
            }

            return obstacles;
        }
    }
}