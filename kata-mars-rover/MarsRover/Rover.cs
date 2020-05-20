using System;

namespace MarsRover.Tests
{
    public class Rover
    {
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

        public void ProcessCommand(string command)
        {
            switch (CurrentDirection)
            {
                case Direction.North:
                    MoveForwardTowardNorth();
                    break;
                case Direction.East:
                    MoveForwardTowardEast();
                    break;
                case Direction.South:
                    MoveForwardTowardSouth();
                    break;
                case Direction.West:
                    MoveForwardTowardWest();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MoveForwardTowardWest()
        {
            if (CurrentCoords[xCoordIndex] > XMin)
                CurrentCoords[xCoordIndex]--;
            else
                CurrentCoords[xCoordIndex] = YMax;
        }

        private void MoveForwardTowardSouth()
        {
            if (CurrentCoords[yCoordIndex] < YMax)
                CurrentCoords[yCoordIndex]++;
            else
                CurrentCoords[yCoordIndex] = YMin;
        }

        private void MoveForwardTowardEast()
        {
            if (CurrentCoords[xCoordIndex] < XMax)
                CurrentCoords[xCoordIndex]++;
            else
                CurrentCoords[xCoordIndex] = XMin;
        }

        private void MoveForwardTowardNorth()
        {
            if (CurrentCoords[yCoordIndex] > YMin)
                CurrentCoords[yCoordIndex]--;
            else
                CurrentCoords[yCoordIndex] = YMax;
        }
    }
}