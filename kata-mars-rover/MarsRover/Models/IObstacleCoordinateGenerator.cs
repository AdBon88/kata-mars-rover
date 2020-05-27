using System;
using System.Collections.Generic;
using MarsRover.Tests;

namespace MarsRover.Models
{
    public interface IObstacleCoordinateGenerator
    {
        public List<Coordinates> Generate(int length, int height, int numberOfObstacles);
    }
}