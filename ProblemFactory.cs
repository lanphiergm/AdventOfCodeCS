using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventOfCode
{
    class ProblemFactory
    {
        public ProblemFactory(ILoggerFactory loggerFactory)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(
                t => typeof(IProblem).IsAssignableFrom(t) && !t.IsAbstract);
            foreach (var type in types)
            {
                var problem = (IProblem)Activator.CreateInstance(type, loggerFactory.CreateLogger(type));
                if (problems.ContainsKey((problem.Year, problem.Day)))
                {
                    throw new InvalidOperationException($"Duplicate problem for year {problem.Year} day {problem.Day}");
                }
                problems[(problem.Year, problem.Day)] = problem;
            }
        }

        public IProblem GetProblem(int year, int day)
        {
            _ = problems.TryGetValue((year, day), out IProblem problem);
            return problem;
        }

        private readonly Dictionary<(int year, int day), IProblem> problems =
            new Dictionary<(int year, int day), IProblem>();
    }
}
