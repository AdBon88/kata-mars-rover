using System.Xml.Schema;
using Moq;
using Xunit;

namespace MarsRover.Tests
{
    public class RoverTests
    {
        [Fact]
        public void ShouldHaveRandomInitialStartingPositionWithinBounds()
        {
            var mockRandomStartingPositionGenerator = new Mock<IRandomStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingCoords(3,3)).Returns(new []{2,2});
            var rover = new Rover(mockRandomStartingPositionGenerator.Object);

            var expectedStartingPoint = new[] {2, 2};
            var actualStartingPoint = rover.CurrentCoords;
            
            Assert.Equal(expectedStartingPoint, actualStartingPoint);
        }     
        
        [Fact]
        public void ShouldHaveRandomInitialStartingDirection()
        {
            var mockRandomStartingPositionGenerator = new Mock<IRandomStartingPositionGenerator>();
            mockRandomStartingPositionGenerator.Setup(generator =>  generator.GenerateStartingDirection()).Returns(Direction.West);
            var rover = new Rover(mockRandomStartingPositionGenerator.Object);

            const Direction expectedStartingDirection = Direction.West;
            var actualStartingDirection = rover.CurrentDirection;
            
            Assert.Equal(expectedStartingDirection, actualStartingDirection);
        }
    }
}