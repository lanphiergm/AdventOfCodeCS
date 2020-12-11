using Microsoft.Extensions.Logging;
using System.Text;

namespace AdventOfCode.Problems.Year2015
{
    class Day10ElvesLookElvesSay : ProblemBase<int>
    {
        public Day10ElvesLookElvesSay(ILogger logger) : base(logger, "Elves Look, Elves Say", 2015, 10) { }

        private const string INITIALDATA = "1113222113";

        protected override int ExecutePart1()
        {
            return LookAndSay(INITIALDATA, 40).Length;
        }

        protected override int ExecutePart2()
        {
            return LookAndSay(INITIALDATA, 50).Length;
        }

        private static string LookAndSay(string initial, int iterations)
        {
            if (iterations == 0)
            {
                return initial;
            }
            var builder = new StringBuilder();
            for (int i = 0; i < initial.Length; i++)
            {
                char curr = initial[i];
                int count = 1;
                for (int j = i + 1; j < initial.Length; j++)
                {
                    if (initial[j] == curr)
                    {
                        i++;
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                builder.Append(count);
                builder.Append(curr);
            }
            return LookAndSay(builder.ToString(), iterations - 1);
        }
    }
}
