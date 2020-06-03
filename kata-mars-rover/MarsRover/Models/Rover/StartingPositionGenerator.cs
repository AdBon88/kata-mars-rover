using System.Collections.Generic;
using System.Linq;
using System.Net;
using MarsRover.Models;
using Random = System.Random;

namespace MarsRover
{
    public class StartingPositionGenerator : IStartingPositionGenerator
    {
        private readonly IRandom _random;

        public StartingPositionGenerator(IRandom random)
        {
            _random = random;
        }
        public Coordinates GenerateStartingCoordsIn(World world)
        {
            var obstacleCoordinates = world.Obstacles.Select(obstacle => obstacle.Coordinates).ToList();
            var availableCoords = world.Coordinates.Except(obstacleCoordinates).ToList();
            var randomIndex = _random.Next(0, availableCoords.Count);
            var roverStartingCoords = availableCoords[randomIndex];
            return roverStartingCoords;
        }
        

        public Orientation GenerateStartingDirection()
        {
            var random = new Random();
            var numberOfDirections = Orientation.GetNames(typeof(Orientation)).Length;
            var randomDirection = (Orientation) random.Next(numberOfDirections);
            return randomDirection;
        }
    }
}