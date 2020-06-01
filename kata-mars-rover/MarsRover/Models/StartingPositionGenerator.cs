using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MarsRover.Tests;

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
            var availableCoords = world.Coordinates.Except(world.ObstacleCoordinates).ToList();
            var randomIndex = _random.Next(0, availableCoords.Count);
            var roverStartingCoords = availableCoords[randomIndex];
            return roverStartingCoords;
        }
        

        public Direction GenerateStartingDirection()
        {
            var random = new Random();
            var numberOfDirections = Direction.GetNames(typeof(Direction)).Length;
            var randomDirection = (Direction) random.Next(numberOfDirections);
            return randomDirection;
        }
    }
}