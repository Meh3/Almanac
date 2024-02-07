using Almanac;
using Xunit;

namespace AlmanasTest;

public class AlmanacTest
{
    [Fact]
    public void TestInputFile()
    {
        // ARRANGE
        string path = Path.Combine(Directory.GetCurrentDirectory(), "zadanie_input.txt");
        var expectedValue = 261668924;

        // ACT
        using var sr = new StreamReader(path);
        var actualValue = AlmanacResolver.GetMinimumLoaction(sr);

        // ASSERT
        Assert.Equal(expectedValue, actualValue);
    }
}
