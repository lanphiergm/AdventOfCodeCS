using Microsoft.Extensions.Logging;
using System;

namespace AdventOfCode
{
    class ProblemRunner
    {
        public ProblemRunner(ILogger<ProblemRunner> logger, ProblemFactory problemFactory)
        {
            _logger = logger;
            _problemFactory = problemFactory;
        }

        private readonly ILogger<ProblemRunner> _logger;
        private readonly ProblemFactory _problemFactory;

        public bool Run(int year, int day, int part)
        {
            IProblem problem = _problemFactory.GetProblem(year, day);
            if (problem == null)
            {
                _logger.LogError("No problem found for year {0} day {1}", year, day);
                return false;
            }

            bool success = false;
            _logger.LogInformation("{0} Day {1}: {2}, Part {3}", year, day, problem.Name, part);
            var startTime = DateTime.Now;
            _logger.LogInformation("Started at {0}", startTime);
            
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

            var endTime = DateTime.Now;
            _logger.LogInformation("Finished at {0}. Duration: {1}", endTime, endTime - startTime);
            return success;
        }
    }
}
