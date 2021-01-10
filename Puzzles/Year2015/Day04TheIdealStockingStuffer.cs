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
    /// Day 4: The Ideal Stocking Stuffer
    /// https://adventofcode.com/2015/day/4
    /// </summary>
    [TestClass]
    public class Day04TheIdealStockingStuffer
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(1048970, ExecutePart1(sampleInput));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(282749, ExecutePart1(puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            //No sample answer was provided
            Assert.AreEqual(1, 1);
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(9962624, ExecutePart2(puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="secretKey">The secret key</param>
        /// <returns>The lowest positive number that produces a hash starting with 5 zeros</returns>
        private static int ExecutePart1(string secretKey)
        {
            return FindHash(secretKey, string.Empty.PadLeft(5, '0'));
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="secretKey">The secret key</param>
        /// <returns>The lowest positive number that produces a hash starting with 6 zeros</returns>
        /// <remarks>
        /// TODO: This puzzle takes ~15 seconds to run. Find a faster solution?
        /// </remarks>
        private static int ExecutePart2(string secretKey)
        {
            return FindHash(secretKey, string.Empty.PadLeft(6, '0'));
        }

        /// <summary>
        /// Finds the lowest positive number that provides a hash with the specified prefix
        /// </summary>
        /// <param name="secretKey">The secret key to use</param>
        /// <param name="prefix">The prefix to find</param>
        /// <returns>The number that produces the hash</returns>
        private static int FindHash(string secretKey, string prefix)
        {
            int answer = 0;

            for (int i = 0; i < int.MaxValue; i++)
            {
                string md5 = CreateMD5(secretKey + i.ToString());
                if (md5.StartsWith(prefix))
                {
                    answer = i;
                    break;
                }
            }

            return answer;
        }

        /// <summary>
        /// Creates an MD5 hash
        /// </summary>
        /// <param name="input">The string to hash</param>
        /// <returns>The computed hash</returns>
        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private static readonly System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

        #region Data

        private const string sampleInput = "pqrstuv";

        private const string puzzleInput = "yzbqklnj";

        #endregion Data
    }
}
