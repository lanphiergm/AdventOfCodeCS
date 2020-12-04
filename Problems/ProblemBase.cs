using Microsoft.Extensions.Logging;

namespace AdventOfCode.Problems
{
    abstract class ProblemBase<T> : IProblem
    {
        protected ProblemBase(ILogger logger, string name, int year, int day)
        {
            Logger = logger;
            Name = name;
            Day = day;
            Year = year;
        }

        public string Name { get; }
        public int Day { get; }
        public int Year { get; }
        protected ILogger Logger { get; }

        protected abstract T ExecutePart1();

        protected abstract T ExecutePart2();

        public string Execute(int part)
        {
            string result = null;
            if (part == 1)
            {
                result = ExecutePart1().ToString();
            }
            else if (part == 2)
            {
                result = ExecutePart2().ToString();
            }
            return result;
        }
    }
}
