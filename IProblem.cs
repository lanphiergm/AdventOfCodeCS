using Microsoft.Extensions.Logging;

namespace AdventOfCode
{
    interface IProblem
    {
        string Name { get; }

        int Day { get; }

        int Year { get; }

        string Execute(int part);
    }
}
