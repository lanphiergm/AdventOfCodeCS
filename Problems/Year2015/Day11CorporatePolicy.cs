using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.Year2015
{
    class Day11CorporatePolicy : ProblemBase<string>
    {
        public Day11CorporatePolicy(ILogger logger) : base(logger, "Corporate Policy", 2015, 11) { }

        protected override string ExecutePart1()
        {
            string password = initialPassword;
            do
            {
                password = IncrementPassword(password);
            } while (!IsValid(password));
            return password;
        }

        protected override string ExecutePart2()
        {
            throw new NotImplementedException();
        }

        private static string IncrementPassword(string password)
        {
            if (password[^1] == 'z')
            {
                password = IncrementPassword(password[0..^1]) + 'a';
            }
            else
            {
                password = password[0..^1] + IncrementCharacter(password[^1]);
            }

            return password;
        }

        private static char IncrementCharacter(char c)
        {
            do
            {
                c++;
            } while (invalidChars.Contains(c));
            return c;
        }

        private static bool IsValid(string password)
        {
            return HasStraight(password) && HasValidChars(password) && HasTwoPairs(password);
        }

        private static bool HasStraight(string password)
        {
            throw new NotImplementedException();
        }

        private static bool HasValidChars(string password)
        {
            throw new NotImplementedException();
        }

        private static bool HasTwoPairs(string password)
        {
            throw new NotImplementedException();
        }

        private static readonly List<char> invalidChars = new List<char>() { 'i', 'o', 'l' };

        private const string initialPassword = "cqjxjnds";
    }
}
