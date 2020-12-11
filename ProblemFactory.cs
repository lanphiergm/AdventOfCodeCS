using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventOfCode
{
    /// <summary>
    /// Creates and serves up all of the solved problems
    /// </summary>
    class ProblemFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerFactory">The logger factory to use for creating loggers for each problem</param>
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

        /// <summary>
        /// Gets a specific problem
        /// </summary>
        /// <param name="year">The year of the problem</param>
        /// <param name="day">The day of the problem</param>
        /// <returns>The initialized problem instance</returns>
        public IProblem GetProblem(int year, int day)
        {
            _ = problems.TryGetValue((year, day), out IProblem problem);
            return problem;
        }

        /// <summary>
        /// Gets the list of years that have problem solutions
        /// </summary>
        /// <returns>The list of year numbers</returns>
        public IEnumerable<int> GetYears()
        {
            return problems.Select(p => p.Key.year).Distinct().OrderBy(y => y);
        }

        /// <summary>
        /// Gets the list of days that have problem solutions for the given year
        /// </summary>
        /// <param name="year">The year in which to look for days</param>
        /// <returns>The list of day numbers</returns>
        public IEnumerable<int> GetDays(int year)
        {
            return problems.Where(p => p.Key.year == year).Select(p => p.Key.day).OrderBy(d => d);
        }

        private readonly Dictionary<(int year, int day), IProblem> problems =
            new Dictionary<(int year, int day), IProblem>();
    }
}
