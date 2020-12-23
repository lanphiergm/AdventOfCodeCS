//#define USESAMPLE
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Problems.Year2020
{
    class Day22CrabCombat : ProblemBase<int>
    {
        public Day22CrabCombat(ILogger logger) : base(logger, "Crab Combat", 2020, 22) { }

        private readonly Queue<int> player1Deck = new Queue<int>();
        private readonly Queue<int> player2Deck = new Queue<int>();

        protected override int ExecutePart1()
        {
            Initialize();
            PlayCombat();
            int winningScore = player1Deck.Any() ? ScoreDeck(player1Deck) : ScoreDeck(player2Deck);
            return winningScore;
        }

        /// <summary>
        /// This solution took over 4 minutes to run. There is probably a shortcut that I missed.
        /// </summary>
        protected override int ExecutePart2()
        {
            Initialize();
            PlayRecursiveCombat(player1Deck, player2Deck);
            int winningScore = player1Deck.Any() ? ScoreDeck(player1Deck) : ScoreDeck(player2Deck);
            return winningScore;
        }

        private void PlayCombat()
        {
            while (player1Deck.Any() && player2Deck.Any())
            {
                int card1 = player1Deck.Dequeue();
                int card2 = player2Deck.Dequeue();
                if (card1 > card2)
                {
                    player1Deck.Enqueue(card1);
                    player1Deck.Enqueue(card2);
                }
                else
                {
                    player2Deck.Enqueue(card2);
                    player2Deck.Enqueue(card1);
                }
            }
        }

        private static int PlayRecursiveCombat(Queue<int> deck1, Queue<int> deck2)
        {
            var deck1History = new HashSet<int>();
            var deck2History = new HashSet<int>();
            while (deck1.Any() && deck2.Any())
            {
                //Taking the hash code of the queue itself doens't work
                int hash1 = deck1.ToArray().GetHashCode();
                int hash2 = deck2.ToArray().GetHashCode();
                if (deck1History.Contains(hash1) || deck2History.Contains(hash2))
                {
                    return 1;
                }
                deck1History.Add(hash1);
                deck2History.Add(hash2);
                int card1 = deck1.Dequeue();
                int card2 = deck2.Dequeue();
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

        private void Initialize()
        {
            player1Deck.Clear();
            player2Deck.Clear();
            foreach (int card in player1StartingDeck)
            {
                player1Deck.Enqueue(card);
            }
            foreach (int card in player2StartingDeck)
            {
                player2Deck.Enqueue(card);
            }
        }

        #region Data

#if USESAMPLE
        private static readonly int[] player1StartingDeck = { 9, 2, 6, 3, 1 };

        private static readonly int[] player2StartingDeck = { 5, 8, 4, 7, 10 };
#else
        private static readonly int[] player1StartingDeck =
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

        private static readonly int[] player2StartingDeck =
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
#endif
        #endregion Data
    }
}
