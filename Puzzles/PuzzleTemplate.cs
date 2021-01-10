// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Puzzles
{
    /// <summary>
    /// Day : 
    /// https://adventofcode.com/202/day/
    /// </summary>
    [TestClass]
    public class PuzzleTemplate
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(1, ExecutePart1(sampleInput));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(1, ExecutePart1(puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(1, ExecutePart2(sampleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(1, ExecutePart2(puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static int ExecutePart1(string[] input)
        {
            return input.Length;
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static int ExecutePart2(string[] input)
        {
            return input.Length;
        }

        #region Data

        private static readonly string[] sampleInput = new string[]
        {
            ""
        };

        private static readonly string[] puzzleInput = new string[]
        {
            ""
        };

        #endregion Data
    }
}
