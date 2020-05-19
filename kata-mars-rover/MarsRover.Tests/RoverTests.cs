using System.Xml.Schema;
using Moq;
using Xunit;

namespace MarsRover.Tests
{
    public class RoverTests
    {
        [Fact]
        public void ShouldHaveRandomInitialStartingPointWithinBounds()
        {
            var mockRandomStartingPointGenerator = new Mock<IRandomStartingPointGenerator>();
            mockRandomStartingPointGenerator.Setup(generator =>  generator.Generate(3,3)).Returns(new []{2,2});
            var rover = new Rover(mockRandomStartingPointGenerator.Object);

            var expectedStartingPoint = new[] {2, 2};
            var actualStartingPoint = rover.CurrentCoords;
            
            Assert.Equal(expectedStartingPoint, actualStartingPoint);
        }
    }
}