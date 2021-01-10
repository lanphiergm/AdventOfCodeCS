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
    /// Day 22: Crab Combat
    /// https://adventofcode.com/2020/day/22
    /// </summary>
    [TestClass]
    public class Day22CrabCombat
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(306, ExecutePart1(sampleDeck1, sampleDeck2));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(32448, ExecutePart1(puzzleDeck1, puzzleDeck2));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(291, ExecutePart2(sampleDeck1, sampleDeck2));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(32949, ExecutePart2(puzzleDeck1, puzzleDeck2));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="startingDeck1">The cards in player 1's starting deck</param>
        /// <param name="startingDeck2">The cards in player 2's starting deck</param>
        /// <returns>The winning player's score</returns>
        private static int ExecutePart1(int[] startingDeck1, int[] startingDeck2)
        {
            var deck1 = new Queue<int>(startingDeck1);
            var deck2 = new Queue<int>(startingDeck2);
            PlayCombat(deck1, deck2);
            int winningScore = deck1.Any() ? ScoreDeck(deck1) : ScoreDeck(deck2);
            return winningScore;
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="startingDeck1">The cards in player 1's starting deck</param>
        /// <param name="startingDeck2">The cards in player 2's starting deck</param>
        /// <returns>The winning player's score</returns>
        private static int ExecutePart2(int[] startingDeck1, int[] startingDeck2)
        {
            var deck1 = new Queue<int>(startingDeck1);
            var deck2 = new Queue<int>(startingDeck2);
            PlayRecursiveCombat(deck1, deck2);
            int winningScore = deck1.Any() ? ScoreDeck(deck1) : ScoreDeck(deck2);
            return winningScore;
        }

        /// <summary>
        /// Plays rounds of the game until one deck is empty
        /// </summary>
        /// <param name="deck1">Player 1's deck</param>
        /// <param name="deck2">Player 2's deck</param>
        private static void PlayCombat(Queue<int> deck1, Queue<int> deck2)
        {
            while (deck1.Any() && deck2.Any())
            {
                int card1 = deck1.Dequeue();
                int card2 = deck2.Dequeue();
                if (card1 > card2)
                {
                    deck1.Enqueue(card1);
                    deck1.Enqueue(card2);
                }
                else
                {
                    deck2.Enqueue(card2);
                    deck2.Enqueue(card1);
                }
            }
        }

        /// <summary>
        /// Plays rounds of the game using the recursive rules until one deck is empty
        /// </summary>
        /// <param name="deck1">Player 1's deck</param>
        /// <param name="deck2">Player 2's deck</param>
        private static int PlayRecursiveCombat(Queue<int> deck1, Queue<int> deck2)
        {
            var deck1History = new HashSet<int>();
            var deck2History = new HashSet<int>();
            while (deck1.Any() && deck2.Any())
            {
                // Recursive end condition
                if (HistoryContains(deck1History, deck1) && HistoryContains(deck2History, deck2))
                {
                    return 1;
                }

                int card1 = deck1.Dequeue();
                int card2 = deck2.Dequeue();

                // Determine if we play normally or need to play a recursive game
                int roundWinner;
                if (deck1.Count >= card1 && deck2.Count >= card2)
                {
                    roundWinner = PlayRecursiveCombat(new Queue<int>(deck1.Take(card1)), 
                        new Queue<int>(deck2.Take(card2)));
                }
                else
                {
                    roundWinner = card1 > card2 ? 1 : 2;
                }

                // Put the cards back in the winner's deck
                if (roundWinner == 1)
                {
                    deck1.Enqueue(card1);
                    deck1.Enqueue(card2);
                }
                else
                {
                    deck2.Enqueue(card2);
                    deck2.Enqueue(card1);
                }
            }

            return deck1.Any() ? 1 : 2;
        }

        /// <summary>
        /// Checks to see if this deck contents have been repeated before
        /// </summary>
        /// <param name="history">The hash codes of all previously played decks</param>
        /// <param name="deck">The current deck</param>
        /// <returns>true if the deck is a repeat</returns>
        private static bool HistoryContains(HashSet<int> history, Queue<int> deck)
        {
            return !history.Add(ComputeHash(deck));
        }

        /// <summary>
        /// Computes the hash code for the deck
        /// </summary>
        /// <param name="deck">The deck to compute</param>
        /// <returns>The order-dependent hash of the deck</returns>
        private static int ComputeHash(Queue<int> deck)
        {
            int hash = 27;
            unchecked
            {
                foreach (int card in deck)
                {
                    hash = (13 * hash) + card.GetHashCode();
                }
            }
            return hash;
        }

        /// <summary>
        /// Computes the score for a deck
        /// </summary>
        /// <param name="deck">The deck to score</param>
        /// <returns>The score for the deck</returns>
        private static int ScoreDeck(Queue<int> deck)
        {
            int score = 0;
            int currCardValue = deck.Count;
            foreach (int card in deck)
            {
                score += card * currCardValue--;
            }
            return score;
        }

        #region Data

        private static readonly int[] sampleDeck1 = { 9, 2, 6, 3, 1 };

        private static readonly int[] sampleDeck2 = { 5, 8, 4, 7, 10 };

        private static readonly int[] puzzleDeck1 =
        {
            12,
            48,
            26,
            22,
            44,
            16,
            31,
            19,
            30,
            10,
            40,
            47,
            21,
            27,
            2,
            46,
            9,
            15,
            23,
            6,
            50,
            28,
            5,
            42,
            34
        };

        private static readonly int[] puzzleDeck2 =
        {
            14,
            45,
            4,
            24,
            1,
            7,
            36,
            29,
            38,
            33,
            3,
            13,
            11,
            17,
            39,
            43,
            8,
            41,
            32,
            37,
            35,
            49,
            20,
            18,
            25
        };

        #endregion Data
    }
}
