using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day17Tests(ILogger<Day17> logger) : DayTestBase
{
    [Fact]
    public void PartOne_1()
    {
        var day = new Day17(logger);

        var result = day.Solve(LinesFromSample(
            """
            Register A: 729
            Register B: 0
            Register C: 0

            Program: 0,1,5,4,3,0
            """));

        Assert.Equal("4,6,3,5,6,3,5,2,1,0", result);
    }

    [Fact]
    public void PartOne_F()
    {
        var day = new Day17(logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal("1,6,7,4,3,0,5,0,6", result);
    }

    [Fact]
    public void PartTwo_1()
    {
        var day = new Day17(logger);

        var result = day.SolveBonus(LinesFromSample(
            """
            Register A: 2024
            Register B: 0
            Register C: 0

            Program: 0,3,5,4,3,0
            """));

        Assert.Equal(117440, result);
    }

    [Fact]
    public void PartTwo_F_1()
    {
        var day = new Day17(logger);

        var parse = Day17.Parse(LinesFromSample(
            """
            Register A: 63687530
            Register B: 0
            Register C: 0

            Program: 2,4,1,3,7,5,0,3,1,5,4,1,5,5,3,0
            """));

        logger.LogInformation(string.Join(Environment.NewLine, parse.Program.GetFormatLines()));

        var computer = new Day17.Computer(Day17.ComputerState.Empty, Day17.Computer.AllInstructions);
    }

    [Fact]
    public void PartTwo_F()
    {
        var day = new Day17(logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }
}
