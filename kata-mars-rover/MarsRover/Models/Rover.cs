using System;
using System.ComponentModel;
using System.Diagnostics;

namespace MarsRover.Tests
{
    public class Rover
    {
        //TODO these will get moved to the grid class
        private const int XMin = 1;
        private const int YMin = 1;
        private const int XMax = 3;
        private const int YMax = 3;

        public int[] CurrentCoords { get; set; }
        private const int xCoordIndex = 0;
        private const int yCoordIndex = 1;
        public Direction CurrentDirection { get; set; }

        public Rover(IStartingPositionGenerator startingPositionGenerator)
        {
            CurrentCoords = startingPositionGenerator.GenerateStartingCoords(XMax, YMax);
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
            if (CurrentCoords[yCoordIndex] > YMin)
                CurrentCoords[yCoordIndex]--;
            else
                CurrentCoords[yCoordIndex] = YMax;
        }
        private void MoveTowardEast()
        {
            if (CurrentCoords[xCoordIndex] < XMax)
                CurrentCoords[xCoordIndex]++;
            else
                CurrentCoords[xCoordIndex] = XMin;
        }
        private void MoveTowardSouth()
        {
            if (CurrentCoords[yCoordIndex] < YMax)
                CurrentCoords[yCoordIndex]++;
            else
                CurrentCoords[yCoordIndex] = YMin;
        }
        private void MoveTowardWest()
        {
            if (CurrentCoords[xCoordIndex] > XMin)
                CurrentCoords[xCoordIndex]--;
            else
                CurrentCoords[xCoordIndex] = YMax;
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