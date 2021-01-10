// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Puzzles.Year2020
{
    /// <summary>
    /// Day 23: Crab Cups
    /// https://adventofcode.com/2020/day/23
    /// </summary>
    [TestClass]
    public class Day23CrabCups
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual("67384529", ExecutePart1(sampleInput));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual("65432978", ExecutePart1(puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(149245887792L, ExecutePart2(sampleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(287230227046L, ExecutePart2(puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="initialLabels">The starting labels</param>
        /// <returns>The labels of the cups after cup 1</returns>
        /// <remarks>
        /// TODO: this part takes ~30 seconds to execute. Find a faster solution?
        /// </remarks>
        private static string ExecutePart1(string initialLabels)
        {
            var (labels, nodeIndex) = GetInitialLabels(initialLabels);
            PerformMoves(labels, nodeIndex, 100);
            var builder = new StringBuilder();
            var node1 = nodeIndex[1];
            var node = node1.Next ?? labels.First;
            while (node != node1)
            {
                builder.Append(node.Value);
                node = node.Next ?? labels.First;
            }
            return builder.ToString();
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="initialLabels">The starting labels</param>
        /// <returns>The product of the labels of the two cups clockwise of cup 1</returns>
        private static long ExecutePart2(string initialLabels)
        {
            var (labels, nodeIndex) = GetInitialLabels(initialLabels);
            for (int i = 10; i <= 1000000; i++)
            {
                labels.AddLast(i);
                nodeIndex[i] = labels.Last;
            }
            PerformMoves(labels, nodeIndex, 10000000);
            var node = nodeIndex[1];
            var nodePlus1 = node.Next ?? labels.First;
            var nodePlus2 = nodePlus1.Next ?? labels.First;
            return (long)nodePlus1.Value * nodePlus2.Value;
        }

        /// <summary>
        /// Performs the specified number of moves
        /// </summary>
        /// <param name="labels">The list of labels</param>
        /// <param name="nodeIndex">The indexes of the nodes</param>
        /// <param name="count">The number of moves to make</param>
        private static void PerformMoves(LinkedList<int> labels, 
            Dictionary<int, LinkedListNode<int>> nodeIndex, int count)
        {
            int max = labels.Max();
            var currNode = labels.First;
            int[] pickedUp = new int[3];
            for (int i = 0; i < count; i++)
            {
                // Select the cards to pick up and remove them
                for (int j = 0; j < 3; j++)
                {
                    if (currNode == labels.Last)
                    {
                        pickedUp[j] = labels.First.Value;
                        labels.RemoveFirst();
                    }
                    else
                    {
                        pickedUp[j] = currNode.Next.Value;
                        labels.Remove(currNode.Next);
                    }
                }

                // Determine the target number
                int destination = currNode.Value - 1;
                if (destination == 0)
                {
                    destination = max;
                }
                while (pickedUp.Contains(destination))
                {
                    destination--;
                    if (destination == 0)
                    {
                        destination = max;
                    }
                }

                // Insert the picked up cups after the target and update the node index
                var destinationNode = nodeIndex[destination];
                labels.AddAfter(destinationNode, pickedUp[0]);
                nodeIndex[pickedUp[0]] = destinationNode.Next;
                labels.AddAfter(destinationNode.Next, pickedUp[1]);
                nodeIndex[pickedUp[1]] = destinationNode.Next.Next;
                labels.AddAfter(destinationNode.Next.Next, pickedUp[2]);
                nodeIndex[pickedUp[2]] = destinationNode.Next.Next.Next;

                // Move to the next cup, wrapping around to the beginning if necessary
                currNode = currNode.Next ?? labels.First;
            }
        }

        /// <summary>
        /// Creates the necessary structures from the initial labels
        /// </summary>
        /// <param name="initialLabels">The starting labels</param>
        /// <returns>The initialized data structures</returns>
        private static (LinkedList<int> labels, Dictionary<int, LinkedListNode<int>> nodeIndex) 
            GetInitialLabels(string initialLabels)
        {
            var labels = new LinkedList<int>();
            var nodeIndex = new Dictionary<int, LinkedListNode<int>>();
            foreach (char c in initialLabels)
            {
                labels.AddLast(int.Parse(c.ToString()));
                nodeIndex[labels.Last.Value] = labels.Last;
            }
            return (labels, nodeIndex);
        }

        private const string sampleInput = "389125467";

        private const string puzzleInput = "962713854";
    }
}
