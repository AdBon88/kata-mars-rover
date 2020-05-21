using System;
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

        public void ProcessInput(string command)
        {
            switch (command.ToLower())
            {
                case "f":
                    processNextAction(Action.MoveForward);
                    break;
                case "b":
                    processNextAction(Action.MoveBackward);
                    break;
                case "l":
                    processNextAction(Action.TurnLeft);
                    break;
                case "r":
                    processNextAction(Action.TurnRight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void processNextAction(Action nextAction)
        {
            switch (nextAction)
            {
                case Action.MoveForward:
                    MoveForward();
                    break;
                case Action.MoveBackward:
                    MoveBackward();
                    break;
                case Action.TurnLeft:
                    TurnLeft();
                    break;
                case Action.TurnRight:
                    TurnRight();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
            switch (CurrentDirection)
            {
                case Direction.North:
                    CurrentDirection = Direction.West;
                    break;
                case Direction.East:
                    CurrentDirection = Direction.North;
                    break;
                case Direction.South:
                    CurrentDirection = Direction.East;
                    break;
                case Direction.West:
                    CurrentDirection = Direction.South;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void TurnRight()
        {
            switch (CurrentDirection)
            {
                case Direction.North:
                    CurrentDirection = Direction.East;
                    break;
                case Direction.East:
                    CurrentDirection = Direction.South;
                    break;
                case Direction.South:
                    CurrentDirection = Direction.West;
                    break;
                case Direction.West:
                    CurrentDirection = Direction.North;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}