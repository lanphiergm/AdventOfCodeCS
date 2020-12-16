using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AdventOfCode.Problems.Year2020
{
    class Day15RambunctiousRecitation : ProblemBase<int>
    {
        public Day15RambunctiousRecitation(ILogger logger) : base(logger, "Rambunctious Recitation", 2020, 15) { }

        private static readonly int[] startingNumbers = { 5, 2, 8, 16, 18, 0, 1 };

        protected override int ExecutePart1()
        {
            return FindNumberAtPosition(2020);
        }

        protected override int ExecutePart2()
        {
            return FindNumberAtPosition(30000000);
        }

        private static int FindNumberAtPosition(int position)
        {
            var indices = new Dictionary<int, int>();
            for (int i = 0; i < startingNumbers.Length - 1; i++)
            {
                indices[startingNumbers[i]] = i;
            }
            int last = startingNumbers[^1];
            for (int i = startingNumbers.Length - 1; i < position - 1; i++)
            {
                if (indices.TryGetValue(last, out int lastIndex))
                {
                    indices[last] = i;
                    last = i - lastIndex;
                }
                else
                {
                    indices[last] = i;
                    last = 0;
                }
            }
            return last;
        }

    }
}
