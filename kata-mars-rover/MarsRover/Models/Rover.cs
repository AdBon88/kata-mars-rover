using System;
using System.ComponentModel;
using System.Diagnostics;

namespace MarsRover.Tests
{
    public class Rover
    {
        public World World { get; }
        public Coordinates CurrentCoords { get; set; }
        public Direction CurrentDirection { get; set; }
        

        public Rover(IStartingPositionGenerator startingPositionGenerator, World world)
        {
            World = world;
            CurrentCoords = startingPositionGenerator.GenerateStartingCoordsIn(world);
            CurrentDirection = startingPositionGenerator.GenerateStartingDirection();
        }

        public void TranslateCommands(string commands)
        {
            foreach (var command in commands.ToLower())
            {
                switch (command)
                {
                    case 'f':
                        MoveForward();
                        break;
                    case 'b':
                        MoveBackward();
                        break;
                    case 'l':
                        TurnLeft();
                        break;
                    case 'r':
                        TurnRight();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void MoveForward()
        {
            CurrentCoords = CurrentDirection switch
            {
                Direction.North => World.GetNextCoordinateTowardNorth(CurrentCoords),
                Direction.East => World.GetNextCoordinateTowardEast(CurrentCoords),
                Direction.South => World.GetNextCoordinateTowardSouth(CurrentCoords),
                Direction.West => World.GetNextCoordinateTowardWest(CurrentCoords),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private void MoveBackward()
        {
            CurrentCoords = CurrentDirection switch
            {
                Direction.North => World.GetNextCoordinateTowardSouth(CurrentCoords),
                Direction.East => World.GetNextCoordinateTowardWest(CurrentCoords),
                Direction.South => World.GetNextCoordinateTowardNorth(CurrentCoords),
                Direction.West => World.GetNextCoordinateTowardEast(CurrentCoords),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private void TurnLeft()
        {
            CurrentDirection = CurrentDirection switch
            {
                Direction.North => Direction.West,
                Direction.East => Direction.North,
                Direction.South => Direction.East,
                Direction.West => Direction.South,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        private void TurnRight()
        {
            CurrentDirection = CurrentDirection switch
            {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}