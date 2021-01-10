// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2015
{
    /// <summary>
    /// Day 9: All in a Single Night
    /// https://adventofcode.com/2015/day/9
    /// </summary>
    [TestClass]
    public class Day09AllInASingleNight
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(605, ExecutePart1(sampleCityNames, sampleDistances));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(117, ExecutePart1(puzzleCityNames, puzzleDistances));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(982, ExecutePart2(sampleCityNames, sampleDistances));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(909, ExecutePart2(puzzleCityNames, puzzleDistances));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="cityNames">The names of all the cities</param>
        /// <param name="cityDistances">The distances between the cities</param>
        /// <returns>The shortest route between all the cities</returns>
        private static int ExecutePart1(List<string> cityNames, string[] cityDistances)
        {
            return ComputeDistances(cityNames, cityDistances).Min();
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="cityNames">The names of all the cities</param>
        /// <param name="cityDistances">The distances between the cities</param>
        /// <returns>The longest route between all the cities</returns>
        private static int ExecutePart2(List<string> cityNames, string[] cityDistances)
        {
            return ComputeDistances(cityNames, cityDistances).Max();
        }

        /// <summary>
        /// Computes the distances between the cities for all permutations
        /// </summary>
        /// <param name="cityNames">The names of all the cities</param>
        /// <param name="cityDistances">The distances between the cities</param>
        /// <returns>The list of distances for each permutation of cities</returns>
        private static List<int> ComputeDistances(List<string> cityNames, string[] cityDistances)
        {
            var distanceMatrix = ParseDistances(cityNames, cityDistances);
            var orders = Utils.BuildPermutations(cityNames);
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

        /// <summary>
        /// Creates the matrix of distances between each city
        /// </summary>
        /// <param name="cityNames">The names of all the cities</param>
        /// <param name="cityDistances">The distances between the cities</param>
        /// <returns>The matrix of distances</returns>
        private static int[,] ParseDistances(List<string> cityNames, string[] cityDistances)
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

        #region Data

        private static readonly List<string> sampleCityNames = new List<string>()
        {
            "London", "Dublin", "Belfast"
        };

        private static readonly string[] sampleDistances =
        {
            "London to Dublin = 464",
            "London to Belfast = 518",
            "Dublin to Belfast = 141"
        };

        private static readonly List<string> puzzleCityNames = new List<string>()
        {
            "Faerun", "Tristram", "Tambi", "Norrath", "Snowdin", "Straylight", "AlphaCentauri", "Arbre"
        };

        private static readonly string[] puzzleDistances =
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
