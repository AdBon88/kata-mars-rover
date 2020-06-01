using System;
using System.ComponentModel;
using System.Diagnostics;

namespace MarsRover.Tests
{
    public class Rover
    {
        private World World { get; }
        public Coordinates CurrentCoords { get; private set; }
        public Direction CurrentDirection { get; private set; }


        public Rover(IStartingPositionGenerator startingPositionGenerator, World world)
        {
            World = world;
            CurrentCoords = startingPositionGenerator.GenerateStartingCoordsIn(world);
            CurrentDirection = startingPositionGenerator.GenerateStartingDirection();
        }

        public string TranslateCommands(string commands)
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

            return "";
        }

        private string MoveForward()
        {
            switch (CurrentDirection)
            {
                case Direction.North:
                    CurrentCoords = World.GetNextCoordinateTowardNorth(CurrentCoords);
                    // if (World.ObstacleCoordinates.Exists(c => c.X == nextCoordinate.X && c.Y == nextCoordinate.Y))
                    // {
                    //     return $"Cannot move to coord ({nextCoordinate.X},{nextCoordinate.Y}) - there is an obstacle here!";
                    // }
                    // else
                    // {
                    //     CurrentCoords = nextCoordinate;
                    //     return "";
                    // }
                    return "";
                case Direction.East:
                    CurrentCoords = World.GetNextCoordinateTowardEast(CurrentCoords);
                    return "";
                case Direction.South:
                    CurrentCoords = World.GetNextCoordinateTowardSouth(CurrentCoords);
                    return "";
                case Direction.West:
                    CurrentCoords = World.GetNextCoordinateTowardWest(CurrentCoords);
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }
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