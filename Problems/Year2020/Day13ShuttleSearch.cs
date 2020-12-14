using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Problems.Year2020
{
    class Day13ShuttleSearch : ProblemBase<long>
    {
        public Day13ShuttleSearch(ILogger logger) : base(logger, "Shuttle Search", 2020, 13) { }

        private const int earliestDeparture = 1003681;
        private const string shuttleIdString = "23,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,37,x,x,x,x,x," +
            "431,x,x,x,x,x,x,x,x,x,x,x,x,13,17,x,x,x,x,19,x,x,x,x,x,x,x,x,x,x,x,409,x,x,x,x,x,x," +
            "x,x,x,41,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,29";

        protected override long ExecutePart1()
        {
            var ids = ParseShuttleIds();
            int shortestWait = int.MaxValue;
            int shortestWaitId = 0;
            foreach (int id in ids)
            {
                int wait = id - earliestDeparture % id;
                if (wait < shortestWait)
                {
                    shortestWait = wait;
                    shortestWaitId = id;
                }
            }

            return shortestWaitId * shortestWait;
        }

        protected override long ExecutePart2()
        {
            var constraints = ParseShuttleConstraints();
            long timestamp = 0;
            // This code works but is way too slow
            //for (; timestamp <= long.MaxValue; timestamp += constraints[0].id)
            //{
            //    bool allConstraintsMet = true;
            //    for (int i = 1; i < constraints.Count; i++)
            //    {
            //        if (constraints[i].id - (timestamp % constraints[i].id) != constraints[i].constraint)
            //        {
            //            allConstraintsMet = false;
            //            break;
            //        }
            //    }
            //    if (allConstraintsMet)
            //    {
            //        break;
            //    }
            //}

            int[] n = constraints.Select(s => s.id).ToArray();
            int[] a = constraints.Select(s => ComputeRemainder(s)).ToArray();
            timestamp = ChineseRemainderTheorem.Solve(n, a);
            return timestamp;
        }

        private static List<int> ParseShuttleIds()
        {
            var ids = new List<int>();
            string[] idStrings = shuttleIdString.Split(',');
            foreach (string id in idStrings)
            {
                if (id != "x")
                {
                    ids.Add(int.Parse(id));
                }
            }
            return ids;
        }

        private static List<(int constraint, int id)> ParseShuttleConstraints()
        {
            var constraints = new List<(int constraint, int id)>();
            string[] idStrings = shuttleIdString.Split(',');
            for (int i = 0; i < idStrings.Length; i++)
            {
                if (idStrings[i] != "x")
                {
                    constraints.Add((i, int.Parse(idStrings[i])));
                }
            }
            return constraints;
        }

        private static int ComputeRemainder((int constraint, int id) shuttle)
        {
            int constraint = shuttle.constraint - (shuttle.constraint / shuttle.id) * shuttle.id;
            return constraint == 0 ? 0 : shuttle.id - constraint;
        }

        /// <summary>
        /// Shamelessly stolen from https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
        /// and updated to use long instead of int
        /// </summary>
        public static class ChineseRemainderTheorem
        {
            public static long Solve(int[] n, int[] a)
            {
                long prod = n.Aggregate(1L, (i, j) => i * j);
                long p;
                long sm = 0;
                for (int i = 0; i < n.Length; i++)
                {
                    p = prod / n[i];
                    sm += a[i] * ModularMultiplicativeInverse(p, n[i]) * p;
                }
                return sm % prod;
            }

            private static long ModularMultiplicativeInverse(long a, int mod)
            {
                long b = a % mod;
                for (int x = 1; x < mod; x++)
                {
                    if ((b * x) % mod == 1)
                    {
                        return x;
                    }
                }
                return 1;
            }
        }
    }
}
