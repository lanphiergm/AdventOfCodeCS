// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Puzzles.Year2020
{
    /// <summary>
    /// Day 25: Combo Breaker
    /// https://adventofcode.com/2020/day/25
    /// </summary>
    [TestClass]
    public class Day25ComboBreaker
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(14897079, ExecutePart1(sampleCardPublicKey, sampleDoorPublicKey));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(7269858, ExecutePart1(puzzleCardPublicKey, puzzleDoorPublicKey));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(0, ExecutePart2());
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(0, ExecutePart2());
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="cardPublicKey">The public key for the card</param>
        /// <param name="doorPublicKey">The public key for the door</param>
        /// <returns>The encryption key handshake</returns>
        private static long ExecutePart1(int cardPublicKey, int doorPublicKey)
        {
            int cardLoopSize = DetermineLoopSize(cardPublicKey);
            return ComputeEncryptionKey(doorPublicKey, cardLoopSize);
        }

        /// <summary>
        /// There is no part 2 to this day's puzzle. Leaving it in so there is an even 100 tests
        /// for the year.
        /// </summary>
        /// <returns>zero</returns>
        private static long ExecutePart2()
        {
            return 0;
        }

        /// <summary>
        /// Determines the size of the loop for the given key
        /// </summary>
        /// <param name="publicKey">The public key to test</param>
        /// <returns>The loop size</returns>
        private static int DetermineLoopSize(long publicKey)
        {
            int loopSize = 0;
            long currKey = 1;
            while (publicKey != currKey)
            {
                currKey = Transform(currKey, initialSubjectNumber);
                loopSize++;
            }
            return loopSize;
        }

        /// <summary>
        /// Computes the encryption key for the given subject number and loop size
        /// </summary>
        /// <param name="subjectNumber">The subject number</param>
        /// <param name="loopSize">The size of the loop</param>
        /// <returns>The encryption key</returns>
        private static long ComputeEncryptionKey(int subjectNumber, int loopSize)
        {
            long encryptionKey = 1;
            for (int i = 0; i < loopSize; i++)
            {
                encryptionKey = Transform(encryptionKey, subjectNumber);
            }
            return encryptionKey;
        }

        /// <summary>
        /// Transforms the input number by the subject number
        /// </summary>
        /// <param name="input">The number to transform</param>
        /// <param name="subjectNumber">The subject number</param>
        /// <returns>The transformed number</returns>
        private static long Transform(long input, long subjectNumber)
        {
            input *= subjectNumber;
            return input % 20201227L;
        }

        private const int initialSubjectNumber = 7;

        #region Data

        private const int sampleCardPublicKey = 5764801;
        private const int sampleDoorPublicKey = 17807724;

        private const int puzzleCardPublicKey = 3469259;
        private const int puzzleDoorPublicKey = 13170438;

        #endregion Data
    }
}
