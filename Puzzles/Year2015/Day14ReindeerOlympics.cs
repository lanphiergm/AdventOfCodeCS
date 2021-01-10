// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2015
{
    /// <summary>
    /// Day 14: Reindeer Olympics
    /// https://adventofcode.com/2015/day/14
    /// </summary>
    [TestClass]
    public class Day14ReindeerOlympics
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(1120, ExecutePart1(sampleDuration, sampleInput));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(2655, ExecutePart1(puzzleDuration, puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(689, ExecutePart2(sampleDuration, sampleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(1059, ExecutePart2(puzzleDuration, puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="duration">The duration of the race</param>
        /// <param name="capabilities">The list of reindeer and their capabilities</param>
        /// <returns>The distance traveled by the winning reindeer</returns>
        private static int ExecutePart1(int duration, string[] capabilities)
        {
            int maxDist = 0;
            // Compute the distance each reindeer can travel by the end and take the maximum
            foreach (Reindeer r in ParseReindeer(capabilities))
            {
                maxDist = Math.Max(maxDist, r.GetDistance(duration));
            }
            return maxDist;
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="duration">The duration of the race</param>
        /// <param name="capabilities">The list of reindeer and their capabilities</param>
        /// <returns>The score of the winning reindeer</returns>
        private static int ExecutePart2(int duration, string[] capabilities)
        {
            var reindeer = ParseReindeer(capabilities);
            for (int i = 1; i <= duration; i++)
            {
                // At each second of the race, determine how far each reindeer has gone
                var distances = new List<(int distance, Reindeer deer)>();
                foreach(Reindeer r in reindeer)
                {
                    distances.Add((r.GetDistance(i), r));
                }

                // Give each reindeer that has traveled the maximum distance a point
                int maxDist = distances.Max(i => i.distance);
                foreach (var (distance, deer) in distances.Where(i => i.distance == maxDist))
                {
                    deer.Score++;
                }
            }

            return reindeer.Max(r => r.Score);
        }

        /// <summary>
        /// Parses the list of capabilities into Reindeer objects
        /// </summary>
        /// <param name="capabilities">The list of reindeer and their capabilities</param>
        /// <returns>The list of initialized Reindeer objects</returns>
        private static List<Reindeer> ParseReindeer(string[] capabilities)
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
        private static readonly Regex capabilityRegex = new Regex(
            "(.*) can fly (\\d*) km/s for (\\d*) seconds, but then must rest for (\\d*) seconds.");

        /// <summary>
        /// Tracks the capabilities and score of a reindeer
        /// </summary>
        private class Reindeer
        {
            private readonly int speed;
            private readonly int cycleTime;
            private readonly int flyTime;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="name">The name of the reindeer</param>
            /// <param name="speed">The speed the reindeer can fly</param>
            /// <param name="flyTime">The number of seconds the reindeer can fly before needing to
            /// take a break</param>
            /// <param name="restTime">The number of seconds the reindeer needs to rest before
            /// resuming flying</param>
            public Reindeer(string name, int speed, int flyTime, int restTime)
            {
                Name = name;
                this.speed = speed;
                this.flyTime = flyTime;
                cycleTime = flyTime + restTime;
            }

            /// <summary>
            /// The name of the reindeer
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// The current score of the reindeer
            /// </summary>
            public int Score { get; set; }

            /// <summary>
            /// Computes the distance the reindeer travels in the specified amount of time
            /// </summary>
            /// <param name="seconds">The number of seconds</param>
            /// <returns>The distance in kilometers</returns>
            public int GetDistance(int seconds)
            {
                int wholeCycles = seconds / cycleTime;
                int remainingSeconds = seconds % cycleTime;
                return wholeCycles * flyTime * speed + Math.Min(remainingSeconds, flyTime) * speed;
            }
        }

        #region Data

        private const int sampleDuration = 1000;

        private static readonly string[] sampleInput =
        {
            "Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.",
            "Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."
        };

        private const int puzzleDuration = 2503;

        private static readonly string[] puzzleInput =
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
