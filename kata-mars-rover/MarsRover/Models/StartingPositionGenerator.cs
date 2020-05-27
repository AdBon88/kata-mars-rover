using System;
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
           
            var x = _random.Next(1, world.Length + 1);
            var y = _random.Next(1, world.Height + 1);
            
            return new Coordinates(x, y);
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