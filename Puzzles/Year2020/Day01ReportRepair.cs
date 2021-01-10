// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AdventOfCode.Puzzles.Year2020
{
    /// <summary>
    /// Day 1: Report Repair
    /// https://adventofcode.com/2020/day/1
    /// </summary>
    [TestClass]
    public class Day01ReportRepair
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(514579, ExecutePart1(sampleInput));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(437931, ExecutePart1(puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(241861950, ExecutePart2(sampleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(157667328, ExecutePart2(puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="entries">The expense report entries</param>
        /// <returns>The product of the two entries that sum to 2020</returns>
        private static int ExecutePart1(int[] entries)
        {
            int answer = 0;
            for (int i = 0; i < entries.Length - 1; i++)
            {
                int a = entries[i];
                for (int j = i + 1; j < entries.Length; j++)
                {
                    int b = entries[j];
                    if (a + b == 2020)
                    {
                        Console.WriteLine("a: {0}, b: {1}", a, b);
                        answer = (a * b);
                        break;
                    }
                }
                if (answer != 0)
                {
                    break;
                }
            }
            return answer;
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="entries">The expense report entries</param>
        /// <returns>The product of the three entries that sum to 2020</returns>
        private static int ExecutePart2(int[] entries)
        {
            int answer = 0;
            for (int i = 0; i < entries.Length - 2; i++)
            {
                int a = entries[i];
                for (int j = i + 1; j < entries.Length - 1; j++)
                {
                    int b = entries[j];
                    for (int k = j + 1; k < entries.Length; k++)
                    {
                        int c = entries[k];
                        if (a + b + c == 2020)
                        {
                            Console.WriteLine("a: {0}, b: {1}, c: {2}", a, b, c);
                            answer = (a * b * c);
                            break;
                        }
                    }
                }
                if (answer != 0)
                {
                    break;
                }
            }
            return answer;
        }

        #region Data

        private static readonly int[] sampleInput = { 1721, 979, 366, 299, 675, 1456, };

        private static readonly int[] puzzleInput = 
        {
            1313, 1968, 1334, 1566, 820, 1435, 1369, 1230, 1383, 1816, 1396, 1974, 1911, 1989,
            1824, 1430, 1709, 1204, 1792, 1800, 1703, 2009, 1467, 1400, 1315, 1985, 1598, 1215,
            1574, 1770, 1870, 1352, 1544, 1339, 188, 1347, 1986, 2003, 1538, 1839, 1688, 1350,
            1191, 1961, 1578, 1946, 1548, 1975, 1745, 1631, 1390, 1811, 1586, 1409, 247, 1600,
            1565, 1929, 1854, 1602, 1773, 1815, 1887, 1689, 1266, 1573, 1534, 1939, 1909, 1273,
            1386, 1713, 1268, 1611, 1348, 1478, 1857, 1916, 1113, 936, 1603, 1716, 1875, 1855,
            1834, 1701, 1279, 1346, 1503, 1797, 1287, 1447, 1475, 1950, 1614, 1261, 1442, 1299,
            1465, 896, 1481, 1804, 1931, 1849, 1675, 1726, 355, 1485, 1343, 1697, 1735, 1858,
            1205, 1345, 1281, 253, 1808, 1557, 1964, 1771, 1891, 1583, 1896, 1398, 1930, 1258,
            1338, 1208, 1328, 1493, 1963, 1374, 1212, 1223, 1501, 2004, 1591, 1954, 115, 1972,
            1814, 1643, 1270, 1349, 1297, 1399, 1969, 1237, 1228, 1379, 1779, 1765, 1427, 1464,
            1247, 1967, 1577, 1719, 1559, 1274, 1879, 1504, 1732, 1277, 1758, 1721, 1936, 1605,
            1358, 1505, 1411, 1823, 1576, 1682, 1439, 1901, 1940, 1760, 1414, 1193, 1900, 1990,
            1781, 1801, 1239, 1729, 1360, 1780, 1848, 1468, 1484, 1280, 1278, 1851, 1903, 1894,
            1731, 1451, 549, 1570
        };

        #endregion Data
    }
}
