using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Problems.Year2015
{
    class Day14ReindeerOlympics : ProblemBase<int>
    {
        public Day14ReindeerOlympics(ILogger logger) : base(logger, "Reindeer Olympics", 2015, 14) { }

        protected override int ExecutePart1()
        {
            int maxDist = 0;
            foreach (Reindeer r in ParseReindeer())
            {
                int dist = r.GetDistance(duration);
                if (dist > maxDist)
                {
                    maxDist = dist;
                }
            }
            return maxDist;
        }

        protected override int ExecutePart2()
        {
            var reindeer = ParseReindeer();
            for (int i = 1; i <= duration; i++)
            {
                var distances = new List<(int distance, Reindeer deer)>();
                foreach(Reindeer r in reindeer)
                {
                    distances.Add((r.GetDistance(i), r));
                }
                int maxDist = distances.Max(i => i.distance);
                foreach (var (distance, deer) in distances.Where(i => i.distance == maxDist))
                {
                    deer.Score++;
                }
            }

            return reindeer.Max(r => r.Score);
        }

        private static List<Reindeer> ParseReindeer()
        {
            var reindeer = new List<Reindeer>();
            foreach (string capability in capabilities)
            {
                var match = capabilityRegex.Match(capability);
                reindeer.Add(new Reindeer(match.Groups[1].Value, int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
            }
            return reindeer;
        }

        private class Reindeer
        {
            private readonly int speed;
            private readonly int cycleTime;
            private readonly int flyTime;

            public Reindeer(string name, int speed, int flyTime, int restTime)
            {
                Name = name;
                this.speed = speed;
                this.flyTime = flyTime;
                cycleTime = flyTime + restTime;
            }

            public string Name { get; }
            public int Score { get; set; }

            public int GetDistance(int seconds)
            {
                int wholeCycles = seconds / cycleTime;
                int remainingSeconds = seconds % cycleTime;
                return wholeCycles * flyTime * speed + Math.Min(remainingSeconds, flyTime) * speed;
            }
        }

        private static readonly Regex capabilityRegex = new Regex(
            "(.*) can fly (\\d*) km/s for (\\d*) seconds, but then must rest for (\\d*) seconds.");

        #region Data

        private const int duration = 2503;

        private static readonly string[] capabilities =
        {
            "Vixen can fly 8 km/s for 8 seconds, but then must rest for 53 seconds.",
            "Blitzen can fly 13 km/s for 4 seconds, but then must rest for 49 seconds.",
            "Rudolph can fly 20 km/s for 7 seconds, but then must rest for 132 seconds.",
            "Cupid can fly 12 km/s for 4 seconds, but then must rest for 43 seconds.",
            "Donner can fly 9 km/s for 5 seconds, but then must rest for 38 seconds.",
            "Dasher can fly 10 km/s for 4 seconds, but then must rest for 37 seconds.",
            "Comet can fly 3 km/s for 37 seconds, but then must rest for 76 seconds.",
            "Prancer can fly 9 km/s for 12 seconds, but then must rest for 97 seconds.",
            "Dancer can fly 37 km/s for 1 seconds, but then must rest for 36 seconds."
        };

        #endregion Data
    }
}
