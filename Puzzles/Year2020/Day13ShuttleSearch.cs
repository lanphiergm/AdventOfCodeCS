// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Puzzles.Year2020
{
    /// <summary>
    /// Day 13: Shuttle Search
    /// https://adventofcode.com/2020/day/13
    /// </summary>
    [TestClass]
    public class Day13ShuttleSearch
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(295L, ExecutePart1(sampleShuttleIds, sampleEarliestDeparture));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(2045L, ExecutePart1(puzzleShuttleIds, puzzleEarliestDeparture));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(1068781L, ExecutePart2(sampleShuttleIds));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(402251700208309L, ExecutePart2(puzzleShuttleIds));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="shuttleIds">The shuttle ID specification</param>
        /// <param name="earliestDeparture">The earliest departure timestamp</param>
        /// <returns>The product of the ID of the earliest bus and the number of minutes of
        /// wait time</returns>
        private static long ExecutePart1(string shuttleIds, int earliestDeparture)
        {
            var ids = ParseShuttleIds(shuttleIds);
            int shortestWait = int.MaxValue;
            int shortestWaitId = 0;
            foreach (int id in ids)
            {
                int wait = id - earliestDeparture % id;
                if (wait < shortestWait)
                {
                    shortestWait = wait;
                    shortestWaitId = id;
                }
            }

            return shortestWaitId * shortestWait;
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="shuttleIds">The shuttle ID specification</param>
        /// <returns>The earliest timestamp where all bus IDs match their positions</returns>
        private static long ExecutePart2(string shuttleIds)
        {
            var constraints = ParseShuttleConstraints(shuttleIds);
            long timestamp = 0;
            // This code works but takes hours to run
            //for (; timestamp <= long.MaxValue; timestamp += constraints[0].id)
            //{
            //    bool allConstraintsMet = true;
            //    for (int i = 1; i < constraints.Count; i++)
            //    {
            //        if (constraints[i].id - (timestamp % constraints[i].id) != constraints[i].constraint)
            //        {
            //            allConstraintsMet = false;
            //            break;
            //        }
            //    }
            //    if (allConstraintsMet)
            //    {
            //        break;
            //    }
            //}

            int[] n = constraints.Select(s => s.id).ToArray();
            int[] a = constraints.Select(s => ComputeRemainder(s)).ToArray();
            timestamp = ChineseRemainderTheorem.Solve(n, a);
            return timestamp;
        }

        /// <summary>
        /// Parses the shuttle ID specification into a list of integer IDs
        /// </summary>
        /// <param name="shuttleIds">The shuttle ID specification</param>
        /// <returns>The list of shuttle IDs</returns>
        private static List<int> ParseShuttleIds(string shuttleIds)
        {
            var ids = new List<int>();
            string[] idStrings = shuttleIds.Split(',');
            foreach (string id in idStrings)
            {
                if (id != "x")
                {
                    ids.Add(int.Parse(id));
                }
            }
            return ids;
        }

        /// <summary>
        /// Parses the shuttle ID specification into a list of IDs along with the constraint
        /// of where the ID needs to be positioned
        /// </summary>
        /// <param name="shuttleIds">The shuttle ID specification</param>
        /// <returns>The shuttle IDs and constraints</returns>
        private static List<(int constraint, int id)> ParseShuttleConstraints(string shuttleIds)
        {
            var constraints = new List<(int constraint, int id)>();
            string[] idStrings = shuttleIds.Split(',');
            for (int i = 0; i < idStrings.Length; i++)
            {
                if (idStrings[i] != "x")
                {
                    constraints.Add((i, int.Parse(idStrings[i])));
                }
            }
            return constraints;
        }

        /// <summary>
        /// Computes the remainders needed for the chinese remainder theorem
        /// </summary>
        /// <param name="shuttle">The shuttle constraint</param>
        /// <returns>The remainder</returns>
        private static int ComputeRemainder((int constraint, int id) shuttle)
        {
            int constraint = shuttle.constraint - (shuttle.constraint / shuttle.id) * shuttle.id;
            return constraint == 0 ? 0 : shuttle.id - constraint;
        }

        /// <summary>
        /// Shamelessly stolen from https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
        /// and updated to use long instead of int
        /// </summary>
        public static class ChineseRemainderTheorem
        {
            public static long Solve(int[] n, int[] a)
            {
                long prod = n.Aggregate(1L, (i, j) => i * j);
                long p;
                long sm = 0;
                for (int i = 0; i < n.Length; i++)
                {
                    p = prod / n[i];
                    sm += a[i] * ModularMultiplicativeInverse(p, n[i]) * p;
                }
                return sm % prod;
            }

            private static long ModularMultiplicativeInverse(long a, int mod)
            {
                long b = a % mod;
                for (int x = 1; x < mod; x++)
                {
                    if ((b * x) % mod == 1)
                    {
                        return x;
                    }
                }
                return 1;
            }
        }

        #region Data

        private const int sampleEarliestDeparture = 939;
        private const string sampleShuttleIds = "7,13,x,x,59,x,31,19";

        private const int puzzleEarliestDeparture = 1003681;
        private const string puzzleShuttleIds = "23,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,37,x,x,x,x," +
            "x,431,x,x,x,x,x,x,x,x,x,x,x,x,13,17,x,x,x,x,19,x,x,x,x,x,x,x,x,x,x,x,409,x,x,x,x,x," +
            "x,x,x,x,41,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,29";

        #endregion Data
    }
}
