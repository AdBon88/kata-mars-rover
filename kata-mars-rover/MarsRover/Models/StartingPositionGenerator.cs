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
            Coordinates startingCoords;

            bool generatedCoordsContainObstacle;
            do
            {
                generatedCoordsContainObstacle = false;
                var x = _random.Next(1, world.Length + 1);
                var y = _random.Next(1, world.Height + 1);

                startingCoords = new Coordinates(x, y);
                foreach (var obstacleCoord in world.ObstacleCoordinates)
                {
                    if (startingCoords.isEqualTo(obstacleCoord))
                    {
                        generatedCoordsContainObstacle = true;
                    }
                }
            } while (generatedCoordsContainObstacle);

            return startingCoords;
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