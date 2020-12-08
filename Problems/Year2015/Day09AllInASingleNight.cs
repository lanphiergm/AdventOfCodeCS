using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Problems.Year2015
{
    class Day09AllInASingleNight : ProblemBase<int>
    {
        private List<int> totalDistances = null;
        public Day09AllInASingleNight(ILogger logger) : base(logger, "All in a Single Night", 2015, 9) { }

        protected override int ExecutePart1()
        {
            Initialize();
            return totalDistances.Min();
        }

        protected override int ExecutePart2()
        {
            Initialize();
            return totalDistances.Max();
        }

        private void Initialize()
        { 
            if (totalDistances == null)
            {
                totalDistances = ComputeDistances();
            }
        }

        private static List<int> ComputeDistances()
        {
            var distanceMatrix = ParseDistances();
            var orders = BuildPermutations(cityNames);
            var totalDistances = new List<int>();
            foreach (var order in orders)
            {
                int distance = 0;
                for (int i = 0; i < order.Count - 1; i++)
                {
                    distance += distanceMatrix[cityNames.IndexOf(order[i]), cityNames.IndexOf(order[i + 1])];
                }
                totalDistances.Add(distance);
            }
            return totalDistances;
        }

        private static int[,] ParseDistances()
        {
            int[,] distanceMatrix = new int[cityNames.Count, cityNames.Count];
            foreach (string distanceStr in cityDistances)
            {
                var match = distanceRegex.Match(distanceStr);
                int cityA = cityNames.IndexOf(match.Groups[1].Value);
                int cityB = cityNames.IndexOf(match.Groups[2].Value);
                int distance = int.Parse(match.Groups[3].Value);
                distanceMatrix[cityA, cityB] = distance;
                distanceMatrix[cityB, cityA] = distance;
            }
            return distanceMatrix;
        }

        private static readonly Regex distanceRegex = new Regex("(.*) to (.*) = (.*)");

        private static readonly List<string> cityNames = new List<string>()
        {
            "Faerun", "Tristram", "Tambi", "Norrath", "Snowdin", "Straylight", "AlphaCentauri", "Arbre"
        };

        #region Data

        private static readonly string[] cityDistances =
        {
            "Faerun to Tristram = 65",
            "Faerun to Tambi = 129",
            "Faerun to Norrath = 144",
            "Faerun to Snowdin = 71",
            "Faerun to Straylight = 137",
            "Faerun to AlphaCentauri = 3",
            "Faerun to Arbre = 149",
            "Tristram to Tambi = 63",
            "Tristram to Norrath = 4",
            "Tristram to Snowdin = 105",
            "Tristram to Straylight = 125",
            "Tristram to AlphaCentauri = 55",
            "Tristram to Arbre = 14",
            "Tambi to Norrath = 68",
            "Tambi to Snowdin = 52",
            "Tambi to Straylight = 65",
            "Tambi to AlphaCentauri = 22",
            "Tambi to Arbre = 143",
            "Norrath to Snowdin = 8",
            "Norrath to Straylight = 23",
            "Norrath to AlphaCentauri = 136",
            "Norrath to Arbre = 115",
            "Snowdin to Straylight = 101",
            "Snowdin to AlphaCentauri = 84",
            "Snowdin to Arbre = 96",
            "Straylight to AlphaCentauri = 107",
            "Straylight to Arbre = 14",
            "AlphaCentauri to Arbre = 46"
        };

        #endregion Data
    }
}
