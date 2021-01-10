// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2015
{
    /// <summary>
    /// Day 11: Corporate Policy
    /// https://adventofcode.com/2015/day/11
    /// </summary>
    [TestClass]
    public class Day11CorporatePolicy
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual("abcdffaa", ExecutePart1(sampleInput));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual("cqjxxyzz", ExecutePart1(puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual("abcdffbb", ExecutePart2(sampleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual("cqkaabcc", ExecutePart2(puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="password">The initial password</param>
        /// <returns>The next password</returns>
        private static string ExecutePart1(string password) => GetNextValidPassword(password);

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="password">The initial password</param>
        /// <returns>The password after the next password</returns>
        private static string ExecutePart2(string password) => 
            GetNextValidPassword(ExecutePart1(password));

        /// <summary>
        /// Gets the next valid password after the specified one
        /// </summary>
        /// <param name="password">The initial password</param>
        /// <returns>The next valid password</returns>
        private static string GetNextValidPassword(string password)
        {
            do
            {
                password = IncrementPassword(password);
            } while (!IsValid(password));
            return password;
        }

        /// <summary>
        /// Increments the password to the next value
        /// </summary>
        /// <param name="password">The password to increment</param>
        /// <returns>The incremented password</returns>
        private static string IncrementPassword(string password)
        {
            // If we're at the end of the alphabet, increment the next beginning of the password
            // and set the last position back to 'a'
            if (password[^1] == 'z')
            {
                password = IncrementPassword(password[0..^1]) + 'a';
            }

            // Otherwise, increment the last position
            else
            {
                password = password[0..^1] + IncrementCharacter(password[^1]);
            }

            return password;
        }

        /// <summary>
        /// Increments a character to the next valid character
        /// </summary>
        /// <param name="c">The character to increment</param>
        /// <returns>The next valid character</returns>
        private static char IncrementCharacter(char c)
        {
            do
            {
                c++;
            } while (invalidChars.Contains(c));
            return c;
        }

        /// <summary>
        /// Determines if a password is valid
        /// </summary>
        /// <param name="password">The password to validate</param>
        /// <returns>true if it meets the rules; otherwise, false</returns>
        private static bool IsValid(string password)
        {
            return HasStraight(password) && HasValidChars(password) && HasTwoPairs(password);
        }

        /// <summary>
        /// Determines if the password contains a straight of three characters
        /// </summary>
        /// <param name="password">The password to examine</param>
        /// <returns>true if the password has a straight; otherwise, false</returns>
        private static bool HasStraight(string password)
        {
            for (int i = 0; i < password.Length - 2; i++)
            {
                if (password[i] == password[i+1] - 1 && password[i] == password[i+2] - 2)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the password contains only valid characters
        /// </summary>
        /// <param name="password">The password to examine</param>
        /// <returns>true if the password consists of only valid characters; otherwise, 
        /// false</returns>
        private static bool HasValidChars(string password)
        {
            foreach (char invalid in invalidChars)
            {
                if (password.Contains(invalid))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if the password contains two pairs
        /// </summary>
        /// <param name="password">The password to examine</param>
        /// <returns>true if the password contains two pairs; otherwise, false</returns>
        private static bool HasTwoPairs(string password)
        {
            int pairsFound = 0;

            for (int i = 0; i < password.Length - 1; i++)
            {
                if (password[i] == password[i+1])
                {
                    pairsFound++;
                    i++;
                }
                if (pairsFound >= 2)
                {
                    return true;
                }
            }

            return false;
        }

        private static readonly List<char> invalidChars = new List<char>() { 'i', 'o', 'l' };

        #region Data

        private const string sampleInput = "abcdefgh";

        private const string puzzleInput = "cqjxjnds";

        #endregion Data
    }
}
