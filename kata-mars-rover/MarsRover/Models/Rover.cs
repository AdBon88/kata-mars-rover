using System;
using System.ComponentModel;
using System.Diagnostics;

namespace MarsRover.Tests
{
    public class Rover
    {
        public World World { get; }
        public Coordinates CurrentCoords { get;}
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
            switch (CurrentDirection)
            {
                case Direction.North:
                    MoveTowardNorth();
                    break;
                case Direction.East:
                    MoveTowardEast();
                    break;
                case Direction.South:
                    MoveTowardSouth();
                    break;
                case Direction.West:
                    MoveTowardWest();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void MoveBackward()
        {
            switch (CurrentDirection)
            {
                case Direction.North:
                    MoveTowardSouth();
                    break;
                case Direction.East:
                    MoveTowardWest();
                    break;
                case Direction.South:
                    MoveTowardNorth();
                    break;
                case Direction.West:
                    MoveTowardEast();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void MoveTowardNorth()
        {
            
            if (CurrentCoords.Y > 1)
                CurrentCoords.Y--;
            else
                CurrentCoords.Y = World.Height;
        }
        private void MoveTowardEast()
        {
            if (CurrentCoords.X < World.Length)
                CurrentCoords.X++;
            else
                CurrentCoords.X = 1;
        }
        private void MoveTowardSouth()
        {
            if (CurrentCoords.Y < World.Height)
                CurrentCoords.Y++;
            else
                CurrentCoords.Y = 1;
        }
        private void MoveTowardWest()
        {
            if (CurrentCoords.X > 1)
                CurrentCoords.X--;
            else
                CurrentCoords.X = World.Height;
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