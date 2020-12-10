using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Problems.Year2020
{
    class Day10AdapterArray : ProblemBase<long>
    {
        public Day10AdapterArray(ILogger logger) : base(logger, "Adapter Array", 2020, 10) { }

        protected override long ExecutePart1()
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

        protected override long ExecutePart2()
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
                if (sorted[i] < 3)
                {
                    sum += possibilities[i];
                }
            }
            return sum;
        }

        #region Data

        private static readonly int[] adapters =
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
