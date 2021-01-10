// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2015
{
    /// <summary>
    /// Day 13: Knights of the Dinner Table
    /// https://adventofcode.com/2015/day/13
    /// </summary>
    [TestClass]
    public class Day13KnightsOfTheDinnerTable
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(330, ExecutePart1(sampleNames, sampleInput));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(664, ExecutePart1(puzzleNames, puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(286, ExecutePart2(sampleNames, sampleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(640, ExecutePart2(puzzleNames, puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="names">The names of the people</param>
        /// <param name="happinesses">The potential happiness values</param>
        /// <returns>The happiness improvement for the optimal seating arrangement</returns>
        private static int ExecutePart1(List<string> names, string[] happinesses)
        {
            var happinessValues = ComputeHappinesses(names, happinesses);
            return happinessValues.Max();
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="names">The names of the people</param>
        /// <param name="happinesses">The potential happiness values</param>
        /// <returns>The happiness improvement for the optimal seating arrangement including
        /// myself in the arrangement</returns>
        private static int ExecutePart2(List<string> names, string[] happinesses)
        {
            // By simply adding "Me" to the list, since it isn't present in the happinesses
            // array, the matrix will automatically have a zero for each interaction with "Me"
            names.Add("Me");
            var happinessValues = ComputeHappinesses(names, happinesses);
            return happinessValues.Max();
        }

        /// <summary>
        /// Computes the happiness values for each possible seating arrangement
        /// </summary>
        /// <param name="names">The names of the people</param>
        /// <param name="happinesses">The potential happiness values</param>
        /// <returns>The list of all happinesses for the seating arrangements</returns>
        private static List<int> ComputeHappinesses(List<string> names, string[] happinesses)
        {
            var orders = Utils.BuildPermutations(names);
            var happinessMatrix = ParseHappinesses(names, happinesses);
            var totalHappinesses = new List<int>();
            foreach (var order in orders)
            {
                int happiness = 0;
                for (int i = 0; i < order.Count - 1; i++)
                {
                    int personA = names.IndexOf(order[i]);
                    int personB = names.IndexOf(order[i + 1]);
                    happiness += happinessMatrix[personA, personB] +
                                 happinessMatrix[personB, personA];
                }
                int first = names.IndexOf(order[0]);
                int last = names.IndexOf(order[^1]);
                happiness += happinessMatrix[first, last] +
                             happinessMatrix[last, first];
                totalHappinesses.Add(happiness);
            }
            return totalHappinesses;
        }

        /// <summary>
        /// Parses the list of happiness values into a matrix
        /// </summary>
        /// <param name="names">The names of the people</param>
        /// <param name="happinesses">The potential happiness values</param>
        /// <returns></returns>
        private static int[,] ParseHappinesses(List<string> names, string[] happinesses)
        {
            int[,] happinessMatrix = new int[names.Count, names.Count];
            foreach (string happinessStr in happinesses)
            {
                var match = happinessRegex.Match(happinessStr);
                int personA = names.IndexOf(match.Groups[1].Value);
                int personB = names.IndexOf(match.Groups[4].Value);
                int happiness = int.Parse(match.Groups[3].Value);
                if (match.Groups[2].Value == "lose")
                {
                    happiness *= -1;
                }
                happinessMatrix[personA, personB] = happiness;
            }
            return happinessMatrix;
        }

        private static readonly Regex happinessRegex = new Regex("(.*) would (gain|lose) (\\d+) happiness units by sitting next to (.*)\\.");

        #region Data

        private static readonly List<string> sampleNames = new List<string>()
        {
            "Alice", "Bob", "Carol", "David"
        };

        private static readonly string[] sampleInput =
        {
            "Alice would gain 54 happiness units by sitting next to Bob.",
            "Alice would lose 79 happiness units by sitting next to Carol.",
            "Alice would lose 2 happiness units by sitting next to David.",
            "Bob would gain 83 happiness units by sitting next to Alice.",
            "Bob would lose 7 happiness units by sitting next to Carol.",
            "Bob would lose 63 happiness units by sitting next to David.",
            "Carol would lose 62 happiness units by sitting next to Alice.",
            "Carol would gain 60 happiness units by sitting next to Bob.",
            "Carol would gain 55 happiness units by sitting next to David.",
            "David would gain 46 happiness units by sitting next to Alice.",
            "David would lose 7 happiness units by sitting next to Bob.",
            "David would gain 41 happiness units by sitting next to Carol."
        };

        private static readonly List<string> puzzleNames = new List<string>()
        {
            "Alice", "Bob", "Carol", "David", "Eric", "Frank", "George", "Mallory"
        };

        private static readonly string[] puzzleInput =
        {
            "Alice would lose 2 happiness units by sitting next to Bob.",
            "Alice would lose 62 happiness units by sitting next to Carol.",
            "Alice would gain 65 happiness units by sitting next to David.",
            "Alice would gain 21 happiness units by sitting next to Eric.",
            "Alice would lose 81 happiness units by sitting next to Frank.",
            "Alice would lose 4 happiness units by sitting next to George.",
            "Alice would lose 80 happiness units by sitting next to Mallory.",
            "Bob would gain 93 happiness units by sitting next to Alice.",
            "Bob would gain 19 happiness units by sitting next to Carol.",
            "Bob would gain 5 happiness units by sitting next to David.",
            "Bob would gain 49 happiness units by sitting next to Eric.",
            "Bob would gain 68 happiness units by sitting next to Frank.",
            "Bob would gain 23 happiness units by sitting next to George.",
            "Bob would gain 29 happiness units by sitting next to Mallory.",
            "Carol would lose 54 happiness units by sitting next to Alice.",
            "Carol would lose 70 happiness units by sitting next to Bob.",
            "Carol would lose 37 happiness units by sitting next to David.",
            "Carol would lose 46 happiness units by sitting next to Eric.",
            "Carol would gain 33 happiness units by sitting next to Frank.",
            "Carol would lose 35 happiness units by sitting next to George.",
            "Carol would gain 10 happiness units by sitting next to Mallory.",
            "David would gain 43 happiness units by sitting next to Alice.",
            "David would lose 96 happiness units by sitting next to Bob.",
            "David would lose 53 happiness units by sitting next to Carol.",
            "David would lose 30 happiness units by sitting next to Eric.",
            "David would lose 12 happiness units by sitting next to Frank.",
            "David would gain 75 happiness units by sitting next to George.",
            "David would lose 20 happiness units by sitting next to Mallory.",
            "Eric would gain 8 happiness units by sitting next to Alice.",
            "Eric would lose 89 happiness units by sitting next to Bob.",
            "Eric would lose 69 happiness units by sitting next to Carol.",
            "Eric would lose 34 happiness units by sitting next to David.",
            "Eric would gain 95 happiness units by sitting next to Frank.",
            "Eric would gain 34 happiness units by sitting next to George.",
            "Eric would lose 99 happiness units by sitting next to Mallory.",
            "Frank would lose 97 happiness units by sitting next to Alice.",
            "Frank would gain 6 happiness units by sitting next to Bob.",
            "Frank would lose 9 happiness units by sitting next to Carol.",
            "Frank would gain 56 happiness units by sitting next to David.",
            "Frank would lose 17 happiness units by sitting next to Eric.",
            "Frank would gain 18 happiness units by sitting next to George.",
            "Frank would lose 56 happiness units by sitting next to Mallory.",
            "George would gain 45 happiness units by sitting next to Alice.",
            "George would gain 76 happiness units by sitting next to Bob.",
            "George would gain 63 happiness units by sitting next to Carol.",
            "George would gain 54 happiness units by sitting next to David.",
            "George would gain 54 happiness units by sitting next to Eric.",
            "George would gain 30 happiness units by sitting next to Frank.",
            "George would gain 7 happiness units by sitting next to Mallory.",
            "Mallory would gain 31 happiness units by sitting next to Alice.",
            "Mallory would lose 32 happiness units by sitting next to Bob.",
            "Mallory would gain 95 happiness units by sitting next to Carol.",
            "Mallory would gain 91 happiness units by sitting next to David.",
            "Mallory would lose 66 happiness units by sitting next to Eric.",
            "Mallory would lose 75 happiness units by sitting next to Frank.",
            "Mallory would lose 99 happiness units by sitting next to George."
        };

        #endregion Data
    }
}
