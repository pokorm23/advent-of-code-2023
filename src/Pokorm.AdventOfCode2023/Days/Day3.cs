﻿using System.Text;

namespace Pokorm.AdventOfCode2023;

public class Day3 : IDay
{
    private readonly IInputService inputService;

    public Day3(IInputService inputService) => this.inputService = inputService;

    public int Day => 3;

    public async Task<int> SolveAsync()
    {
        var lines = await this.inputService.GetInputLinesAsync(this.Day);

        var top = 0;
        var parser = new Parser();
        var spans = new List<EngineSpan>();

        foreach (var line in lines)
        {
            spans.AddRange(parser.ParseLine(line, top).ToList());

            top++;
        }

        var engine = new EngineSchematic(top + 1, spans.Select(x => x.GetMaxLeftCoord()).Max() + 1, spans);

        var partNumbers = engine.GetPartNumbers();

        return partNumbers.Aggregate(0, (acc, pn) => acc + pn.Number);
    }

    public async Task<int> SolveBonusAsync() => throw new NotImplementedException();

    private class Parser
    {
        private StringBuilder? numberParse;

        public IEnumerable<EngineSpan> ParseLine(string line, int top)
        {
            var left = 0;

            foreach (var c in line)
            {
                if (char.IsAsciiDigit(c))
                {
                    this.numberParse ??= new StringBuilder();
                    this.numberParse.Append(c);
                }
                else if (this.numberParse is not null && this.numberParse.Length > 0)
                {
                    var numStr = this.numberParse.ToString();

                    yield return new NumberEngineSpan(new Coord(left - this.numberParse.Length, top), numStr, int.Parse(numStr));

                    this.numberParse.Clear();
                }

                if (c != '.' && !char.IsAsciiDigit(c))
                {
                    yield return new SymbolEngineSpan(new Coord(left, top), c);
                }

                left++;
            }

            if (this.numberParse is null || this.numberParse.Length == 0)
            {
                yield break;
            }

            var numStr2 = this.numberParse.ToString();

            yield return new NumberEngineSpan(new Coord(left - this.numberParse.Length, top), numStr2, int.Parse(numStr2));

            this.numberParse.Clear();
        }
    }

    private record EngineSchematic(int Height, int Width, IReadOnlyCollection<EngineSpan> Spans)
    {
        public IEnumerable<NumberEngineSpan> GetPartNumbers()
        {
            var symbols = this.Spans.OfType<SymbolEngineSpan>().ToList();

            foreach (var n in this.Spans.OfType<NumberEngineSpan>())
            {
                var frame = n.GetSurrounding();

                if (symbols.Any(s => frame.IsIn(s.Coord)))
                {
                    yield return n;
                }
            }
        }
    }

    private record Frame(Coord TopLeft, Coord BottomRight)
    {
        public bool IsIn(Coord c) => c.Left >= this.TopLeft.Left
                                     && c.Left <= this.BottomRight.Left
                                     && c.Top >= this.TopLeft.Top
                                     && c.Top <= this.BottomRight.Top;
    }

    private record EngineSpan(Coord Coord, int Width)
    {
        public int GetMaxLeftCoord() => this.Coord.Left + this.Width - 1;
    }

    private record SymbolEngineSpan(Coord Coord, char Symbol) : EngineSpan(Coord, 1);

    private record NumberEngineSpan(Coord Coord, string Original, int Number) : EngineSpan(Coord, Original.Length)
    {
        public Frame GetSurrounding()
        {
            var topLeft = new Coord(this.Coord.Left - 1, this.Coord.Top - 1);
            var bottomRight = new Coord(this.Coord.Left + this.Width, this.Coord.Top + 1);

            return new Frame(topLeft, bottomRight);
        }
    }

    private record Coord(int Left, int Top);
}
