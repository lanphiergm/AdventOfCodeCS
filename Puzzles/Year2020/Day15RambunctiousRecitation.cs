// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2020
{
    /// <summary>
    /// Day 15: Rambunctious Recitation
    /// https://adventofcode.com/2020/day/15
    /// </summary>
    [TestClass]
    public class Day15RambunctiousRecitation
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(436, ExecutePart1(sampleInput));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(517, ExecutePart1(puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(175594, ExecutePart2(sampleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(1047739, ExecutePart2(puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="startingNumbers">The starting numbers</param>
        /// <returns>The number at the 2020th position</returns>
        private static int ExecutePart1(int[] startingNumbers)
        {
            return FindNumberAtPosition(startingNumbers, 2020);
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="startingNumbers">The starting numbers</param>
        /// <returns>The number at the 30,000,000th position</returns>
        /// <remarks>
        /// TODO: this part takes ~2.5 seconds to execute. Find a faster solution?
        /// </remarks>
        private static int ExecutePart2(int[] startingNumbers)
        {
            return FindNumberAtPosition(startingNumbers, 30_000_000);
        }

        /// <summary>
        /// Finds the number at the given position given the starting numbers
        /// </summary>
        /// <param name="startingNumbers">The starting numbers</param>
        /// <param name="position">The desired position</param>
        /// <returns>The number at the given position</returns>
        private static int FindNumberAtPosition(int[] startingNumbers, int position)
        {
            var indices = new Dictionary<int, int>();
            for (int i = 0; i < startingNumbers.Length - 1; i++)
            {
                indices[startingNumbers[i]] = i;
            }
            int last = startingNumbers[^1];
            for (int i = startingNumbers.Length - 1; i < position - 1; i++)
            {
                if (indices.TryGetValue(last, out int lastIndex))
                {
                    indices[last] = i;
                    last = i - lastIndex;
                }
                else
                {
                    indices[last] = i;
                    last = 0;
                }
            }
            return last;
        }

        #region Data

        private static readonly int[] sampleInput = { 0, 3, 6 };

        private static readonly int[] puzzleInput = { 5, 2, 8, 16, 18, 0, 1 };

        #endregion Data
    }
}
