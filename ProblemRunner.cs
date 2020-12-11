using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Runs a set of problems
    /// </summary>
    class ProblemRunner
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">The logger to which progress and answers get logged</param>
        /// <param name="problemFactory">The factory that provides the problems</param>
        public ProblemRunner(ILogger<ProblemRunner> logger, ProblemFactory problemFactory)
        {
            _logger = logger;
            _problemFactory = problemFactory;
        }

        private readonly ILogger<ProblemRunner> _logger;
        private readonly ProblemFactory _problemFactory;
        private readonly List<(string problem, TimeSpan duration)> _executionTimes =
            new List<(string problem, TimeSpan duration)>();

        /// <summary>
        /// Runs the specified problems
        /// </summary>
        /// <param name="year">The year to run; null runs all</param>
        /// <param name="day">The day to run; null runs all</param>
        /// <param name="part">The part to run; null runs all</param>
        /// <returns></returns>
        public bool Run(int? year, int? day, int? part)
        {
            bool success = year.HasValue ? ExecuteYear(year.Value, day, part) :
                                           IterateYears(day, part);
            PrintStatistics();
            return success;
        }

        private bool IterateYears(int? day, int? part)
        {
            bool success = true;
            foreach (int year in _problemFactory.GetYears())
            {
                success &= ExecuteYear(year, day, part);
            }
            return success;
        }

        private bool ExecuteYear(int year, int? day, int? part)
        {
            return day.HasValue ? ExecuteDay(year, day.Value, part) :
                                  IterateDays(year, part);
        }

        private bool IterateDays(int year, int? part)
        {
            bool success = true;
            foreach (int day in _problemFactory.GetDays(year))
            {
                success &= ExecuteDay(year, day, part);
            }
            return success;
        }

        private bool ExecuteDay(int year, int day, int? part)
        {
            return part.HasValue ? ExecutePart(year, day, part.Value) :
                                   IterateParts(year, day);
        }

        private bool IterateParts(int year, int day)
        {
            return ExecutePart(year, day, 1) && ExecutePart(year, day, 2);
        }

        private bool ExecutePart(int year, int day, int part)
        {
            IProblem problem = _problemFactory.GetProblem(year, day);
            if (problem == null)
            {
                _logger.LogError("No problem found for year {0} day {1}", year, day);
                return false;
            }

            bool success = false;
            string problemId = string.Format("{0} Day {1}: {2}, Part {3}", year, day, problem.Name, part);
            _logger.LogInformation(problemId);
            var startTime = DateTime.Now;
            
            string answer = problem.Execute(part);
            if (string.IsNullOrEmpty(answer) || answer == "0") //it's safe to assume an answer of zero is invalid
            {
                _logger.LogWarning("NO ANSWER");
            }
            else
            {
                _logger.LogInformation("Answer: {0}", answer);
                success = true;
            }

            var duration = DateTime.Now - startTime;
            _executionTimes.Add((problemId, duration));
            _logger.LogInformation("Duration: {1}", duration);
            return success;
        }

        private void PrintStatistics()
        {
            // Only print if there is more than one problem that ran
            if (_executionTimes.Count > 1)
            {
                _logger.LogInformation("--------------------------------------------------------");
                var (longestProblem, longestDuration) = _executionTimes.First(a => a.duration == _executionTimes.Max(b => b.duration));
                var (shortestProblem, shortestDuration) = _executionTimes.First(a => a.duration == _executionTimes.Min(b => b.duration));
                _logger.LogInformation("Longest duration: {0} ({1})", longestDuration, longestProblem);
                _logger.LogInformation("Average duration: {0}", new TimeSpan((long)_executionTimes.Average(a => a.duration.Ticks)));
                _logger.LogInformation("Shortest duration: {0} ({1})", shortestDuration, shortestProblem);
                _logger.LogInformation("Total duration: {0}", new TimeSpan((long)_executionTimes.Sum(a => a.duration.Ticks)));
            }
        }
    }
}
