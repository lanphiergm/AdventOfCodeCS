using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AdventOfCode.Problems
{
    abstract class ProblemBase<T> : IProblem
    {
        protected ProblemBase(ILogger logger, string name, int year, int day)
        {
            Logger = logger;
            Name = name;
            Day = day;
            Year = year;
        }

        public string Name { get; }
        public int Day { get; }
        public int Year { get; }
        protected ILogger Logger { get; }

        protected abstract T ExecutePart1();

        protected abstract T ExecutePart2();

        public string Execute(int part)
        {
            string result = null;
            if (part == 1)
            {
                result = ExecutePart1().ToString();
            }
            else if (part == 2)
            {
                result = ExecutePart2().ToString();
            }
            return result;
        }

        #region Helper Methods

        protected static List<List<string>> BuildPermutations(List<string> list)
        {
            var orders = new List<List<string>>();
            BuildPermutations(list, orders, 0, list.Count - 1);
            return orders;
        }

        private static void BuildPermutations(List<string> list, List<List<string>> orders, int k, int m)
        {
            if (k == m)
            {
                orders.Add(new List<string>(list));
            }
            else
            {
                for (int i = k; i <= m; i++)
                {
                    Swap(list, k, i);
                    BuildPermutations(list, orders, k + 1, m);
                    Swap(list, k, i);
                }
            }
        }

        private static void Swap(List<string> list, int a, int b)
        {
            string tmp = list[a];
            list[a] = list[b];
            list[b] = tmp;
        }
        #endregion Helper Methods
    }
}
