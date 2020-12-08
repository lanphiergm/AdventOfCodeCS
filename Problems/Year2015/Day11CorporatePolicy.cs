using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AdventOfCode.Problems.Year2015
{
    class Day11CorporatePolicy : ProblemBase<string>
    {
        public Day11CorporatePolicy(ILogger logger) : base(logger, "Corporate Policy", 2015, 11) { }

        protected override string ExecutePart1() => GetNextValidPassword(initialPassword);

        protected override string ExecutePart2() => GetNextValidPassword(ExecutePart1());

        private static string GetNextValidPassword(string password)
        {
            do
            {
                password = IncrementPassword(password);
            } while (!IsValid(password));
            return password;
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
            for (int i = 0; i < password.Length - 2; i++)
            {
                if (password[i] == password[i+1] - 1 && password[i] == password[i+2] - 2)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool HasValidChars(string password)
        {
            foreach (char invalid in invalidChars)
            {
                if (password.Contains(invalid))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool HasTwoPairs(string password)
        {
            int pairsFound = 0;

            for (int i = 0; i < password.Length - 1; i++)
            {
                if (password[i] == password[i+1])
                {
                    pairsFound++;
                    i++;
                }
                if (pairsFound >= 2)
                {
                    return true;
                }
            }

            return false;
        }

        private static readonly List<char> invalidChars = new List<char>() { 'i', 'o', 'l' };

        private const string initialPassword = "cqjxjnds";
    }
}
