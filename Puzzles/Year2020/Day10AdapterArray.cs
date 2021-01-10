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
    /// Day 10: Adapter Array
    /// https://adventofcode.com/2020/day/10
    /// </summary>
    [TestClass]
    public class Day10AdapterArray
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(35L, ExecutePart1(sampleInput1));
            Assert.AreEqual(220L, ExecutePart1(sampleInput2));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(2030L, ExecutePart1(puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(8L, ExecutePart2(sampleInput1));
            Assert.AreEqual(19208L, ExecutePart2(sampleInput2));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(42313823813632L, ExecutePart2(puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="adapters">The list of adapter joltages</param>
        /// <returns>The product of the 1- and 3-joltage jumps</returns>
        private static long ExecutePart1(int[] adapters)
        {
            var sorted = adapters.OrderBy(a => a);
            int jump1Count = 0;
            int jump3Count = 1; // the built-in adapter is 3 jolts
            int currJoltage = 0;
            foreach(int adapter in sorted)
            {
                if (adapter == currJoltage + 1)
                {
                    jump1Count++;
                }
                else if (adapter == currJoltage + 3)
                {
                    jump3Count++;
                }
                currJoltage = adapter;
            }
            return jump1Count * jump3Count;
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="adapters">The list of adapter joltages</param>
        /// <returns>The number of distinct adapter arrangements</returns>
        private static long ExecutePart2(int[] adapters)
        {
            var sorted = adapters.OrderBy(a => a).ToList();
            var possibilities = new Dictionary<int, long>();
            int builtInJoltage = sorted[^1] + 3;
            for (int i = sorted.Count - 1; i >= 0; i--)
            {
                long possibilityCount = 0;
                for (int j = i + 1; j <= i + 3; j++)
                {
                    if (j == sorted.Count && builtInJoltage - sorted[i] <= 3)
                    {
                        possibilityCount++;
                        break;
                    }
                    else if (j == sorted.Count)
                    {
                        break;
                    }
                    else if (sorted[j] - sorted[i] <= 3)
                    {
                        possibilityCount += possibilities[j];
                    }
                    else
                    {
                        break;
                    }
                }
                possibilities[i] = possibilityCount;
            }

            long sum = 0;
            for (int i = 0; i < 3; i++)
            {
                if (sorted[i] <= 3)
                {
                    sum += possibilities[i];
                }
            }
            return sum;
        }

        #region Data

        private static readonly int[] sampleInput1 =
        {
            16,
            10,
            15,
            5,
            1,
            11,
            7,
            19,
            6,
            12,
            4
        };

        private static readonly int[] sampleInput2 =
        {
            28,
            33,
            18,
            42,
            31,
            14,
            46,
            20,
            48,
            47,
            24,
            23,
            49,
            45,
            19,
            38,
            39,
            11,
            1,
            32,
            25,
            35,
            8,
            17,
            7,
            9,
            4,
            2,
            34,
            10,
            3
        };

        private static readonly int[] puzzleInput =
        {
            35,
            111,
            135,
            32,
            150,
            5,
            106,
            154,
            41,
            7,
            27,
            117,
            109,
            63,
            64,
            21,
            138,
            98,
            40,
            71,
            144,
            13,
            66,
            48,
            12,
            55,
            119,
            103,
            54,
            78,
            65,
            112,
            39,
            128,
            53,
            140,
            77,
            34,
            28,
            81,
            151,
            125,
            85,
            124,
            2,
            99,
            131,
            59,
            60,
            6,
            94,
            33,
            42,
            93,
            14,
            141,
            92,
            38,
            104,
            9,
            29,
            100,
            52,
            19,
            147,
            49,
            74,
            70,
            84,
            113,
            120,
            91,
            97,
            17,
            45,
            139,
            90,
            116,
            149,
            129,
            87,
            69,
            20,
            24,
            148,
            18,
            58,
            123,
            76,
            118,
            130,
            132,
            75,
            110,
            105,
            1,
            8,
            86
        };

        #endregion Data
    }
}
