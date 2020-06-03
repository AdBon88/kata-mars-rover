using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Transactions;
using MarsRover.Models;
using Random = MarsRover.Models.Random;

namespace MarsRover
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the world length: ");
            if(!int.TryParse(Console.ReadLine(), out var length))
                Console.WriteLine("Please enter an integer > 0");
            
            Console.WriteLine("Enter the world height: ");
            if(!int.TryParse(Console.ReadLine(), out var height))
                Console.WriteLine("Please enter an integer > 0");
            
            var maxObstacles = length * height - 1;
            Console.WriteLine($"Enter the number of obstacles to generate (max for this world size: {maxObstacles}): ");

            if(!int.TryParse(Console.ReadLine(), out var numberOfObstacles) || numberOfObstacles > maxObstacles)
                Console.WriteLine($"Please enter an integer > 0 and < {maxObstacles + 1}");
            
            Console.WriteLine("Generating world...");
            var world = new World(length, height);
            world.GenerateObstacleCoordinates(numberOfObstacles, new ObstacleGenerator(new Random()));
            var rover = new Rover(new StartingPositionGenerator(new Random()), world);
            
            Console.WriteLine($"At position {rover.CurrentCoords.X},{rover.CurrentCoords.Y} facing {rover.CurrentOrientation.ToString()}");
            string commands;
            do
            {
                Console.WriteLine("Enter next command(s), or q to quit");
                commands = Console.ReadLine();
                Console.WriteLine(rover.TranslateCommands(commands));
            } while (!commands.ToLower().Contains("q"));
        }
    }
}
