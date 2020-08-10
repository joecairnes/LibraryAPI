using System;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var a = 10;
            var b = 20;
            var answer = a + b;
            Assert.Equal(30, answer);
        }

        [Theory]
        [InlineData(10, 2, 12)]
        [InlineData(2, 2, 4)]
        [InlineData(10, 5, 15)]
        public void CanDoAddition(int a, int b, int expected)
        {
            int answer = a + b;
            Assert.Equal(expected, answer);
        }
    }
}
