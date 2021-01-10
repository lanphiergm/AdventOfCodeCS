// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace AdventOfCode.Puzzles.Year2015
{
    /// <summary>
    /// Day 10: Elves Look, Elves Say
    /// https://adventofcode.com/2015/day/10
    /// </summary>
    [TestClass]
    public class Day10ElvesLookElvesSay
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(6, LookAndSay(sampleInput, sampleIterations).Length);
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(252594, LookAndSay(puzzleInput, puzzleIterations1).Length);
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(6, LookAndSay(sampleInput, sampleIterations).Length);
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(3579328, LookAndSay(puzzleInput, puzzleIterations2).Length);
        }

        /// <summary>
        /// Performs look and say on the initial string the specified number of times
        /// </summary>
        /// <param name="initial">The initial string</param>
        /// <param name="iterations">The number of iterations</param>
        /// <returns>The final string</returns>
        private static string LookAndSay(string initial, int iterations)
        {
            if (iterations == 0)
            {
                return initial;
            }
            var builder = new StringBuilder();
            for (int i = 0; i < initial.Length; i++)
            {
                char curr = initial[i];
                int count = 1;
                for (int j = i + 1; j < initial.Length; j++)
                {
                    if (initial[j] == curr)
                    {
                        i++;
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                builder.Append(count);
                builder.Append(curr);
            }
            return LookAndSay(builder.ToString(), iterations - 1);
        }

        #region Data

        private const string sampleInput = "1";
        private const int sampleIterations = 5;

        private const string puzzleInput = "1113222113";
        private const int puzzleIterations1 = 40;
        private const int puzzleIterations2 = 50;

        #endregion Data
    }
}
