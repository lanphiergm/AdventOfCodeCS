using System;
using System.Text;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Problems.Year2015
{
    class Day04TheIdealStockingStuffer : ProblemBase<int>
    {
        public Day04TheIdealStockingStuffer(ILogger logger) : base(logger, "The Ideal Stocking Stuffer", 2015, 4) { }

        protected override int ExecutePart1()
        {
            return FindHash(string.Empty.PadLeft(5, '0'));
        }

        protected override int ExecutePart2()
        {
            return FindHash(string.Empty.PadLeft(6, '0'));
        }

        private static int FindHash(string prefix)
        {
            int answer = 0;

            for (int i = 0; i < int.MaxValue; i++)
            {
                string md5 = CreateMD5(secretKey + i.ToString());
                if (md5.StartsWith(prefix))
                {
                    answer = i;
                    break;
                }
            }

            return answer;
        }

        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private static readonly System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        private const string secretKey = "yzbqklnj";
    }
}
