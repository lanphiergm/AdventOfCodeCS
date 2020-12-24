using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Problems.Year2020
{
    class Day23CrabCups : ProblemBase<string>
    {
        public Day23CrabCups(ILogger logger) : base(logger, "Crab Cups", 2020, 23) { }

        protected override string ExecutePart1()
        {
            var (labels, nodeIndex) = GetInitialLabels();
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

        protected override string ExecutePart2()
        {
            var (labels, nodeIndex) = GetInitialLabels();
            for (int i = 10; i <= 1000000; i++)
            {
                labels.AddLast(i);
                nodeIndex[i] = labels.Last;
            }
            PerformMoves(labels, nodeIndex, 10000000);
            var node = nodeIndex[1];
            var nodePlus1 = node.Next ?? labels.First;
            var nodePlus2 = nodePlus1.Next ?? labels.First;
            long product = (long)nodePlus1.Value * nodePlus2.Value;
            return product.ToString();
        }

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

                // Move the current cup
                currNode = currNode.Next ?? labels.First;
            }
        }

        private static (LinkedList<int> labels, Dictionary<int, LinkedListNode<int>> nodeIndex) GetInitialLabels()
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

        private const string initialLabels = "962713854";
    }
}
