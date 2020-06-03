using System;
using System.Collections.Generic;

namespace MarsRover.Models
{
    public interface IObstacleGenerator
    {
        public List<Obstacle> Generate(List<Coordinates> worldCoordinates, int numberOfObstacles);
    }
}