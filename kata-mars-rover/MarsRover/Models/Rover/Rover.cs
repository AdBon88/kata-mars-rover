using System;
using MarsRover.Models;
using MarsRover.Models.Enums;

namespace MarsRover
{
    public class Rover
    {
        private const string obstacleDetectedString = "Obstacle detected! Cannot move to coord ";
        private World World { get; }
        public Coordinates CurrentCoords { get; private set; }
        public Orientation CurrentOrientation { get; private set; }


        public Rover(IStartingPositionGenerator startingPositionGenerator, World world)
        {
            World = world;
            CurrentCoords = startingPositionGenerator.GenerateStartingCoordsIn(world);
            CurrentOrientation = startingPositionGenerator.GenerateStartingDirection();
        }

        public string TranslateCommands(string commands)
        {
            var output = "";
            foreach (var command in commands.ToLower())
            {
                switch (command)
                {
                    case 'q':
                        return "Bye!";
                    case 'f':
                        var nextCoords = GetNextForwardCoordinate();
                        output += $"{MoveResult(nextCoords, MoveDirection.Forward)}\n";
                        break;
                    case 'b':
                         nextCoords = GetNextBackwardCoordinate();
;                        output += $"{MoveResult(nextCoords, MoveDirection.Backward)}\n";
                        break;
                    case 'l':
                        TurnLeft();
                        output += $"{TurnResult(TurnDirection.Left)}\n";
                        break;
                    case 'r':
                        TurnRight();
                        output += $"{TurnResult(TurnDirection.Right)}\n";
                        break;
                    default:
                        return "Unknown command. Commands are: f (forward), b (backward), l (turn left), r (turn right), q (quit)";
                }
                if (output.Contains(obstacleDetectedString))
                    return output;
            }
            return output;
        }

        private Coordinates GetNextForwardCoordinate()
        {
            var nextCoordinates = CurrentOrientation switch
            {
                Orientation.North => World.GetNextCoordinateTowardNorth(CurrentCoords),
                Orientation.East => World.GetNextCoordinateTowardEast(CurrentCoords),
                Orientation.South => World.GetNextCoordinateTowardSouth(CurrentCoords),
                Orientation.West => World.GetNextCoordinateTowardWest(CurrentCoords),
                _ => throw new ArgumentOutOfRangeException()
            };
            return nextCoordinates;
        }
        private Coordinates GetNextBackwardCoordinate()
        {
            var nextCoordinates = CurrentOrientation switch
            {
                Orientation.North => World.GetNextCoordinateTowardSouth(CurrentCoords),
                Orientation.East => World.GetNextCoordinateTowardWest(CurrentCoords),
                Orientation.South => World.GetNextCoordinateTowardNorth(CurrentCoords),
                Orientation.West => World.GetNextCoordinateTowardEast(CurrentCoords),
                _ => throw new ArgumentOutOfRangeException()
            };
            return nextCoordinates;
        }
        private void TurnLeft()
        {
            CurrentOrientation = CurrentOrientation switch
            {
                Orientation.North => Orientation.West,
                Orientation.East => Orientation.North,
                Orientation.South => Orientation.East,
                Orientation.West => Orientation.South,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        private void TurnRight()
        {
            CurrentOrientation = CurrentOrientation switch
            {
                Orientation.North => Orientation.East,
                Orientation.East => Orientation.South,
                Orientation.South => Orientation.West,
                Orientation.West => Orientation.North,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private string MoveResult(Coordinates nextCoordinate, MoveDirection moveDirection)
        {
            if (World.Obstacles.Exists(o => o.Coordinates.X == nextCoordinate.X && o.Coordinates.Y == nextCoordinate.Y))
            {
                return $"{obstacleDetectedString}{nextCoordinate.X},{nextCoordinate.Y}";
            }

            CurrentCoords = nextCoordinate;
            return $"Moved {moveDirection.ToString().ToLower()} to {nextCoordinate.X},{nextCoordinate.Y}";
        }

        private string TurnResult(TurnDirection turnDirection)
        {
            return $"Turned {turnDirection.ToString().ToLower()} towards {CurrentOrientation.ToString()}";
        }
    }
}